using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Vendor.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Queries.GetVendors
{
    public class GetVendorsQuery : BaseQuery, IGetVendorsQuery
    {
        public GetVendorsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<VendorModel>> Execute()
        {
            var vendors = await _repository.Query<domain.Data.Vendor>().Select(f => new VendorModel
            {
                Id = f.Id,
                Name = f.Name,
                IsActive = f.IsActive
            }).OrderBy(f => f.Name).ToListAsync();

            return vendors;
        }
    }
}
