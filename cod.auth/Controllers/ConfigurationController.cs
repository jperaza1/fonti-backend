using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cod.application.Configuration.Commands.CreateConfiguration;
using cod.application.Configuration.Commands.DisableConfiguration;
using cod.application.Configuration.Commands.EnableConfiguration;
using cod.application.Configuration.Commands.UpdateConfiguration;
using cod.application.Configuration.Queries.GetActivesConfigurations;
using cod.application.Configuration.Queries.GetConfigurationById;
using cod.application.Configuration.Queries.GetConfigurationByKey;
using cod.application.Configuration.Queries.GetConfigurations;
using cod.application.Features.Models;

namespace cod.auth.Controllers
{
    [ApiController]
    [Authorize]
    public class ConfigurationController : BaseController
    {
        private readonly IGetConfigurationsQuery _getConfigurationsQuery;
        private readonly IGetConfigurationByIdQuery _getConfigurationByIdQuery;
        private readonly ICreateConfigurationCommand _createConfigurationCommand;
        private readonly IUpdateConfigurationCommand _updateConfigurationCommand;
        private readonly IEnableConfigurationCommand _enableConfigurationCommand;
        private readonly IDisableConfigurationCommand _disableConfigurationCommand;
        private readonly IGetActivesConfigurationsQuery _getActivesConfigurationsQuery;
        private readonly IGetConfigurationByKeyQuery _getConfigurationByKeyQuery;

        public ConfigurationController(IGetConfigurationsQuery getConfigurationsQuery,
            IGetConfigurationByIdQuery getConfigurationByIdQuery,
            ICreateConfigurationCommand createConfigurationCommand,
            IUpdateConfigurationCommand updateConfigurationCommand,
            IEnableConfigurationCommand enableConfigurationCommand,
            IDisableConfigurationCommand disableConfigurationCommand,
            IGetActivesConfigurationsQuery getActivesConfigurationsQuery,
            IGetConfigurationByKeyQuery getConfigurationByKeyQuery)
        {
            _getConfigurationsQuery = getConfigurationsQuery;
            _getConfigurationByIdQuery = getConfigurationByIdQuery;
            _createConfigurationCommand = createConfigurationCommand;
            _updateConfigurationCommand = updateConfigurationCommand;
            _enableConfigurationCommand = enableConfigurationCommand;
            _disableConfigurationCommand = disableConfigurationCommand;
            _getActivesConfigurationsQuery = getActivesConfigurationsQuery;
            this.ControllerName = "Configuration";
            _getConfigurationByKeyQuery = getConfigurationByKeyQuery;
        }

        [HttpGet]
        public async Task<GetConfigurationByIdModel> GetById(int id)
        {
            return await _getConfigurationByIdQuery.Execute(id);
        }

        [HttpGet]
        public async Task<GetConfigurationsModel> GetByKey(string key)
        {
            return await _getConfigurationByKeyQuery.Execute(key);
        }

        [HttpGet]
        public async Task<List<GetConfigurationsModel>> GetAll()
        {
            return await _getConfigurationsQuery.Execute();
        }

        [HttpGet]
        public async Task<List<GetActivesConfigurationsQueryModel>> GetActives()
        {
            return await _getActivesConfigurationsQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConfigurationModel model)
        {
            try
            {
                await _createConfigurationCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateConfigurationModel model, int id)
        {
            try
            {
                await _updateConfigurationCommand.Execute(model, id);
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
                await _disableConfigurationCommand.Execute(id);
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
                await _enableConfigurationCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
