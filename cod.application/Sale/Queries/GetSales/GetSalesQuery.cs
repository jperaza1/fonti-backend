using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Queries.GetInvoices;
using cod.application.Main.Session;
using cod.application.Product.Common;
using cod.application.Sale.Common;
using cod.domain.Data;
using cod.domain.Features;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSales
{
    public class GetSalesQuery : BaseQuery, IGetSalesQuery
    {
        private readonly AuthService _authService;
        public GetSalesQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<SaleModel>> Execute(FilterSaleDto filterSale)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.Sale, bool>> GetSaleBySaleDateFilter = GetSaleBySaleDate(filterSale, timezone);
            Expression<Func<domain.Operation.Sale, bool>> GetSaleBySaleTypeFilter = GetSaleBySaleType(filterSale);
            Expression<Func<domain.Operation.Sale, bool>> GetProductTypeFilter = GetProductType(filterSale);
            Expression<Func<domain.Operation.Sale, bool>> GetCategoryFilter = GetCategory(filterSale);
            Expression<Func<domain.Operation.Sale, bool>> GetSaleByCafeteriaFilter = GetSaleByCafeteria(filterSale);

            var baseQuery = _repository.Query<domain.Operation.Sale>()
                .Where(GetSaleByCafeteriaFilter)
                .Where(GetSaleBySaleDateFilter)
                .Where(GetProductTypeFilter)
                .Where(GetCategoryFilter)
                .Where(GetSaleBySaleTypeFilter)
                .Select(f => new
            {
                f.Id,
                f.CategoryId,
                Cafeteria = f.Cafeteria.Name,
                f.SaleDate,
                f.SaleType,
                SaleTypeName = f.SaleType.GetEnumDescription(),
                f.CafeteriaId,
                Category = f.Category.Name,
                f.ProductTypeId,
                ProductType = f.ProductType.Name,
                f.Amount,
                f.RegisterId,
                Register = f.RegisterId.HasValue ? f.Register.Name : string.Empty,
                f.CustomerId,
                Customer = f.CustomerId.HasValue ? f.Customer.Name : string.Empty,
                ServicePeriod = f.ServicePeriodId.HasValue ? f.ServicePeriod.Name : string.Empty,
                f.ServicePeriodId,
                f.IsActive
            });

            return await baseQuery.Select(f => new SaleModel
            {
                Id = f.Id,
                Amount = f.Amount,
                CafeteriaId = f.CafeteriaId,
                Cafeteria = f.Cafeteria,
                RegisterId = f.RegisterId,
                Register = f.Register,
                Category = f.Category,
                CategoryId = f.CategoryId,
                ProductTypeId = f.ProductTypeId,
                ProductType = f.ProductType,
                SaleDate = f.SaleDate.Date,
                SaleType = f.SaleType,
                SaleTypeName = f.SaleTypeName,
                IsActive = f.IsActive,
                ServicePeriod = f.ServicePeriod,
                ServicePeriodId = f.ServicePeriodId,
                Customer = f.Customer,
                CustomerId = f.CustomerId
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.Sale, bool>> GetSaleByCafeteria(FilterSaleDto filterSale)
        {
            if (filterSale.Cafeterias.Any())
                return sale => filterSale.Cafeterias.Any(g => g == sale.CafeteriaId);
            return sale => true;
        }

        public static Expression<Func<domain.Operation.Sale, bool>> GetSaleBySaleDate(FilterSaleDto filterSale, string timeZone)
        {

            filterSale.DateFrom = DateService.GetCLientLocalTime(filterSale.DateFrom.Value, timeZone);
            filterSale.DateTo = DateService.GetCLientLocalTime(filterSale.DateTo.Value, timeZone);
            if (filterSale.DateFrom.HasValue && filterSale.DateTo.HasValue)
                return sale => sale.SaleDate.Date >= filterSale.DateFrom.Value.Date && sale.SaleDate.Date <= filterSale.DateTo.Value.Date;
            return sale => true;
        }

        public static Expression<Func<domain.Operation.Sale, bool>> GetSaleBySaleType(FilterSaleDto filterSale)
        {
            if (filterSale.SaleType.IsValid())
                return sale => sale.SaleType == filterSale.SaleType;
            return sale => true;
        }

        public static Expression<Func<domain.Operation.Sale, bool>> GetProductType(FilterSaleDto filterSale)
        {
            if (filterSale.ProductTypeId > 0)
                return sale => sale.ProductTypeId == filterSale.ProductTypeId;
            return sale => true;
        }

        public static Expression<Func<domain.Operation.Sale, bool>> GetCategory(FilterSaleDto filterSale)
        {
            if (filterSale.CategoryId > 0)
                return sale => sale.CategoryId == filterSale.CategoryId;
            return sale => true;
        }
    }
}
