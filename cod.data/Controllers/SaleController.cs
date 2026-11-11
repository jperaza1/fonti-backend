using cod.application.Features.Models;
using cod.application.Sale.Commands.ConsolidateSale;
using cod.application.Sale.Commands.CreateSale;
using cod.application.Sale.Commands.DisableSale;
using cod.application.Sale.Commands.EnableSale;
using cod.application.Sale.Commands.UpdateSale;
using cod.application.Sale.Common;
using cod.application.Sale.Queries.GetConsolidateAvailability;
using cod.application.Sale.Queries.GetDailySalesByCafeteria;
using cod.application.Sale.Queries.GetPendingBillingReport;
using cod.application.Sale.Queries.GetSale;
using cod.application.Sale.Queries.GetSales;
using cod.application.Sale.Queries.GetSaleChartReport;
using cod.application.Sale.Queries.GetSaleRouteQuantityReport;
using cod.application.Sale.Queries.GetSalesByMonth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class SaleController : BaseController
    {
        private readonly IGetSaleQuery _getSaleQuery;
        private readonly IGetSalesQuery _getSalesQuery;
        private readonly IGetSaleByMonthQuery _getSaleByMonthQuery;
        private readonly IGetPendingBillingReportQuery _getPendingBillingReportQuery;
        private readonly IGetSaleChartReportQuery _getSaleChartReportQuery;
        private readonly IGetSaleRouteQuantityReportQuery _getSaleRouteQuantityReportQuery;
        private readonly IGetDailySalesByCafeteriaQuery _getDailySalesByCafeteriaQuery;
        private readonly ICreateSaleCommand _createSaleCommand;
        private readonly IUpdateSaleCommand _updateSaleCommand;
        private readonly IEnableSaleCommand _enableSaleCommand;
        private readonly IDisableSaleCommand _disableSaleCommand;
        private readonly IConsolidateSaleCommand _consolidateSaleCommand;
        private readonly IGetConsolidateAvailabilityQuery _getConsolidateAvailabilityQuery;

        public SaleController(
            IGetSaleQuery getSaleQuery, 
            IGetSalesQuery getSalesQuery,
            IGetPendingBillingReportQuery getPendingBillingReportQuery,
            IGetSaleByMonthQuery getSaleByMonthQuery,
            ICreateSaleCommand createSaleCommand,
            IUpdateSaleCommand updateSaleCommand,
            IEnableSaleCommand enableSaleCommand,
            IDisableSaleCommand disableSaleCommand,
            IConsolidateSaleCommand consolidateSaleCommand,
            IGetConsolidateAvailabilityQuery getConsolidateAvailabilityQuery,
            IGetDailySalesByCafeteriaQuery getDailySalesByCafeteriaQuery,
            IGetSaleChartReportQuery getSaleChartReportQuery,
            IGetSaleRouteQuantityReportQuery getSaleRouteQuantityReportQuery)
        {
            _getSaleQuery = getSaleQuery;
            _getSalesQuery = getSalesQuery;
            _getSaleByMonthQuery = getSaleByMonthQuery;
            _createSaleCommand = createSaleCommand;
            _updateSaleCommand = updateSaleCommand;
            _enableSaleCommand = enableSaleCommand;
            _disableSaleCommand = disableSaleCommand;
            _consolidateSaleCommand = consolidateSaleCommand;
            _getConsolidateAvailabilityQuery = getConsolidateAvailabilityQuery;
            _getDailySalesByCafeteriaQuery = getDailySalesByCafeteriaQuery;
            _getPendingBillingReportQuery = getPendingBillingReportQuery;
            _getSaleChartReportQuery = getSaleChartReportQuery;
            _getSaleRouteQuantityReportQuery = getSaleRouteQuantityReportQuery;
        }

        [HttpPost]
        public async Task<List<SaleChartPointModel>> GetChartReport([FromBody] SaleChartFilterModel filter)
        {
            return await _getSaleChartReportQuery.Execute(filter);
        }

        [HttpPost]
        public async Task<SaleRouteQuantityReportModel> GetRouteQuantityReport([FromBody] SaleRouteQuantityFilterModel filter)
        {
            return await _getSaleRouteQuantityReportQuery.Execute(filter);
        }

        [HttpPost]
        public async Task<List<SaleModel>> GetPendingBillingReport([FromBody] FilterSaleDto filterSale)
        {
            return await _getPendingBillingReportQuery.Execute(filterSale);
        }

        [HttpPost]
        public async Task<List<SaleModel>> GetAll([FromBody] FilterSaleDto filterSale)
        {
            return await _getSalesQuery.Execute(filterSale);
        }

        [HttpGet]
        public async Task<SaleModel> GetById(int id)
        {
            return await _getSaleQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<GetSalesByMonthModel>> GetSalesByMonth()
        {
            return await _getSaleByMonthQuery.Execute();
        }

        [HttpGet]
        public async Task<List<GetSalesByMonthModel>> GetSalesByWeek()
        {
            return await _getDailySalesByCafeteriaQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleModel model)
        {
            try
            {
                await _createSaleCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] SaleModel model)
        {
            try
            {
                await _updateSaleCommand.Execute(model, id);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<List<ConsolidateAvailabilityLine>> GetConsolidateAvailability([FromBody] ConsolidateSaleModel model)
        {
            return await _getConsolidateAvailabilityQuery.Execute(model);
        }

        [HttpPost]
        public async Task<IActionResult> Consolidate([FromBody] ConsolidateSaleModel model)
        {
            try
            {
                await _consolidateSaleCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                await _disableSaleCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Enable(int id)
        {
            try
            {
                await _enableSaleCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
