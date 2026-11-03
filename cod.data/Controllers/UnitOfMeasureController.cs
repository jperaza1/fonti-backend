using cod.application.Features.Models;
using cod.application.UnitOfMeasure.Commands.CreateUnitOfMeasure;
using cod.application.UnitOfMeasure.Commands.DisableUnitOfMeasure;
using cod.application.UnitOfMeasure.Commands.EnableUnitOfMeasure;
using cod.application.UnitOfMeasure.Commands.UpdateUnitOfMeasure;
using cod.application.UnitOfMeasure.Common;
using cod.application.UnitOfMeasure.Queries.GetActiveUnitOfMeasures;
using cod.application.UnitOfMeasure.Queries.GetUnitOfMeasure;
using cod.application.UnitOfMeasure.Queries.GetUnitOfMeasures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class UnitOfMeasureController : BaseController
    {
        private readonly IGetUnitOfMeasuresQuery _getUnitOfMeasuresQuery;
        private readonly IGetActiveUnitOfMeasuresQuery _getActiveUnitOfMeasuresQuery;
        private readonly IGetUnitOfMeasureQuery _getUnitOfMeasureQuery;
        private readonly ICreateUnitOfMeasureCommand _createUnitOfMeasureCommand;
        private readonly IUpdateUnitOfMeasureCommand _updateUnitOfMeasureCommand;
        private readonly IEnableUnitOfMeasureCommand _enableUnitOfMeasureCommand;
        private readonly IDisableUnitOfMeasureCommand _disableUnitOfMeasureCommand;

        public UnitOfMeasureController(
            IGetUnitOfMeasuresQuery getUnitOfMeasuresQuery,
            IGetActiveUnitOfMeasuresQuery getActiveUnitOfMeasuresQuery,
            IGetUnitOfMeasureQuery getUnitOfMeasureQuery,
            ICreateUnitOfMeasureCommand createUnitOfMeasureCommand,
            IUpdateUnitOfMeasureCommand updateUnitOfMeasureCommand,
            IEnableUnitOfMeasureCommand enableUnitOfMeasureCommand,
            IDisableUnitOfMeasureCommand disableUnitOfMeasureCommand)
        {
            _getUnitOfMeasuresQuery = getUnitOfMeasuresQuery;
            _getActiveUnitOfMeasuresQuery = getActiveUnitOfMeasuresQuery;
            _getUnitOfMeasureQuery = getUnitOfMeasureQuery;
            _createUnitOfMeasureCommand = createUnitOfMeasureCommand;
            _updateUnitOfMeasureCommand = updateUnitOfMeasureCommand;
            _enableUnitOfMeasureCommand = enableUnitOfMeasureCommand;
            _disableUnitOfMeasureCommand = disableUnitOfMeasureCommand;
        }

        [HttpGet]
        public async Task<List<UnitOfMeasureModel>> GetAll()
        {
            return await _getUnitOfMeasuresQuery.Execute();
        }

        [HttpGet]
        public async Task<List<UnitOfMeasureModel>> GetAllActive()
        {
            return await _getActiveUnitOfMeasuresQuery.Execute();
        }

        [HttpGet]
        public async Task<UnitOfMeasureModel> GetById(int id)
        {
            return await _getUnitOfMeasureQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UnitOfMeasureModel model)
        {
            try
            {
                await _createUnitOfMeasureCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UnitOfMeasureModel updateModel, int id)
        {
            try
            {
                await _updateUnitOfMeasureCommand.Execute(updateModel, id);
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
                await _disableUnitOfMeasureCommand.Execute(id);
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
                await _enableUnitOfMeasureCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
