using cod.application.Distributor.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Distributor.Queries.GetDistributor
{
    public class GetDistributorQuery : BaseQuery, IGetDistributorQuery
    {
        public GetDistributorQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<DistributorModel> Execute(int id)
        {
            var distributor = await _repository.Query<domain.Data.Distributor>()
                .Where(f => f.Id == id)
                .Select(f => new DistributorModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive,
                    ProductDistributorPrices = f.ProductDistributorPrices.Select(g => new ProductDistributorPricesModel
                    {
                        Id = g.Id,
                        DistributorId = g.DistributorId,
                        Price = g.Price,
                        ProductId = g.ProductId,
                        ProductName = g.Product.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (distributor == null)
                throw new ApplicationException("Distribuidor no existe", "DISTRIBUIDOR");

            return distributor;
        }
    }
}
