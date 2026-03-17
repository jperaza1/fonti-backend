using cod.application.Dish.Common;
using cod.application.Features.Models;
using cod.application.SideItem.Commands.CreateSideItem;
using cod.application.SideItem.Commands.DisableSideItem;
using cod.application.SideItem.Commands.EnableSideItem;
using cod.application.SideItem.Commands.UpdateSideItem;
using cod.application.SideItem.Common;
using cod.application.SideItem.Queries.GetSideItem;
using cod.application.SideItem.Queries.GetSideItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class SideItemController : BaseController
    {
        private readonly IGetSideItemsQuery _getSideItemsQuery;
        private readonly IGetSideItemQuery _getSideItemQuery;
        private readonly ICreateSideItemCommand _createSideItemCommand;
        private readonly IUpdateSideItemCommand _updateSideItemCommand;
        private readonly IDisableSideItemCommand _disableSideItemCommand;
        private readonly IEnableSideItemCommand _enableSideItemCommand;
        public SideItemController(
            IGetSideItemsQuery getSideItemsQuery, 
            IGetSideItemQuery getSideItemQuery,
            ICreateSideItemCommand createSideItemCommand,
            IUpdateSideItemCommand updateSideItemCommand,
            IDisableSideItemCommand disableSideItemCommand,
            IEnableSideItemCommand enableSideItemCommand)
        {
            _getSideItemsQuery = getSideItemsQuery;
            _getSideItemQuery = getSideItemQuery;
            _createSideItemCommand = createSideItemCommand;
            _updateSideItemCommand = updateSideItemCommand;
            _disableSideItemCommand = disableSideItemCommand;
            _enableSideItemCommand = enableSideItemCommand;
        }

        [HttpGet]
        public async Task<List<SideItemModel>> GetAll()
        {
            return await _getSideItemsQuery.Execute();
        }

        [HttpGet]
        public async Task<SideItemModel> GetById(int id)
        {
            return await _getSideItemQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SideItemModel model)
        {
            try
            {
                await _createSideItemCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SideItemModel updateModel, int id)
        {
            try
            {
                await _updateSideItemCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                await _disableSideItemCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Enable(int id)
        {
            try
            {
                await _enableSideItemCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
