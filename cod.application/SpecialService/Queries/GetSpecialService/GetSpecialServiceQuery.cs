using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.application.SpecialService.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SpecialService.Queries.GetSpecialService
{
    public class GetSpecialServiceQuery : BaseQuery, IGetSpecialServiceQuery
    {
        public GetSpecialServiceQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<SpecialServiceModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.SpecialService>()
            .Where(f => f.Id == id)
            .Select(f => new
            {
                f.Id,
                f.CafeteriaId,
                Cafeteria = f.Cafeteria.Name,
                f.Status,
                f.CreatorUser,
                f.ApprovedAt,
                f.ApprovedUser,
                f.RejectedAt,
                f.RejectedUser,
                f.ReasonRejection,
                f.Date,
                f.Reference,
                SpecialServiceDetails = f.SpecialServiceDetails.Select(x => new
                {
                    x.Id,
                    x.SpecialServiceId,
                    x.ProductId,
                    Product = $"{x.Product.Name}",
                    x.Quantity,
                }).ToList()
            });

            var purchaseOrder = await baseQuery.FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Orden no existe", "ORDEN COMPRA");

            return new SpecialServiceModel
            {
                Id = purchaseOrder.Id,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
                Date = purchaseOrder.Date,
                Reference = purchaseOrder.Reference,
                Status = purchaseOrder.Status,
                CreatorUser = purchaseOrder.CreatorUser,
                ApprovedUser = purchaseOrder.ApprovedUser,
                StatusName = purchaseOrder.Status.GetEnumDescription(),
                ReasonRejection = purchaseOrder.ReasonRejection,
                Details = purchaseOrder.SpecialServiceDetails.Select(f => new SpecialServiceDetailModel
                {
                    Id = f.Id,
                    SpecialServiceId = f.SpecialServiceId,
                    Quantity = f.Quantity,
                    ProductName = f.Product,
                    ProductId = f.ProductId,
                }).ToList()
            };
        }
    }
}
