using cod.application.Features.Models;
using cod.application.Production.Commands.CancelProduction;
using cod.application.Production.Commands.CancelProductionShift;
using cod.application.Production.Commands.CloseProductionShift;
using cod.application.Production.Commands.CreateProduction;
using cod.application.Production.Common;
using cod.application.Production.Queries.GetOpenProduction;
using cod.application.Production.Queries.GetProductionConsumption;
using cod.application.Production.Queries.GetProductionProducts;
using cod.application.Production.Queries.GetProductionShift;
using cod.application.Production.Queries.GetProductions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class ProductionController : BaseController
    {
        private readonly IGetProductionsQuery _getProductionsQuery;
        private readonly IGetProductionShiftQuery _getProductionShiftQuery;
        private readonly IGetProductionProductsQuery _getProductionProductsQuery;
        private readonly IGetProductionConsumptionQuery _getProductionConsumptionQuery;
        private readonly ICreateProductionCommand _createProductionCommand;
        private readonly ICancelProductionCommand _cancelProductionCommand;
        private readonly ICancelProductionShiftCommand _cancelProductionShiftCommand;
        private readonly ICloseProductionShiftCommand _closeProductionShiftCommand;
        private readonly IGetOpenProductionQuery _getOpenProductionQuery;

        public ProductionController(
            IGetProductionsQuery getProductionsQuery,
            IGetProductionShiftQuery getProductionShiftQuery,
            IGetProductionProductsQuery getProductionProductsQuery,
            IGetProductionConsumptionQuery getProductionConsumptionQuery,
            ICreateProductionCommand createProductionCommand,
            ICancelProductionCommand cancelProductionCommand,
            ICancelProductionShiftCommand cancelProductionShiftCommand,
            ICloseProductionShiftCommand closeProductionShiftCommand,
            IGetOpenProductionQuery getOpenProductionQuery)
        {
            ControllerName = "Producción";
            _getProductionsQuery = getProductionsQuery;
            _getProductionShiftQuery = getProductionShiftQuery;
            _getProductionProductsQuery = getProductionProductsQuery;
            _getProductionConsumptionQuery = getProductionConsumptionQuery;
            _createProductionCommand = createProductionCommand;
            _cancelProductionCommand = cancelProductionCommand;
            _cancelProductionShiftCommand = cancelProductionShiftCommand;
            _closeProductionShiftCommand = closeProductionShiftCommand;
            _getOpenProductionQuery = getOpenProductionQuery;
        }

        [HttpPost]
        public async Task<List<ProductionShiftModel>> GetAll([FromBody] ProductionFilterModel filter)
        {
            return await _getProductionsQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<ProductionShiftModel> GetByDay(DateTime date, int shiftTypeId)
        {
            return await _getProductionShiftQuery.Execute(date, shiftTypeId);
        }

        [HttpGet]
        public async Task<ProductionShiftModel> GetOpen()
        {
            return await _getOpenProductionQuery.Execute();
        }

        [HttpGet]
        public async Task<List<ProductionProductModel>> GetProducts()
        {
            return await _getProductionProductsQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> GetConsumption([FromBody] ProductionConsumptionRequest request)
        {
            try
            {
                var lines = await _getProductionConsumptionQuery.Execute(request);
                return Ok(lines);
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductionModel model)
        {
            try
            {
                await _createProductionCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _cancelProductionCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CloseShift(int id)
        {
            try
            {
                await _closeProductionShiftCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelShift(int id)
        {
            try
            {
                await _cancelProductionShiftCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
