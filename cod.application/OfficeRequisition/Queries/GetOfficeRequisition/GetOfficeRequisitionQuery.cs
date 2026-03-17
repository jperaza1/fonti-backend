using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.OfficeRequisition.Common;
using cod.application.SpecialService.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.OfficeRequisition.Queries.GetOfficeRequisition
{
    public class GetOfficeRequisitionQuery : BaseQuery, IGetOfficeRequisitionQuery
    {
        public GetOfficeRequisitionQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<OfficeRequisitionModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.OfficeRequisition>()
            .Where(f => f.Id == id)
            .Select(f => new
            {
                f.Id,
                f.CafeteriaId,
                Cafeteria = f.Cafeteria.Name,
                f.Description,
                f.Status,
                f.CreatorUser,
                f.ApprovedAt,
                f.ApprovedUser,
                f.RejectedAt,
                f.RejectedUser,
                f.ReasonRejection,
                f.Date,
                f.Reference,
                Details = f.OfficeRequisitionDetails.Select(x => new
                {
                    x.Id,
                    x.OfficeRequisitionId,
                    x.ProductId,
                    Product = $"{x.Product.Name}",
                    x.Quantity,
                }).ToList()
            });

            var purchaseOrder = await baseQuery.FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Orden no existe", "ORDEN COMPRA");

            return new OfficeRequisitionModel
            {
                Id = purchaseOrder.Id,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
                Date = purchaseOrder.Date,
                Description = purchaseOrder.Description,
                Reference = purchaseOrder.Reference,
                Status = purchaseOrder.Status,
                CreatorUser = purchaseOrder.CreatorUser,
                ApprovedUser = purchaseOrder.ApprovedUser,
                StatusName = purchaseOrder.Status.GetEnumDescription(),
                ReasonRejection = purchaseOrder.ReasonRejection,
                Details = purchaseOrder.Details.Select(f => new OfficeRequisitionDetailModel
                {
                    Id = f.Id,
                    OfficeRequisitionId = f.OfficeRequisitionId,
                    Quantity = f.Quantity,
                    ProductName = f.Product,
                    ProductId = f.ProductId,
                }).ToList()
            };
        }
    }
}
