using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.Tenant.Commands.CreateTenant;
using cod.application.Tenant.Commands.DisableTenant;
using cod.application.Tenant.Commands.EnableTenant;
using cod.application.Tenant.Commands.ProfileTenant;
using cod.application.Tenant.Commands.UpdateTenant;
using cod.application.Tenant.Queries.GetTenantById;
using cod.application.Tenant.Queries.GetTenants;

namespace cod.auth.Controllers
{
    [ApiController]
    public class TenantController : BaseController
    {
        private readonly ICreateTenantCommand _createTenantCommand;
        private readonly IUpdateTenantCommand _updateTenantCommand;
        private readonly IDisableTenantCommand _disableTenantCommand;
        private readonly IEnableTenantCommand _enableTenantCommand;
        private readonly IGetTenantByIdQuery _getTenantByIdQuery;
        private readonly IGetTenantsQuery _getTenantsQuery;
        private readonly IProfileTenantCommand _profileTenantCommand;

        public TenantController(ICreateTenantCommand createTenantCommand,
            IUpdateTenantCommand updateTenantCommand,
            IDisableTenantCommand disableTenantCommand,
            IEnableTenantCommand enableTenantCommand,
            IGetTenantByIdQuery getTenantByIdQuery,
            IGetTenantsQuery getTenantsQuery,
            IProfileTenantCommand profileTenantCommand)
        {
            _createTenantCommand = createTenantCommand;
            _updateTenantCommand = updateTenantCommand;
            _disableTenantCommand = disableTenantCommand;
            _enableTenantCommand = enableTenantCommand;
            _getTenantByIdQuery = getTenantByIdQuery;
            _getTenantsQuery = getTenantsQuery;
            _profileTenantCommand = profileTenantCommand;
            this.ControllerName = "Tenant";
        }
        [HttpGet]
        public async Task<GetTenantByIdModel> GetById(int id)
        {
            return await _getTenantByIdQuery.Execute(id);
        }
        [HttpGet]
        public async Task<List<GetTenantsModel>> GetAll()
        {
            return await _getTenantsQuery.Execute();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantModel model)
        {
            try
            {
                await _createTenantCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Profile([FromBody] ProfileTenantModel model, int id)
        {
            try
            {
                await _profileTenantCommand.Execute(model, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTenantModel model, int id)
        {
            try
            {
                await _updateTenantCommand.Execute(model, id);
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
                await _disableTenantCommand.Execute(id);
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
                await _enableTenantCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

    }
}
