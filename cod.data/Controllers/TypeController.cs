using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.Type.Commands.CreateType;
using cod.application.Type.Commands.DisableType;
using cod.application.Type.Commands.EnableType;
using cod.application.Type.Commands.UpdateType;
using cod.application.Type.Queries.GetActivesTypes;
using cod.application.Type.Queries.GetActivesTypesByGroupCode;
using cod.application.Type.Queries.GetTypes;
using cod.application.Type.Queries.GetTypesByGroupCode;
using cod.application.Type.Queries.GetTypesByGroupCodeList;

namespace cod.data.Controllers
{
    [ApiController]
    public class TypeController : BaseController
    {
        private readonly IGetTypesByGroupCodeQuery _getTypesByGroupCodeQuery;
        private readonly IGetTypesByGroupCodeListQuery _getTypesByGroupCodeListQuery;
        private readonly IGetActivesTypesByGroupCodeQuery _getActivesTypesByGroupCodeQuery;
        private readonly IGetTypesQuery _getTypesQuery;
        private readonly IGetActivesTypesQuery _getActiveTypesQuery;
        private readonly ICreateTypeCommand _createTypeCommand;
        private readonly IUpdateTypeCommand _updateTypeCommand;
        private readonly IDisableTypeCommand _disableTypeCommand;
        private readonly IEnableTypeCommand _enableTypeCommand;

        public TypeController(IGetTypesByGroupCodeQuery getTypesByGroupCodeQuery,
            IGetTypesByGroupCodeListQuery getTypesByGroupCodeListQuery,
            IGetActivesTypesByGroupCodeQuery getActivesTypesByGroupCodeQuery,
            IGetTypesQuery getTypesQuery,
            IGetActivesTypesQuery getActiveTypesQuery,
            ICreateTypeCommand createTypeCommand,
            IUpdateTypeCommand updateTypeCommand,
            IDisableTypeCommand disableTypeCommand,
            IEnableTypeCommand enableTypeCommand


            )
        {
            _getTypesByGroupCodeQuery = getTypesByGroupCodeQuery;
            _getTypesByGroupCodeListQuery = getTypesByGroupCodeListQuery;
            this.ControllerName = "Types";
            _getActivesTypesByGroupCodeQuery = getActivesTypesByGroupCodeQuery;
            _getTypesQuery = getTypesQuery;
            _getActiveTypesQuery = getActiveTypesQuery;
            _createTypeCommand = createTypeCommand;
            _updateTypeCommand = updateTypeCommand;
            _disableTypeCommand = disableTypeCommand;
            _enableTypeCommand = enableTypeCommand;


        }
        [HttpGet]
        public async Task<List<GetTypesByGroupCodeModel>> GetTypesByGroupCode(string groupCode)
        {
            return await _getTypesByGroupCodeQuery.Execute(groupCode);
        }
        [HttpGet]
        public async Task<List<GetTypesByGroupCodeModel>> GetAciteTypesByGroupCode(string groupCode)
        {
            return await _getActivesTypesByGroupCodeQuery.Execute(groupCode);
        }
        [HttpPost]
        public async Task<Dictionary<string, List<GetTypesByGroupCodeModel>>> GetTypesByGroupCodeList(List<GetTypesByGroupCodeListModel> groupCode)
        {
            return await _getTypesByGroupCodeListQuery.Execute(groupCode);
        }

        [HttpGet]
        public async Task<List<GetTypeModel>> GetAll()
        {
            return await _getTypesQuery.Execute();
        }

        [HttpGet]
        public async Task<List<GetActivesTypesModel>> GetActives()
        {
            return await _getActiveTypesQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTypeModel createModel)
        {
            try
            {
                await _createTypeCommand.Execute(createModel);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTypeModel updateModel, int id)
        {
            try
            {
                await _updateTypeCommand.Execute(updateModel, id);
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
                await _disableTypeCommand.Execute(id);
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
                await _enableTypeCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }













    }
}
