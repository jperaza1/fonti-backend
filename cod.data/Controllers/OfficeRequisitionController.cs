using cod.application.Features.Models;
using cod.application.OfficeRequisition.Commands.CreateOfficeRequisition;
using cod.application.OfficeRequisition.Commands.RejectOfficeRequisition;
using cod.application.OfficeRequisition.Commands.UpdateOfficeRequisition;
using cod.application.OfficeRequisition.Commands.UpdateStatus;
using cod.application.OfficeRequisition.Common;
using cod.application.OfficeRequisition.Queries.GetOfficeRequisition;
using cod.application.OfficeRequisition.Queries.GetOfficeRequisitionPdf;
using cod.application.OfficeRequisition.Queries.GetOfficeRequisitions;
using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class OfficeRequisitionController : BaseController
    {
        private readonly IGetOfficeRequisitionQuery _getOfficeRequisitionQuery;
        private readonly IGetOfficeRequisitionsQuery _getOfficeRequisitionsQuery;
        private readonly IGetOfficeRequisitionPdfQuery _getOfficeRequisitionPdfQuery;
        private readonly ICreateOfficeRequisitionCommand _createOfficeRequisitionCommand;
        private readonly IUpdateOfficeRequisitionCommand _updateOfficeRequisitionCommand;
        private readonly IUpdateStatusOfficeRequisitionCommand _updateStatusOfficeRequisitionCommand;
        private readonly IRejectOfficeRequisitionCommand _rejectOfficeRequisitionCommand;

        public OfficeRequisitionController(
            IGetOfficeRequisitionQuery getOfficeRequisitionQuery,
            IGetOfficeRequisitionsQuery getOfficeRequisitionsQuery,
            IGetOfficeRequisitionPdfQuery getOfficeRequisitionPdfQuery,
            ICreateOfficeRequisitionCommand createOfficeRequisitionCommand,
            IUpdateOfficeRequisitionCommand updateOfficeRequisitionCommand,
            IUpdateStatusOfficeRequisitionCommand updateStatusOfficeRequisitionCommand,
            IRejectOfficeRequisitionCommand rejectOfficeRequisitionCommand)
        {
            _getOfficeRequisitionQuery = getOfficeRequisitionQuery;
            _getOfficeRequisitionsQuery = getOfficeRequisitionsQuery;
            _getOfficeRequisitionPdfQuery = getOfficeRequisitionPdfQuery;
            _createOfficeRequisitionCommand = createOfficeRequisitionCommand;
            _updateOfficeRequisitionCommand = updateOfficeRequisitionCommand;
            _updateStatusOfficeRequisitionCommand = updateStatusOfficeRequisitionCommand;
            _rejectOfficeRequisitionCommand = rejectOfficeRequisitionCommand;
        }

        [HttpPost]
        public async Task<List<OfficeRequisitionModel>> GetAll([FromBody] FilterOfficeRequisition filter)
        {
            return await _getOfficeRequisitionsQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<OfficeRequisitionModel> GetById(int id)
        {
            return await _getOfficeRequisitionQuery.Execute(id);
        }

        [HttpGet]
        public async Task<string> GetOfficeRequisitionFile(int id)
        {
            return await _getOfficeRequisitionPdfQuery.Execute(id);
        }


        [HttpPost]
        public async Task<IActionResult> Reject([FromBody] RejectPurchaseOrderModel model)
        {
            try
            {
                await _rejectOfficeRequisitionCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OfficeRequisitionModel model)
        {
            try
            {
                await _createOfficeRequisitionCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OfficeRequisitionModel updateModel, int id)
        {
            try
            {
                await _updateOfficeRequisitionCommand.Execute(updateModel, id);
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
                await _updateStatusOfficeRequisitionCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
