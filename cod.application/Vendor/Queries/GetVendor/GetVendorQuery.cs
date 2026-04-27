using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Vendor.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Vendor.Queries.GetVendor
{
    public class GetVendorQuery : BaseQuery, IGetVendorQuery
    {
        public GetVendorQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<VendorModel> Execute(int id)
        {
            var vendor = await _repository.QueryActive<domain.Data.Vendor>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (vendor == null)
                throw new ApplicationException("Proveedor no existe", "PROVEEDOR");

            return new VendorModel
            {
                Id = id,
                Name = vendor.Name,
                IsActive = vendor.IsActive,
            };
        }
    }
}
