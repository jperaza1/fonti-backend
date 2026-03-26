using cod.application.Common.Clases;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Expense.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace cod.application.Expense.Queries.GetExpenses
{
    public class GetExpensesQuery : BaseQuery, IGetExpensesQuery
    {
        private readonly AuthService _authService;

        public GetExpensesQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<ExpenseModel>> Execute(ExpenseFilterModel filterModel)
        {
            filterModel ??= new ExpenseFilterModel();
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.Expense, bool>> byDate = GetExpenseByDate(filterModel, timezone);
            Expression<Func<domain.Operation.Expense, bool>> byCategory = GetExpenseByCategory(filterModel);
            Expression<Func<domain.Operation.Expense, bool>> bySubCategory = GetExpenseBySubCategory(filterModel);
            Expression<Func<domain.Operation.Expense, bool>> byRoute = GetExpenseByRoute(filterModel);

            return await _repository.Query<domain.Operation.Expense>()
                .Where(byDate)
                .Where(byCategory)
                .Where(bySubCategory)
                .Select(f => new ExpenseModel
                {
                    Id = f.Id,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name,
                    SubCategoryId = f.SubCategoryId,
                    SubCategoryName = f.SubCateogy.Name,
                    VendorId = f.VendorId,
                    RouteId = f.RouteId,
                    Route = f.Routes.Name,
                    VendorName = f.VendorId.HasValue ? f.Vendors.Name : string.Empty,
                    Notes = f.Notes,
                    Date = f.Date,
                    IsInvoice = f.IsInvoice,
                    Total = f.Total,
                    IsActive = f.IsActive,
                }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.Expense, bool>> GetExpenseByCategory(ExpenseFilterModel filterModel)
        {
            if (filterModel.CategoryId > 0)
                return e => e.CategoryId == filterModel.CategoryId;
            return e => true;
        }

        public static Expression<Func<domain.Operation.Expense, bool>> GetExpenseByRoute(ExpenseFilterModel filterModel)
        {
            if (filterModel.RouteId > 0)
                return e => e.RouteId == filterModel.RouteId;
            return e => true;
        }

        public static Expression<Func<domain.Operation.Expense, bool>> GetExpenseBySubCategory(ExpenseFilterModel filterModel)
        {
            if (filterModel.SubCategoryId > 0)
                return e => e.SubCategoryId == filterModel.SubCategoryId;
            return e => true;
        }

        public static Expression<Func<domain.Operation.Expense, bool>> GetExpenseByDate(ExpenseFilterModel filterModel, string timeZone)
        {
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
            {
                filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
                filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
                return e => e.Date.Date >= filterModel.DateFrom.Value.Date && e.Date.Date <= filterModel.DateTo.Value.Date;
            }

            return e => true;
        }
    }
}
