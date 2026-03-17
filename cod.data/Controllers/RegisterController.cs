using cod.application.Cafeteria.Commands;
using cod.application.Features.Models;
using cod.application.Register.Commands.CreateRegister;
using cod.application.Register.Commands.DisableRegister;
using cod.application.Register.Commands.EnableRegister;
using cod.application.Register.Commands.UpdateRegister;
using cod.application.Register.Common;
using cod.application.Register.Queries.GetActiveRegisters;
using cod.application.Register.Queries.GetRegister;
using cod.application.Register.Queries.GetRegisters;
using cod.application.Register.Queries.GetRegistersByCafeteria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    
    [ApiController]
    public class RegisterController : BaseController
    {
        private readonly IGetActiveRegistersQuery _getActiveRegistersQuery;
        private readonly IGetRegisterQuery _getRegisterQuery;
        private readonly IGetRegistersQuery _getRegistersQuery;
        private readonly IGetRegistersByCafeteriaQuery _getRegistersByCafeteriaQuery;
        private readonly ICreateRegisterCommand _createRegisterCommand;
        private readonly IUpdateRegisterCommand _updateRegisterCommand;
        private readonly IDisableRegisterCommand _disableRegisterCommand;
        private readonly IEnableRegisterCommand _enableRegisterCommand;
        public RegisterController(
            IGetActiveRegistersQuery getActiveRegistersQuery,
            IGetRegisterQuery getRegisterQuery,
            IGetRegistersQuery getRegistersQuery,
            IGetRegistersByCafeteriaQuery getRegistersByCafeteriaQuery,
            ICreateRegisterCommand createRegisterCommand,
            IUpdateRegisterCommand updateRegisterCommand,
            IEnableRegisterCommand enableRegisterCommand,
            IDisableRegisterCommand disableRegisterCommand
            ) 
        {
            this._getActiveRegistersQuery = getActiveRegistersQuery;
            this._getRegisterQuery = getRegisterQuery;
            this._getRegistersQuery = getRegistersQuery;
            this._getRegistersByCafeteriaQuery = getRegistersByCafeteriaQuery;
            this._createRegisterCommand = createRegisterCommand;
            this._updateRegisterCommand = updateRegisterCommand;
            this._enableRegisterCommand = enableRegisterCommand;
            this._disableRegisterCommand = disableRegisterCommand;
        }

        [HttpGet]
        public async Task<List<RegisterModel>> GetAll()
        {
            return await _getRegistersQuery.Execute();
        }

        [HttpGet]
        public async Task<RegisterModel> GetById(int id)
        {
            return await _getRegisterQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<RegisterModel>> GetAllActive()
        {
            return await _getActiveRegistersQuery.Execute();
        }

        [HttpGet]
        public async Task<List<RegisterModel>> GetByCafeteria(int cafeteriaId)
        {
            return await _getRegistersByCafeteriaQuery.Execute(cafeteriaId, false);
        }

        [HttpGet]
        public async Task<List<RegisterModel>> GetByCafeteriaInventory(int cafeteriaId)
        {
            return await _getRegistersByCafeteriaQuery.Execute(cafeteriaId, true);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterModel model)
        {
            try
            {
                await _createRegisterCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RegisterModel updateModel, int id)
        {
            try
            {
                await _updateRegisterCommand.Execute(updateModel, id);
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
                await _disableRegisterCommand.Execute(id);
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
                await _enableRegisterCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

    }
}
