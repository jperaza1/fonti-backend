using cod.application.Features.Models;
using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.application.SpecialService.Commands.CreateSpecialService;
using cod.application.SpecialService.Commands.RejectSpecialService;
using cod.application.SpecialService.Commands.UpdateSpecialService;
using cod.application.SpecialService.Commands.UpdateStatus;
using cod.application.SpecialService.Common;
using cod.application.SpecialService.Queries.GetSpecialService;
using cod.application.SpecialService.Queries.GetSpecialServicePdf;
using cod.application.SpecialService.Queries.GetSpecialServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class SpecialServicesController : BaseController
    {
        private readonly IGetSpecialServicesQuery _getSpecialServicesQuery;
        private readonly IGetSpecialServiceQuery _getSpecialServiceQuery;
        private readonly IGetSpecialServicePdfQuery _getSpecialServicePdfQuery;
        private readonly ICreateSpecialServiceCommand _createSpecialServiceCommand;
        private readonly IUpdateSpecialServiceCommand _updateSpecialServiceCommand;
        private readonly IUpdateStatusSpecialServiceCommand _updateStatusSpecialServiceCommand;
        private readonly IRejectSpecialServiceCommand _rejectSpecialServiceCommand;

        public SpecialServicesController(
            IGetSpecialServicesQuery getSpecialServicesQuery,
            IGetSpecialServiceQuery getSpecialServiceQuery,
            IGetSpecialServicePdfQuery getSpecialServicePdfQuery,
            ICreateSpecialServiceCommand createSpecialServiceCommand,
            IUpdateSpecialServiceCommand updateSpecialServiceCommand,
            IUpdateStatusSpecialServiceCommand updateStatusSpecialServiceCommand,
            IRejectSpecialServiceCommand rejectSpecialServiceCommand)
        {
            _getSpecialServicesQuery = getSpecialServicesQuery;
            _getSpecialServiceQuery = getSpecialServiceQuery;
            _getSpecialServicePdfQuery = getSpecialServicePdfQuery;
            _createSpecialServiceCommand = createSpecialServiceCommand;
            _updateSpecialServiceCommand = updateSpecialServiceCommand;
            _updateStatusSpecialServiceCommand = updateStatusSpecialServiceCommand;
            _rejectSpecialServiceCommand = rejectSpecialServiceCommand;
        }

        [HttpPost]
        public async Task<List<SpecialServiceModel>> GetAll([FromBody] FilterSpecialService filter)
        {
            return await _getSpecialServicesQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<SpecialServiceModel> GetById(int id)
        {
            return await _getSpecialServiceQuery.Execute(id);
        }

        [HttpGet]
        public async Task<string> GetSpecialServiceFile(int id)
        {
            return await _getSpecialServicePdfQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Reject([FromBody] RejectPurchaseOrderModel model)
        {
            try
            {
                await _rejectSpecialServiceCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecialServiceModel model)
        {
            try
            {
                await _createSpecialServiceCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SpecialServiceModel updateModel, int id)
        {
            try
            {
                await _updateSpecialServiceCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                await _updateStatusSpecialServiceCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }


    }
}
