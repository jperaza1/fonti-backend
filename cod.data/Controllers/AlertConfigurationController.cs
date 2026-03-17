using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cod.application.Alerts.Commands.CreateAlertConfiguration;
using cod.application.Alerts.Commands.DisableAlertConfiguration;
using cod.application.Alerts.Commands.EnableAlertConfiguration;
using cod.application.Alerts.Commands.UpdateAlertConfiguration;
using cod.application.Alerts.Queries.GetAlertConfigurationById;
using cod.application.Alerts.Queries.GetAlertConfigurations;
using cod.application.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class AlertConfigurationController : BaseController
    {
        private readonly IGetAlertConfigurationsQuery _getAlertConfigurationsQuery;
        private readonly IGetAlertConfigurationByIdQuery _getAlertConfigurationByIdQuery;
        private readonly ICreateAlertConfigurationCommand _createAlertConfigurationCommand;
        private readonly IUpdateAlertConfigurationCommand _updateAlertConfigurationCommand;
        private readonly IDisableAlertConfigurationCommand _disableAlertConfigurationCommand;
        private readonly IEnableAlertConfigurationCommand _enableAlertConfigurationCommand;
        public AlertConfigurationController(
            IGetAlertConfigurationsQuery getAlertConfigurationsQuery,
            IGetAlertConfigurationByIdQuery getAlertConfigurationByIdQuery,
            ICreateAlertConfigurationCommand createAlertConfigurationCommand,
            IUpdateAlertConfigurationCommand updateAlertConfigurationCommand,
            IDisableAlertConfigurationCommand disableAlertConfigurationCommand,
            IEnableAlertConfigurationCommand enableAlertConfigurationCommand
            )
        {

            this._getAlertConfigurationsQuery = getAlertConfigurationsQuery;
            this._getAlertConfigurationByIdQuery = getAlertConfigurationByIdQuery;
            this._createAlertConfigurationCommand = createAlertConfigurationCommand;
            this._updateAlertConfigurationCommand = updateAlertConfigurationCommand;
            this._disableAlertConfigurationCommand = disableAlertConfigurationCommand;
            this._enableAlertConfigurationCommand = enableAlertConfigurationCommand;
            this.ControllerName = "Alert Configuration";
        }

        [HttpGet]
        public async Task<List<GetAlertConfigurationsModel>> GetAll()
        {
            return await _getAlertConfigurationsQuery.Execute();
        }

        [HttpGet]
        public async Task<GetAlertConfigurationByIdModel> GetById(int id)
        {
            return await _getAlertConfigurationByIdQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAlertConfigurationModel model)
        {
            try
            {
                await _createAlertConfigurationCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAlertConfigurationModel updateModel, int id)
        {
            try
            {
                await _updateAlertConfigurationCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                await _disableAlertConfigurationCommand.Execute(id);
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
                await _enableAlertConfigurationCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
