using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.TypeGroup.Commands.CreateTypeGroup;
using cod.application.TypeGroup.Commands.DisableTypeGroup;
using cod.application.TypeGroup.Commands.EnableTypeGroup;
using cod.application.TypeGroup.Commands.UpdateTypeGroup;
using cod.application.TypeGroup.Queries.GetActiveTypeGroup;
using cod.application.TypeGroup.Queries.GetTypeGroupByCode;
using cod.application.TypeGroup.Queries.GetTypeGroups;

namespace cod.data.Controllers
{
    [ApiController]
    public class TypeGroupController : BaseController
    {
        private readonly ICreateTypeGroupCommand _createTypeGroupCommand;
        private readonly IUpdateTypeGroupCommand _updateTypeGroupCommand;
        private readonly IDisableTypeGroupCommand _disableTypeGroupCommand;
        private readonly IEnableTypeGroupCommand _enableTypeGroupCommand;
        private readonly IGetTypeGroupsQuery _getTypeGroupsQuery;
        private readonly IGetActiveTypeGroupsQuery _getActiveTypeGroupsQuery;
        private readonly IGetTypeGroupByCodeQuery _getTypeGroupByCodeQuery;

        public TypeGroupController(ICreateTypeGroupCommand createTypeGroupCommand,
            IUpdateTypeGroupCommand updateTypeGroupCommand,
            IDisableTypeGroupCommand disableTypeGroupCommand,
            IEnableTypeGroupCommand enableTypeGroupCommand,
            IGetTypeGroupsQuery getTypeGroupsQuery,
            IGetActiveTypeGroupsQuery getActiveTypeGroupsQuery,
            IGetTypeGroupByCodeQuery getTypeGroupByCodeQuery)
        {
            _createTypeGroupCommand = createTypeGroupCommand;
            _updateTypeGroupCommand = updateTypeGroupCommand;
            _disableTypeGroupCommand = disableTypeGroupCommand;
            _enableTypeGroupCommand = enableTypeGroupCommand;
            _getTypeGroupsQuery = getTypeGroupsQuery;
            _getActiveTypeGroupsQuery = getActiveTypeGroupsQuery;
            _getTypeGroupByCodeQuery = getTypeGroupByCodeQuery;
            this.ControllerName = "TypeGroup";
        }

        [HttpGet]
        public async Task<GetTypeGroupByCodeModel> GetTypeGroupByCode(string code)
        {
            return await _getTypeGroupByCodeQuery.Execute(code);
        }
        [HttpGet]
        public async Task<List<GetTypeGroupsModel>> GetAll()
        {
            return await _getTypeGroupsQuery.Execute();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTypeGroupModel createModel)
        {
            try
            {
                await _createTypeGroupCommand.Execute(createModel);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTypeGroupModel updateModel, int id)
        {
            try
            {
                await _updateTypeGroupCommand.Execute(updateModel, id);
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
                await _disableTypeGroupCommand.Execute(id);
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
                await _enableTypeGroupCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpGet]
        public async Task<List<GetActiveTypeGroupModel>> GetActives()
        {
            return await _getActiveTypeGroupsQuery.Execute();
        }



    }
}
