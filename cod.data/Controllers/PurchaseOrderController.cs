using cod.application.Features.Models;
using cod.application.PurchaseOrder.Commands.CancelPurchaseOrder;
using cod.application.PurchaseOrder.Commands.CreatePurchaseOrder;
using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using cod.application.PurchaseOrder.Commands.UpdatePurchaseOrder;
using cod.application.PurchaseOrder.Commands.UpdateStatus;
using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPivotPurchaseOrder;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrder;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class PurchaseOrderController : BaseController
    {
        private readonly IGetPurchaseOrderQuery _getPurchaseOrderQuery;
        private readonly IGetPurchasesOrderQuery _getPurchasesOrderQuery;
        private readonly IGetPurchaseOrderPdfQuery _getPurchaseOrderPdfQuery;
        private readonly IGetPivotPurchaseOrderQuery _getPivotPurchaseOrderQuery;
        private readonly ICreatePurchaseOrderCommand _createPurchaseOrderCommand;
        private readonly IUpdatePurchaseOrderCommand _updatePurchaseOrderCommand;
        private readonly IUpdateStatusCommand _updateStatusCommand;
        private readonly IRejectPurchaseOrderCommand _rejectPurchaseOrderCommand;
        private readonly ICancelPurchaseOrderCommand _cancelPurchaseOrderCommand;

        public PurchaseOrderController(
            ICreatePurchaseOrderCommand createPurchaseOrderCommand,
            IUpdatePurchaseOrderCommand updatePurchaseOrderCommand,
            IUpdateStatusCommand updateStatusCommand,
            IRejectPurchaseOrderCommand rejectPurchaseOrderCommand,
            IGetPurchaseOrderQuery getPurchaseOrderQuery,
            IGetPivotPurchaseOrderQuery getPivotPurchaseOrderQuery,
            IGetPurchasesOrderQuery getPurchasesOrderQuery,
            IGetPurchaseOrderPdfQuery getPurchaseOrderPdfQuery,
            ICancelPurchaseOrderCommand cancelPurchaseOrderCommand)
        {
            _createPurchaseOrderCommand = createPurchaseOrderCommand;
            _updatePurchaseOrderCommand = updatePurchaseOrderCommand;
            _updateStatusCommand = updateStatusCommand;
            _rejectPurchaseOrderCommand = rejectPurchaseOrderCommand;
            _getPurchaseOrderQuery = getPurchaseOrderQuery;
            _getPurchasesOrderQuery = getPurchasesOrderQuery;
            _getPurchaseOrderPdfQuery = getPurchaseOrderPdfQuery;
            _cancelPurchaseOrderCommand = cancelPurchaseOrderCommand;
            _getPivotPurchaseOrderQuery = getPivotPurchaseOrderQuery;
        }

        [HttpPost]
        public async Task<List<PurchaseOrderModel>> GetAll([FromBody] FilterPurchaseOrder filter)
        {
            return await _getPurchasesOrderQuery.Execute(filter);
        }

        [HttpPost]
        public async Task<List<object>> GetExpenseReport([FromBody] FilterPurchaseOrder filter)
        {
            return new List<object>();
        }

        [HttpPost]
        public async Task<List<GetPivotPurchaseOrderModel>> GetPivotPurchaseOrder([FromBody] FilterPurchaseOrder filter)
        {
            return await _getPivotPurchaseOrderQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<PurchaseOrderModel> GetById(int id)
        {
            return await _getPurchaseOrderQuery.Execute(id);
        }

        [HttpGet]
        public async Task<string> GetPurchaseOrderFile(int id)
        {
            return await _getPurchaseOrderPdfQuery.Execute(id, false);
        }

        [HttpGet]
        public async Task<string> GetPurchaseOrderVendorFile(int id)
        {
            return await _getPurchaseOrderPdfQuery.Execute(id, true);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseOrderModel model)
        {
            try
            {
                var result = await _createPurchaseOrderCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName, Data = result });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reject([FromBody] RejectPurchaseOrderModel model)
        {
            try
            {
                await _rejectPurchaseOrderCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] RejectPurchaseOrderModel model)
        {
            try
            {
                await _cancelPurchaseOrderCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PurchaseOrderModel updateModel, int id)
        {
            try
            {
                await _updatePurchaseOrderCommand.Execute(updateModel, id);
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
                await _updateStatusCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
