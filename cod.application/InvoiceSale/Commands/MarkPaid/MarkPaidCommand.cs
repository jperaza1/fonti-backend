using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.InvoiceSale.Commands.MarkPaid
{
    public class MarkPaidCommand : BaseCommand, IMarkPaidCommand
    {
        private readonly AuthService _authService;
        public MarkPaidCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(int id)
        {
            var sale = await _repository.QueryActive<domain.Finance.SaleInvoice>()
                .Where(f => f.Id == id).FirstOrDefaultAsync();

            if (sale == null)
                throw new ApplicationException("CANCELED", "Factura no encontrada");

            sale.PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Paid;
            sale.Paid = true;
            sale.PaidAt = Clock.Now;
            sale.PaidUser = _authService.User;
            sale.Balance = 0;

            _repository.UpdateSync(sale);

            await _repository.CommitAsync();
        }
    }
}
