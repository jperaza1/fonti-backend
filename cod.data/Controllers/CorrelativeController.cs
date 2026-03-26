using cod.application.Correlative.Commands.CreateCorrelative;
using cod.application.Correlative.Commands.DisableCorrelative;
using cod.application.Correlative.Commands.EnableCorrelative;
using cod.application.Correlative.Commands.UpdateCorrelative;
using cod.application.Correlative.Common;
using cod.application.Correlative.Queries.GetCorrelative;
using cod.application.Correlative.Queries.GetCorrelatives;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class CorrelativeController : BaseController
    {
        private readonly IGetCorrelativesQuery _getCorrelativesQuery;
        private readonly IGetCorrelativeQuery _getCorrelativeQuery;
        private readonly ICreateCorrelativeCommand _createCorrelativeCommand;
        private readonly IUpdateCorrelativeCommand _updateCorrelativeCommand;
        private readonly IEnableCorrelativeCommand _enableCorrelativeCommand;
        private readonly IDisableCorrelativeCommand _disableCorrelativeCommand;

        public CorrelativeController(
            IGetCorrelativesQuery getCorrelativesQuery,
            IGetCorrelativeQuery getCorrelativeQuery,
            ICreateCorrelativeCommand createCorrelativeCommand,
            IUpdateCorrelativeCommand updateCorrelativeCommand,
            IEnableCorrelativeCommand enableCorrelativeCommand,
            IDisableCorrelativeCommand disableCorrelativeCommand)
        {
            _getCorrelativesQuery = getCorrelativesQuery;
            _getCorrelativeQuery = getCorrelativeQuery;
            _createCorrelativeCommand = createCorrelativeCommand;
            _updateCorrelativeCommand = updateCorrelativeCommand;
            _enableCorrelativeCommand = enableCorrelativeCommand;
            _disableCorrelativeCommand = disableCorrelativeCommand;
        }

        [HttpGet]
        public async Task<List<CorrelativeModel>> GetAll()
        {
            return await _getCorrelativesQuery.Execute();
        }

        [HttpGet]
        public async Task<CorrelativeModel> GetById(int id)
        {
            return await _getCorrelativeQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CorrelativeModel model)
        {
            try
            {
                await _createCorrelativeCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CorrelativeModel updateModel, int id)
        {
            try
            {
                await _updateCorrelativeCommand.Execute(updateModel, id);
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
                await _disableCorrelativeCommand.Execute(id);
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
                await _enableCorrelativeCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
