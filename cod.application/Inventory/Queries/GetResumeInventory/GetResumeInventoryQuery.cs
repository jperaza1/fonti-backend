using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetResumeInventory
{
    public class GetResumeInventoryQuery : BaseQuery, IGetResumeInventoryQuery
    {
        public GetResumeInventoryQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ResumeInventoryModel>> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.InventoryDetail>()
                .Where(x => x.InventoryId == id)
                .Select(x => new
                {
                    x.Id,
                    x.InventoryId,
                    x.ProductId,
                    Product = $"{x.Product.Name} ({x.Product.Measure.Name}) ({x.Product.Vendor.Name})",
                    x.Total,
                    x.Product.VendorId,
                    VendorName = x.Product.Vendor.Name,
                });

            var inventories = await baseQuery.ToListAsync();

            var grouped = inventories.GroupBy(x => x.VendorName).ToList();

            var lst = new List<ResumeInventoryModel>();

            foreach (var item in grouped) 
            {
                var inventory = new ResumeInventoryModel
                {
                    Vendor = item.Key,
                    Total = item.Sum(x => x.Total),
                    InventoryId = item.First().InventoryId,
                };

                lst.Add(inventory);
            }

            return lst;
        }
    }
}
