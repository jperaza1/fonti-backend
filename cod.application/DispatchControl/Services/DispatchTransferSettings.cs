using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Services
{
    public class DispatchTransferSettings : IDispatchTransferSettings
    {
        public const string Key = "DISPATCH_TRANSFER";

        private readonly IRepository _repository;

        public DispatchTransferSettings(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsEnabled()
        {
            var value = await _repository
                .QueryActive<domain.General.Configuration>(c => c.Key == Key)
                .Select(c => c.Value)
                .FirstOrDefaultAsync();

            if (value != null)
                return string.Equals(value.Trim(), "true", StringComparison.OrdinalIgnoreCase);

            var exists = await _repository.AnyAsync<domain.General.Configuration>(c => c.Key == Key);
            if (!exists)
            {
                _repository.InsertAsync(new domain.General.Configuration
                {
                    Key = Key,
                    Value = "false"
                });
                await _repository.CommitAsync();
            }

            return false;
        }
    }
}
