using cod.application.Cafeteria.Commands;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.application.Register.Common;
using cod.domain.Features.Enums;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Queries.GetRegistersByCafeteria
{
    public class GetRegistersByCafeteriaQuery : BaseQuery, IGetRegistersByCafeteriaQuery
    {
        public GetRegistersByCafeteriaQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<RegisterModel>> Execute(int cafeteriaId, bool UseInInventory)
        {
            Expression<Func<domain.Data.Register, bool>> GetUseInInventoryFilter = GetUseInInventory(UseInInventory);

            return await _repository.QueryActive<domain.Data.Register>()
                .Where(f => f.CafeteriaId == cafeteriaId)
                .Where(GetUseInInventoryFilter)
                .Select(f => new RegisterModel
                {
                Id = f.Id,
                Name = f.Name,
                IsActive = f.IsActive,
            }).ToListAsync();
        }

        public static Expression<Func<domain.Data.Register, bool>> GetUseInInventory(bool UseInInventory)
        {
            if (UseInInventory)
                return detail => detail.UseInInventory == UseInInventory;
            return detail => true;
        }
    }
}
