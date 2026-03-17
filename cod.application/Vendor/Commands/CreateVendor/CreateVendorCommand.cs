using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Main;
using cod.application.Vendor.Common;
using cod.domain.Data;
using cod.domain.Features;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X500;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Vendor.Commands.CreateVendor
{
    public class CreateVendorCommand : BaseCommand, ICreateVendorCommand
    {
        
        public CreateVendorCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
            
        }

        public async Task Execute(VendorModel model)
        {
            await ValidateModel(model);
            var newVendor = new domain.Data.Vendor
            {
                Name = model.Name,
                Phone = model.Phone,
                Seller = model.Seller,
                Address = model.Address,
                Rtn = model.Rtn,
                HasCredit = model.HasCredit,
                CreditDays = model.CreditDays,
                HasInvoice = model.HasInvoice,
                RequiresAdminApproval = model.RequiresAdminApproval,
            };

            _repository.InsertAsync(newVendor);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(VendorModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("proveedor", ExceptionEnums.ExceptionCode.Required));

            if (model.Rtn.NullOrEmpty())
                this._exceptionFactory.Add("rtn", this._exceptionFactory.GetMessage("rtn", ExceptionEnums.ExceptionCode.Required));

            if (model.Address.NullOrEmpty())
                this._exceptionFactory.Add("address", this._exceptionFactory.GetMessage("Direccion", ExceptionEnums.ExceptionCode.Required));

            if (model.Phone.NullOrEmpty())
                this._exceptionFactory.Add("phone", this._exceptionFactory.GetMessage("Telefono", ExceptionEnums.ExceptionCode.Required));

            if (model.Seller.NullOrEmpty())
                this._exceptionFactory.Add("seller", this._exceptionFactory.GetMessage("Vendedor", ExceptionEnums.ExceptionCode.Required));

            if(model.HasCredit && model.CreditDays <= 0)
                this._exceptionFactory.Add("creditDays", this._exceptionFactory.GetMessage("Días de crédito", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PROVEEDORES", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Vendor>(f => f.Name.ToUpper() == model.Name.ToUpper());
            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("proveedor", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PROVEEDORES", this._exceptionFactory.Exceptions);
        }
    }
}
