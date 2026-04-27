using cod.application.Features.Models;
using cod.application.Route.Commands.CreateRoute;
using cod.application.Route.Commands.DisableRoute;
using cod.application.Route.Commands.EnableRoute;
using cod.application.Route.Commands.UpdateRoute;
using cod.application.Route.Common;
using cod.application.Route.Queries.GetProductRoutePrices;
using cod.application.Route.Queries.GetRoute;
using cod.application.Route.Queries.GetRoutes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class RouteController : BaseController
    {
        private readonly IGetRoutesQuery _getRoutesQuery;
        private readonly IGetRouteQuery _getRouteQuery;
        private readonly IGetProductRoutePricesQuery _getProductRoutePricesQuery;
        private readonly ICreateRouteCommand _createRouteCommand;
        private readonly IUpdateRouteCommand _updateRouteCommand;
        private readonly IEnableRouteCommand _enableRouteCommand;
        private readonly IDisableRouteCommand _disableRouteCommand;

        public RouteController(
            IGetRoutesQuery getRoutesQuery,
            IGetRouteQuery getRouteQuery,
            IGetProductRoutePricesQuery getProductRoutePricesQuery,
            ICreateRouteCommand createRouteCommand,
            IUpdateRouteCommand updateRouteCommand,
            IEnableRouteCommand enableRouteCommand,
            IDisableRouteCommand disableRouteCommand)
        {
            _getRoutesQuery = getRoutesQuery;
            _getRouteQuery = getRouteQuery;
            _getProductRoutePricesQuery = getProductRoutePricesQuery;
            _createRouteCommand = createRouteCommand;
            _updateRouteCommand = updateRouteCommand;
            _enableRouteCommand = enableRouteCommand;
            _disableRouteCommand = disableRouteCommand;
        }

        [HttpGet]
        public async Task<List<RouteModel>> GetAll()
        {
            return await _getRoutesQuery.Execute();
        }

        [HttpGet]
        public async Task<List<RouteModel>> GetAllActive()
        {
            var response = await _getRoutesQuery.Execute();
            return response.Where(f => f.IsActive).ToList();
        }

        [HttpGet]
        public async Task<RouteModel> GetById(int id)
        {
            return await _getRouteQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<ProductRoutePricesModel>> GetProductRoutePrices(int routeId)
        {
            return await _getProductRoutePricesQuery.Execute(routeId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RouteModel model)
        {
            try
            {
                await _createRouteCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RouteModel updateModel, int id)
        {
            try
            {
                await _updateRouteCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
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
                await _disableRouteCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Enable(int id)
        {
            try
            {
                await _enableRouteCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
