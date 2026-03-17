using cod.application.Features.Models;
using cod.application.Inventory.Commands.CretaeInventory;
using cod.application.Inventory.Commands.UpdateInventory;
using cod.application.Inventory.Common;
using cod.application.Inventory.Queries.GetInventories;
using cod.application.Inventory.Queries.GetInventory;
using cod.application.Inventory.Queries.GetLastInventoryByCafeteria;
using cod.application.Inventory.Queries.GetResumeInventory;
using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class InventoryController : BaseController
    {
        private readonly IGetInventoriesQuery _getInventoriesQuery;
        private readonly IGetInventoryQuery _getInventoryQuery;
        private readonly IGetLastInventoryByCafeteriaQuery _getLastInventoryByCafeteriaQuery;
        private readonly IGetResumeInventoryQuery _getResumeInventoryQuery;
        private readonly ICretaeInventoryCommand _cretaeInventoryCommand;
        private readonly IUpdateInventoryCommand _updateInventoryCommand;

        public InventoryController(
            IGetInventoriesQuery getInventoriesQuery,
            IGetInventoryQuery getInventoryQuery,
            IGetResumeInventoryQuery getResumeInventoryQuery,
            IGetLastInventoryByCafeteriaQuery getLastInventoryByCafeteriaQuery,
            ICretaeInventoryCommand cretaeInventoryCommand,
            IUpdateInventoryCommand updateInventoryCommand)
        {
            _getInventoriesQuery = getInventoriesQuery;
            _getInventoryQuery = getInventoryQuery;
            _getLastInventoryByCafeteriaQuery = getLastInventoryByCafeteriaQuery;
            _getResumeInventoryQuery = getResumeInventoryQuery;
            _cretaeInventoryCommand = cretaeInventoryCommand;
            _updateInventoryCommand = updateInventoryCommand;
        }

        [HttpPost]
        public async Task<List<InventoryModel>> GetAll([FromBody] FilterInventory filter)
        {
            return await _getInventoriesQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<InventoryModel> GetById(int id)
        {
            return await _getInventoryQuery.Execute(id);
        }

        [HttpGet]
        public async Task<InventoryModel> GetLastInventoryByCafeteria(int cafeteriaId)
        {
            return await _getLastInventoryByCafeteriaQuery.Execute(cafeteriaId);
        }

        [HttpGet]
        public async Task<List<ResumeInventoryModel>> GetResume(int id)
        {
            return await _getResumeInventoryQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryModel model)
        {
            try
            {
                var result = await _cretaeInventoryCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName, Data = result });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InventoryModel updateModel, int id)
        {
            try
            {
                await _updateInventoryCommand.Execute(id, updateModel);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
