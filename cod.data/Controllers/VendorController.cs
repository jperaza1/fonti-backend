
using cod.application.Features.Models;
using cod.application.Vendor.Commands.CreateVendor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using cod.application.Vendor.Common;
using Microsoft.AspNetCore.Authorization;
using cod.application.Vendor.Queries.GetVendors;
using cod.application.Cafeteria.Commands;
using System.Collections.Generic;
using cod.application.Vendor.Queries.GetVendor;
using cod.application.Vendor.Queries.GetActiveVendors;
using cod.application.Vendor.Commands.UpdateVendor;
using cod.application.Vendor.Commands.DisableVendor;
using cod.application.Vendor.Commands.EnableVendor;

namespace cod.data.Controllers
{
    [ApiController]
    public class VendorController : BaseController
    {
        private readonly IGetVendorsQuery _getVendorsQuery;
        private readonly IGetVendorQuery _getVendorQuery;
        private readonly IGetActiveVendorsQuery _getActiveVendorsQuery;
        private readonly ICreateVendorCommand _createVendorCommand;
        private readonly IUpdateVendorCommand _updateVendorCommand;
        private readonly IDisableVendorCommand _disableVendorCommand;
        private readonly IEnableVendorCommand _enableVendorCommand;

        public VendorController(
            ICreateVendorCommand createVendorCommand, 
            IGetVendorsQuery getVendorsQuery,
            IGetVendorQuery getVendorQuery,
            IGetActiveVendorsQuery getActiveVendorsQuery,
            IUpdateVendorCommand updateVendorCommand,
            IDisableVendorCommand disableVendorCommand,
            IEnableVendorCommand enableVendorCommand)
        {
            _getVendorsQuery = getVendorsQuery;
            _getVendorQuery = getVendorQuery;
            _getActiveVendorsQuery = getActiveVendorsQuery;
            _createVendorCommand = createVendorCommand;
            this.ControllerName = "Proveedores";
            _updateVendorCommand = updateVendorCommand;
            _disableVendorCommand = disableVendorCommand;
            _enableVendorCommand = enableVendorCommand;
        }

        [HttpGet]
        public async Task<List<VendorModel>> GetAll()
        {
            return await _getVendorsQuery.Execute();
        }

        [HttpGet]
        public async Task<VendorModel> GetById(int id)
        {
            return await _getVendorQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<VendorModel>> GetAllActive()
        {
            return await _getActiveVendorsQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorModel createModel)
        {
            try
            {
                await _createVendorCommand.Execute(createModel);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VendorModel updateModel, int id)
        {
            try
            {
                await _updateVendorCommand.Execute(updateModel, id);
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
                await _disableVendorCommand.Execute(id);
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
                await _enableVendorCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
