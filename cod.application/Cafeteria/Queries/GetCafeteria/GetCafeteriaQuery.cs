using cod.application.Cafeteria.Commands;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Cafeteria.Queries.GetCafeteria
{
    public class GetCafeteriaQuery : BaseQuery, IGetCafeteriaQuery
    {
        public GetCafeteriaQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<CafeteriaModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Data.Cafeteria>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Code,
                    f.Name,
                    f.IsActive,
                    f.PurchaseOrderCorelative,
                    f.SpecialServicesCorelative,
                    f.OfficeRequisitionCorelative,
                    f.InventoryCorelative,
                    Customers = f.CustomerCafeterias.Select(x => new
                    {
                        x.Id,
                        x.CafeteriaId,
                        x.CustomerId,
                        Customer = x.Customer.Name
                    }).ToList()
                });

            var cafeteria = await baseQuery.FirstOrDefaultAsync();

            if(cafeteria == null)
                throw new ApplicationException("Cafeteria no existe", "CAFETERIA");

            return new CafeteriaModel
            {
                Id = id,
                Code = cafeteria.Code,
                Name = cafeteria.Name,
                IsActive = cafeteria.IsActive,
                PurchaseOrderCorelative = cafeteria.PurchaseOrderCorelative,
                SpecialServicesCorelative = cafeteria.SpecialServicesCorelative,
                OfficeRequisitionCorelative = cafeteria.OfficeRequisitionCorelative,
                InventoryCorelative = cafeteria.InventoryCorelative,
                Customers = cafeteria.Customers.Select(f => new CustomerCafeteriaModel
                {
                    Id = f.Id,
                    CafeteriaId = f.CafeteriaId,
                    Customer = f.Customer,
                    CustomerId = f.CustomerId
                }).ToList()
            };

        }
    }
}
