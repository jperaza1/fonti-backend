using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Payment.Commands.UpdatePaymentNote
{
    public class UpdatePaymentNoteCommand : BaseCommand, IUpdatePaymentNoteCommand
    {
        public UpdatePaymentNoteCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id, PaymentModel model)
        {
            if (string.IsNullOrEmpty(model.Notes))
                throw new ApplicationException("Se Requiere una nota", "PAGO", this._exceptionFactory.Exceptions);

            var payment = await _repository.AllAsync<domain.Finance.Payment>()
                .Where(f => f.Id == id).FirstOrDefaultAsync();

            if (payment != null)
            {
                payment.Notes = model.Notes;

                _repository.UpdateSync(payment);

                await _repository.CommitAsync();
            }
        }
    }
}
