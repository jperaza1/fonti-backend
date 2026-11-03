using cod.application.Common.Clases;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Data;
using cod.domain.Features;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Correlative.Commands.GenerateCorrelative
{
    public class GenerateCorrelativeCommand : BaseCommand, IGenerateCorrelativeCommand
    {
        private readonly AuthService _authService;
        public GenerateCorrelativeCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task<GenerateCorrelativeModel> Execute()
        {
            var timezone = _authService.ClientTimezone;

            var nowDate = DateService.GetCLientLocalTime(Clock.Now, timezone);

            var existCorrelative = await _repository.QueryActive<domain.Data.Correlative>()
                .Where(f => f.StartDate.Date <= nowDate.Date && f.EndDate.Date >= nowDate.Date)
                .Where(f => f.Current < f.End)
                .ToListAsync();

            if(existCorrelative.Any())
            {
                var correlative = existCorrelative.First();

                var newResponse = new GenerateCorrelativeModel
                {
                    Id = correlative.Id,
                    Current = correlative.Current,
                    Reference = $"{correlative.EmissionPoint}-{correlative.Establishment}-{correlative.DocumentType}-{correlative.Current.ToString("D8")}"
                };

                return newResponse;
            }

            throw new ApplicationException("CANCELED", "CORRELATIVO NO CONFIGURADO", this._exceptionFactory.Exceptions);
        }
    }
}
