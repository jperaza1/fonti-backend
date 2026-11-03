using cod.application.Features.Models;
using cod.application.Truck.Commands.CreateTruck;
using cod.application.Truck.Commands.DisableTruck;
using cod.application.Truck.Commands.EnableTruck;
using cod.application.Truck.Commands.UpdateTruck;
using cod.application.Truck.Common;
using cod.application.Truck.Queries.GetActiveTrucks;
using cod.application.Truck.Queries.GetTruck;
using cod.application.Truck.Queries.GetTrucks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class TruckController : BaseController
    {
        private readonly IGetTrucksQuery _getTrucksQuery;
        private readonly IGetTruckQuery _getTruckQuery;
        private readonly IGetActiveTrucksQuery _getActiveTrucksQuery;
        private readonly ICreateTruckCommand _createTruckCommand;
        private readonly IUpdateTruckCommand _updateTruckCommand;
        private readonly IEnableTruckCommand _enableTruckCommand;
        private readonly IDisableTruckCommand _disableTruckCommand;

        public TruckController(
            IGetTrucksQuery getTrucksQuery,
            IGetTruckQuery getTruckQuery,
            IGetActiveTrucksQuery getActiveTrucksQuery,
            ICreateTruckCommand createTruckCommand,
            IUpdateTruckCommand updateTruckCommand,
            IEnableTruckCommand enableTruckCommand,
            IDisableTruckCommand disableTruckCommand)
        {
            _getTrucksQuery = getTrucksQuery;
            _getTruckQuery = getTruckQuery;
            _getActiveTrucksQuery = getActiveTrucksQuery;
            _createTruckCommand = createTruckCommand;
            _updateTruckCommand = updateTruckCommand;
            _enableTruckCommand = enableTruckCommand;
            _disableTruckCommand = disableTruckCommand;
            this.ControllerName = "Camiones";
        }

        [HttpGet]
        public async Task<List<TruckModel>> GetAll()
        {
            return await _getTrucksQuery.Execute();
        }

        [HttpGet]
        public async Task<TruckModel> GetById(int id)
        {
            return await _getTruckQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<TruckModel>> GetActive()
        {
            return await _getActiveTrucksQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TruckModel model)
        {
            try
            {
                await _createTruckCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TruckModel updateModel, int id)
        {
            try
            {
                await _updateTruckCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Inactive(int id)
        {
            try
            {
                await _disableTruckCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Active(int id)
        {
            try
            {
                await _enableTruckCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
