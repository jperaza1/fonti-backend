using cod.application.Correlative.Common;
using cod.application.DispatchControl.Commands.CreateDispatchControl;
using cod.application.DispatchControl.Commands.UpdateDispatchControl;
using cod.application.DispatchControl.Commom;
using cod.application.DispatchControl.Queries.GenerateReportDispatchControl;
using cod.application.DispatchControl.Queries.GetAllDispatchControl;
using cod.application.DispatchControl.Queries.GetDispatchControl;
using cod.application.Features.Models;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{

    [ApiController]
    public class DispatchControlController : BaseController
    {
        private readonly ICreateDispatchControlCommand _createDispatchControlCommand;
        private readonly IUpdateDispatchControlCommand _updateDispatchControlCommand;
        private readonly IGetDispatchControlQuery _getDispatchControlQuery;
        private readonly IGetAllDispatchControlQuery _getAllDispatchControlQuery;
        private readonly IGenerateReportDispatchControlQuery _generateReportDispatchControlQuery;
        public DispatchControlController(
            ICreateDispatchControlCommand createDispatchControlCommand,
            IUpdateDispatchControlCommand updateDispatchControlCommand,
            IGetDispatchControlQuery getDispatchControlQuery,
            IGetAllDispatchControlQuery getAllDispatchControlQuery,
            IGenerateReportDispatchControlQuery generateReportDispatchControlQuery)
        {
            _getDispatchControlQuery = getDispatchControlQuery;
            _getAllDispatchControlQuery = getAllDispatchControlQuery;
            _generateReportDispatchControlQuery = generateReportDispatchControlQuery;
            _updateDispatchControlCommand = updateDispatchControlCommand;
            _createDispatchControlCommand = createDispatchControlCommand;
        }

        [HttpPost]
        public async Task<List<DispatchControlModel>> GetAll([FromBody] DispatchControlFilterModel filter)
        {
            return await _getAllDispatchControlQuery.Execute(filter);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] DispatchControlFilterModel filter)
        {
            var response = await _generateReportDispatchControlQuery.Execute(filter);
            return Ok(new
            {
                FileName = "Reporte Control de Salidas.xlsx",
                File = Convert.ToBase64String(response) // o el span/byte[] que corresponda
            });
        }

        [HttpGet]
        public async Task<DispatchControlModel> GetById(int id)
        {
            return await _getDispatchControlQuery.Execute(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DispatchControlModel model, int id)
        {
            try
            {
                await _updateDispatchControlCommand.Execute(model, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DispatchControlModel model)
        {
            try
            {
                await _createDispatchControlCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
