using cod.application.Common.Extensions;
using cod.application.DispatchControl.Commom;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Data;
using cod.domain.Features.Enums;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;


namespace cod.application.DispatchControl.Queries.GetDispatchControl
{
    public class GetDispatchControlQuery : BaseQuery, IGetDispatchControlQuery
    {
        public GetDispatchControlQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<DispatchControlModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.DispatchControl>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.RouteId,
                    RouteName = f.Route.Name,
                    f.Vendor,
                    f.DispatchControlDate,
                    f.CreationTime,
                    f.CreatorUser,
                    f.Status,
                    f.TruckId,
                    Truck = f.TruckId.HasValue ? f.Truck.Plate : string.Empty,
                    DispatchControlDetails = f.DispatchControlDetails.Select(g => new
                    {
                        g.Id,
                        g.DispatchControlId,
                        g.BottleQuantity,
                        g.CrateQuantity,
                        g.DispatchControlType,
                        DispatchControlTypeName = g.DispatchControlType.GetEnumDescription(),
                        g.MissingBottleQuantity,
                        ProductMovements = g.ProductMovements
                        .Where(x => x.Product.ProductType == ProductType.Sale)
                        .Select(x => new
                        {
                            x.Id,
                            x.ProductId,
                            x.Quantity,
                            x.DispatchControlDetailId,
                            ProductName = x.Product.Name,

                        }).ToList()
                    }).ToList()
                });

            var item = await baseQuery.FirstOrDefaultAsync();

            if (item == null)
                throw new ApplicationException("Registro no existe", "CONTROL DE SALIDA");

            return new DispatchControlModel
            {
                Id = item.Id,
                RouteId = item.RouteId,
                Route = item.RouteName,
                Vendor = item.Vendor,
                DispatchControlDate = item.DispatchControlDate,
                CreationTime = item.CreationTime,
                CreatorUser = item.CreatorUser,
                Status = item.Status,
                StatusName = item.Status.GetEnumDescription(),
                Truck = item.Truck,
                TruckId = item.TruckId,
                DispatchControlDetails = item.DispatchControlDetails.Select(g => new DispatchControlDetailModel
                {
                    Id = g.Id,
                    DispatchControlId = g.DispatchControlId,
                    BottleQuantity = g.BottleQuantity,
                    CrateQuantity = g.CrateQuantity,
                    DispatchControlType = g.DispatchControlType,
                    DispatchControlTypeName = g.DispatchControlType.GetEnumDescription(),
                    MissingBottleQuantity = g.MissingBottleQuantity,
                    ProductMovements = g.ProductMovements.Select(x => new ProductMovementModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        DispatchControlDetailId = x.DispatchControlDetailId,
                        Product = x.ProductName,
                    }).ToList()
                }).ToList()
            };
        }
    }
}
