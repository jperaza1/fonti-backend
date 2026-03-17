using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.Users.Commands.CreateUser;
using cod.application.Users.Commands.DisableUser;
using cod.application.Users.Commands.EnableUser;
using cod.application.Users.Commands.UpdateUser;
using cod.application.Users.Queries.GetActiveUsers;
using cod.application.Users.Queries.GetUserById;
using cod.application.Users.Queries.GetUsers;
using cod.domain.Features.Enums;

namespace cod.auth.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ICreateUserCommand _createUserCommand;
        private readonly IUpdateUserCommand _updateUserCommand;
        private readonly IDisableUserCommand _disableUserCommand;
        private readonly IEnableUserCommand _enableUserCommand;
        private readonly IGetUserByIdQuery _getUserByIdQuery;
        private readonly IGetUsersQuery _getUsersQuery;
        private readonly IGetActiveUsersQuery _getActiveUsersQuery;

        public UserController(ICreateUserCommand createUserCommand,
            IUpdateUserCommand updateUserCommand,
            IDisableUserCommand disableUserCommand,
            IEnableUserCommand enableUserCommand,
            IGetUserByIdQuery getUserByIdQuery,
            IGetUsersQuery getUsersQuery,
            IGetActiveUsersQuery getActiveUsersQuery)
        {
            _createUserCommand = createUserCommand;
            _updateUserCommand = updateUserCommand;
            _disableUserCommand = disableUserCommand;
            _enableUserCommand = enableUserCommand;
            _getUserByIdQuery = getUserByIdQuery;
            _getUsersQuery = getUsersQuery;
            _getActiveUsersQuery = getActiveUsersQuery;
            this.ControllerName = "User";
        }

        [HttpGet]
        public async Task<GetUserByIdModel> GetById(int id)
        {
            return await _getUserByIdQuery.Execute(id);
        }
        [HttpGet]
        public async Task<List<GetUsersModel>> GetAll()
        {
            return await _getUsersQuery.Execute();
        }
        [HttpGet]
        public async Task<List<GetUsersModel>> GetActives()
        {
            return await _getActiveUsersQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserModel model)
        {
            try
            {
                await _createUserCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel model, int id)
        {
            try
            {
                await _updateUserCommand.Execute(model, id);
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
                await _disableUserCommand.Execute(id);
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
                await _enableUserCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
