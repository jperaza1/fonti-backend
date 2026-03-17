using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.Role.Commands.CreateRole;
using cod.application.Role.Commands.UpdateRole;
using cod.application.Role.Queries.GetRoleById;
using cod.application.Role.Queries.GetRoles;

namespace cod.auth.Controllers
{
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly ICreateRoleCommand _createRoleCommand;
        private readonly IUpdateRoleCommand _updateRoleCommand;
        private readonly IGetRolesQuery _getRolesQuery;
        private readonly IGetRoleByIdQuery _getRoleByIdQuery;

        public RoleController(ICreateRoleCommand createRoleCommand,
            IUpdateRoleCommand updateRoleCommand,
            IGetRolesQuery getRolesQuery,
            IGetRoleByIdQuery getRoleByIdQuery)
        {
            _createRoleCommand = createRoleCommand;
            _updateRoleCommand = updateRoleCommand;
            _getRolesQuery = getRolesQuery;
            _getRoleByIdQuery = getRoleByIdQuery;
            this.ControllerName = "ROLES";
        }
        [HttpGet]
        public async Task<GetRoleByIdModel> GetById(int id)
        {
            return await _getRoleByIdQuery.Execute(id);
        }
        [HttpGet]
        public async Task<List<GetRolesModel>> GetAll()
        {
            return await _getRolesQuery.Execute();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleModel model)
        {
            try
            {
                await _createRoleCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleModel model)
        {
            try
            {
                await _updateRoleCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
