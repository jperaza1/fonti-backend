using cod.application.Distributor.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Distributor.Queries.GetProductDistributorPrices
{
    public class GetProductDistributorPricesQuery : BaseQuery, IGetProductDistributorPricesQuery
    {
        public GetProductDistributorPricesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductDistributorPricesModel>> Execute(int distributorId)
        {
            var baseQuery = _repository.QueryActive<domain.Data.ProductDistributorPrice>()
                .Where(f => f.DistributorId == distributorId);

            return await baseQuery.Select(g => new ProductDistributorPricesModel
            {
                Id = g.Id,
                DistributorId = g.DistributorId,
                ProductId = g.ProductId,
                ProductName = g.Product.Name,
                Price = g.Price,
            })
                .OrderBy(f => f.ProductName)
                .ToListAsync();
        }
    }
}
