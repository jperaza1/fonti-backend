using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Vendor.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Queries.GetActiveVendors
{
    public class GetActiveVendorsQuery : BaseQuery, IGetActiveVendorsQuery
    {
        public GetActiveVendorsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<VendorModel>> Execute()
        {
            var vendors = await _repository.QueryActive<domain.Data.Vendor>().Select(f => new VendorModel
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Address,
                Phone = f.Phone,
                Rtn = f.Rtn,
                Seller = f.Seller,
                IsActive = f.IsActive,
                HasInvoice = f.HasInvoice,
            }).OrderBy(f => f.Name).ToListAsync();

            return vendors;
        }
    }
}
