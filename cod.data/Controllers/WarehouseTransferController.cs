using cod.application.Features.Models;
using cod.application.WarehouseTransfer.Commands.AcceptWarehouseTransfer;
using cod.application.WarehouseTransfer.Commands.CreateWarehouseTransfer;
using cod.application.WarehouseTransfer.Commands.RejectWarehouseTransfer;
using cod.application.WarehouseTransfer.Common;
using cod.application.WarehouseTransfer.Queries.GetWarehouseTransfer;
using cod.application.WarehouseTransfer.Queries.GetWarehouseTransfers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class WarehouseTransferController : BaseController
    {
        private readonly IGetWarehouseTransfersQuery _getWarehouseTransfersQuery;
        private readonly IGetWarehouseTransferQuery _getWarehouseTransferQuery;
        private readonly ICreateWarehouseTransferCommand _createWarehouseTransferCommand;
        private readonly IAcceptWarehouseTransferCommand _acceptWarehouseTransferCommand;
        private readonly IRejectWarehouseTransferCommand _rejectWarehouseTransferCommand;

        public WarehouseTransferController(
            IGetWarehouseTransfersQuery getWarehouseTransfersQuery,
            IGetWarehouseTransferQuery getWarehouseTransferQuery,
            ICreateWarehouseTransferCommand createWarehouseTransferCommand,
            IAcceptWarehouseTransferCommand acceptWarehouseTransferCommand,
            IRejectWarehouseTransferCommand rejectWarehouseTransferCommand)
        {
            _getWarehouseTransfersQuery = getWarehouseTransfersQuery;
            _getWarehouseTransferQuery = getWarehouseTransferQuery;
            _createWarehouseTransferCommand = createWarehouseTransferCommand;
            _acceptWarehouseTransferCommand = acceptWarehouseTransferCommand;
            _rejectWarehouseTransferCommand = rejectWarehouseTransferCommand;
        }

        [HttpGet]
        public async Task<List<WarehouseTransferModel>> GetAll()
        {
            return await _getWarehouseTransfersQuery.Execute();
        }

        [HttpGet]
        public async Task<WarehouseTransferModel> GetById(int id)
        {
            return await _getWarehouseTransferQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WarehouseTransferModel model)
        {
            try
            {
                var id = await _createWarehouseTransferCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName, Data = id });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            try
            {
                await _acceptWarehouseTransferCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reject([FromBody] RejectWarehouseTransferModel model)
        {
            try
            {
                await _rejectWarehouseTransferCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
