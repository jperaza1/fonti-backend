using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LOGISTIC.CONTAINER.APPLICATION.GroupRole.Queries.GetGroupRolePermissions;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.GroupRole.Commands.CreateGroupRole;
using cod.application.GroupRole.Commands.UpdateGroupRole;
using cod.application.GroupRole.Queries.GetGroupRoleById;
using cod.application.GroupRole.Queries.GetGroupRolePermissions;
using cod.application.GroupRole.Queries.GetGroupRoles;
using cod.application.GroupRole.Queries.GetPermissions;
using cod.application.GroupRole.Queries.GetPermissionsByCurrentTenant;

namespace cod.auth.Controllers
{
    [ApiController]
    public class GroupRoleController : BaseController
    {
        private readonly ICreateGroupRoleCommand _createGroupRoleCommand;
        private readonly IUpdateGroupRoleCommand _updateGroupRoleCommand;
        private readonly IGetGroupRolesQuery _getGroupRolesQuery;
        private readonly IGetGroupRoleByIdQuery _getGroupRoleByIdQuery;
        private readonly IGetPermissionsQuery _getPermissionsQuery;
        private readonly IGetGroupRolePermissionsQuery _getGroupRolePermissionsQuery;
        private readonly IGetPermissionsByCurrentTenantQuery _getPermissionsByCurrentTenantQuery;

        public GroupRoleController(ICreateGroupRoleCommand createGroupRoleCommand,
            IUpdateGroupRoleCommand updateGroupRoleCommand,
            IGetGroupRolesQuery getGroupRolesQuery,
            IGetGroupRoleByIdQuery getGroupRoleByIdQuery,
            IGetPermissionsQuery getPermissionsQuery,
            IGetGroupRolePermissionsQuery getGroupRolePermissionsQuery,
            IGetPermissionsByCurrentTenantQuery getPermissionsByCurrentTenantQuery)
        {
            _createGroupRoleCommand = createGroupRoleCommand;
            _updateGroupRoleCommand = updateGroupRoleCommand;
            _getGroupRolesQuery = getGroupRolesQuery;
            _getGroupRoleByIdQuery = getGroupRoleByIdQuery;
            _getPermissionsQuery = getPermissionsQuery;
            _getGroupRolePermissionsQuery = getGroupRolePermissionsQuery;
            _getPermissionsByCurrentTenantQuery = getPermissionsByCurrentTenantQuery;
            this.ControllerName = "GROUP ROLES";
        }
        [HttpGet]
        public async Task<List<GetGroupPermissionModel>> GetPermissionsGroupedByTenant()
        {
            return await _getPermissionsByCurrentTenantQuery.Execute();
        }
        [HttpGet]
        public async Task<List<GetGroupPermissionModel>> GetPermissionsGrouped()
        {
            return await _getGroupRolePermissionsQuery.Execute();
        }
        [HttpGet]
        public async Task<GetGroupRoleByIdModel> GetById(int id)
        {
            return await _getGroupRoleByIdQuery.Execute(id);
        }
        [HttpGet]
        public async Task<List<GetGroupRolesModel>> GetAll()
        {
            return await _getGroupRolesQuery.Execute();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupRoleModel model)
        {
            try
            {
                await _createGroupRoleCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateGroupRoleModel model)
        {
            try
            {
                await _updateGroupRoleCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
        [HttpGet]
        public async Task<List<GetPermissionsModel>> GetAllPermissions()
        {
            return await _getPermissionsQuery.Execute();
        }
    }
}
