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
            Expression<Func<domain.Operation.Sale, bool>> GetSaleByRouteFilter = GetSaleByRoute(filterSale);

            var baseQuery = _repository.Query<domain.Operation.Sale>()
                .Where(GetSaleByRouteFilter)
                .Where(GetSaleBySaleDateFilter)
                .Where(GetSaleBySaleTypeFilter)
                .Select(f => new
                {
                    f.Id,
                    f.RouteId,
                    Route = f.Route.Name,
                    f.Vendor,
                    f.SaleDate,
                    f.SaleType,
                    SaleTypeName = f.SaleType.GetEnumDescription(),
                    f.CustomerId,
                    CustomerName = f.CustomerId.HasValue ? f.Customer.Name : string.Empty,
                    f.Amount,
                    f.IsActive,
                    InvoiceNumber = f.SaleInvoiceId.HasValue ? f.SaleInvoice.InvoiceNumber : string.Empty,
                    f.SaleInvoiceId
                });

            return await baseQuery.Select(f => new SaleModel
            {
                Id = f.Id,
                Amount = f.Amount,
                Route = f.Route,
                RouteId = f.RouteId,
                Vendor = f.Vendor,
                SaleDate = f.SaleDate.Date,
                SaleType = f.SaleType,
                SaleTypeName = f.SaleTypeName,
                CustomerName = f.CustomerName,
                CustomerId = f.CustomerId,
                IsActive = f.IsActive,
                SaleInvoiceId = f.SaleInvoiceId,
                InvoiceNumber = f.InvoiceNumber
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.Sale, bool>> GetSaleByRoute(FilterSaleDto filterSale)
        {
            if (filterSale.RouteId > 0)
                return sale => sale.RouteId == filterSale.RouteId;
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
    }
}
