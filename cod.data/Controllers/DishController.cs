using cod.application.Dish.Commands.AddDishCafeteria;
using cod.application.Dish.Commands.CreateDish;
using cod.application.Dish.Commands.DisableDish;
using cod.application.Dish.Commands.EnableDish;
using cod.application.Dish.Commands.RemoveDishCafeteria;
using cod.application.Dish.Commands.UpdateDish;
using cod.application.Dish.Commands.UpdateDishCafeteria;
using cod.application.Dish.Common;
using cod.application.Dish.Queries.GetDish;
using cod.application.Dish.Queries.GetDishes;
using cod.application.Features.Models;
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
    public class DishController : BaseController
    {
        private readonly IGetDishesQuery _getDishesQuery;
        private readonly IGetDishQuery _getDishQuery;
        private readonly ICreateDishCommand _createDishCommand;
        private readonly IUpdateDishCommand _updateDishCommand;
        private readonly IDisableDishCommand _disableDishCommand;
        private readonly IEnableDishCommand _enableDishCommand;
        private readonly IAddDishCafeteriaCommand _addDishCafeteriaCommand;
        private readonly IUpdateDishCafeteriaCommand _updateDishCafeteriaCommand;
        private readonly IRemoveDishCafeteriaCommand _removeDishCafeteriaCommand;
        public DishController(
            IGetDishesQuery getDishesQuery,
            IGetDishQuery getDishQuery,
            ICreateDishCommand createDishCommand,
            IUpdateDishCommand updateDishCommand,
            IDisableDishCommand disableDishCommand,
            IEnableDishCommand enableDishCommand,
            IAddDishCafeteriaCommand addDishCafeteriaCommand,
            IUpdateDishCafeteriaCommand updateDishCafeteriaCommand,
            IRemoveDishCafeteriaCommand removeDishCafeteriaCommand)
        {
            _getDishesQuery = getDishesQuery;
            _getDishQuery = getDishQuery;
            _createDishCommand = createDishCommand;
            _updateDishCommand = updateDishCommand;
            _disableDishCommand = disableDishCommand;
            _enableDishCommand = enableDishCommand;
            _addDishCafeteriaCommand = addDishCafeteriaCommand;
            _updateDishCafeteriaCommand = updateDishCafeteriaCommand;
            _removeDishCafeteriaCommand = removeDishCafeteriaCommand;
        }

        [HttpGet]
        public async Task<List<DishModel>> GetAll()
        {
            return await _getDishesQuery.Execute();
        }

        [HttpGet]
        public async Task<DishModel> GetById(int id)
        {
            return await _getDishQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DishModel model)
        {
            try
            {
                await _createDishCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DishModel updateModel, int id)
        {
            try
            {
                await _updateDishCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDishCafeteria([FromBody] DishCafeteriaModel model)
        {
            try
            {
                await _addDishCafeteriaCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateDishCafeteria([FromBody] DishCafeteriaModel updateModel, int id)
        {
            try
            {
                await _updateDishCafeteriaCommand.Execute(updateModel, id);
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
                await _disableDishCommand.Execute(id);
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
                await _enableDishCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveDishCafeteria(int id)
        {
            try
            {
                await _removeDishCafeteriaCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
