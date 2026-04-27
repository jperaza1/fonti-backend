using cod.application.Cafeteria.Commands;
using cod.application.Cafeteria.Common.CreateCafeteria;
using cod.application.Cafeteria.Common.DisableCafeteria;
using cod.application.Cafeteria.Common.EnableCafeteria;
using cod.application.Cafeteria.Common.UpdateCaferia;
using cod.application.Cafeteria.Queries.GetActiveCafeterias;
using cod.application.Cafeteria.Queries.GetCafeteria;
using cod.application.Cafeteria.Queries.GetCafeterias;
using cod.application.Features.Models;
using cod.application.TypeGroup.Commands.CreateTypeGroup;
using cod.application.TypeGroup.Commands.UpdateTypeGroup;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class CafeteriaController : BaseController
    {
        private readonly IGetCafeteriasQuery _getCafeteriasQuery;
        private readonly IGetCafeteriaQuery _getCafeteriaQuery;
        private readonly IGetActiveCafeteriasQuery _getActiveCafeteriasQuery;
        private readonly ICreateCafeteriaCommand _createCafeteriaCommand;
        private readonly IUpdateCaferiaCommand _updateCaferiaCommand;
        private readonly IEnableCafeteriaCommand _enableCafeteriaCommand;
        private readonly IDisableCafeteriaCommand _disableCafeteriaCommand;

        public CafeteriaController(
            IGetCafeteriasQuery getCafeteriasQuery,
            IGetCafeteriaQuery getCafeteriaQuery,
            IGetActiveCafeteriasQuery getActiveCafeteriasQuery,
            ICreateCafeteriaCommand createCafeteriaCommand,
            IUpdateCaferiaCommand updateCaferiaCommand,
            IEnableCafeteriaCommand enableCafeteriaCommand,
            IDisableCafeteriaCommand disableCafeteriaCommand)
        {
            _getCafeteriasQuery = getCafeteriasQuery;
            _getCafeteriaQuery = getCafeteriaQuery;
            _getActiveCafeteriasQuery = getActiveCafeteriasQuery;
            _createCafeteriaCommand = createCafeteriaCommand;
            _updateCaferiaCommand = updateCaferiaCommand;
            _enableCafeteriaCommand = enableCafeteriaCommand;
            _disableCafeteriaCommand = disableCafeteriaCommand;
        }

        [HttpGet]
        public async Task<List<CafeteriaModel>> GetAll()
        {
            return await _getCafeteriasQuery.Execute();
        }

        [HttpGet]
        public async Task<CafeteriaModel> GetById(int id)
        {
            return await _getCafeteriaQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<CafeteriaModel>> GetAllActive()
        {
            return await _getActiveCafeteriasQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CafeteriaModel model)
        {
            try
            {
                await _createCafeteriaCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CafeteriaModel updateModel, int id)
        {
            try
            {
                await _updateCaferiaCommand.Execute(updateModel, id);
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
                await _disableCafeteriaCommand.Execute(id);
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
                await _enableCafeteriaCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
