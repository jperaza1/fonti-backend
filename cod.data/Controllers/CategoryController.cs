using cod.application.Category.Commands.CreateCategory;
using cod.application.Category.Commands.DisableCategory;
using cod.application.Category.Commands.EnableCategory;
using cod.application.Category.Commands.UpdateCategory;
using cod.application.Category.Common;
using cod.application.Category.Queries.GetActiveCategories;
using cod.application.Category.Queries.GetCategories;
using cod.application.Category.Queries.GetCategory;
using cod.application.Category.Queries.GetSubCategories;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IGetCategoriesQuery _getCategoriesQuery;
        private readonly IGetActiveCategoriesQuery _getActiveCategoriesQuery;
        private readonly IGetCategoryQuery _getCategoryQuery;
        private readonly IGetSubCategoriesQuery _getSubCategoriesQuery;
        private readonly ICreateCategoryCommand _createCategoryCommand;
        private readonly IUpdateCategoryCommand _updateCategoryCommand;
        private readonly IEnableCategoryCommand _enableCategoryCommand;
        private readonly IDisableCategoryCommand _disableCategoryCommand;

        public CategoryController(
            IGetCategoriesQuery getCategoriesQuery,
            IGetActiveCategoriesQuery getActiveCategoriesQuery,
            IGetCategoryQuery getCategoryQuery,
            IGetSubCategoriesQuery getSubCategoriesQuery,
            ICreateCategoryCommand createCategoryCommand,
            IUpdateCategoryCommand updateCategoryCommand,
            IEnableCategoryCommand enableCategoryCommand,
            IDisableCategoryCommand disableCategoryCommand)
        {
            _getCategoriesQuery = getCategoriesQuery;
            _getActiveCategoriesQuery = getActiveCategoriesQuery;
            _getCategoryQuery = getCategoryQuery;
            _getSubCategoriesQuery = getSubCategoriesQuery;
            _createCategoryCommand = createCategoryCommand;
            _updateCategoryCommand = updateCategoryCommand;
            _enableCategoryCommand = enableCategoryCommand;
            _disableCategoryCommand = disableCategoryCommand;
        }

        [HttpGet]
        public async Task<List<CategoryModel>> GetAll()
        {
            return await _getCategoriesQuery.Execute();
        }

        [HttpGet]
        public async Task<List<CategoryModel>> GetAllActive()
        {
            return await _getActiveCategoriesQuery.Execute();
        }

        [HttpGet]
        public async Task<CategoryModel> GetById(int id)
        {
            return await _getCategoryQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<SubCategoryModel>> GetSubCategories(int categoryId)
        {
            return await _getSubCategoriesQuery.Execute(categoryId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryModel model)
        {
            try
            {
                await _createCategoryCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryModel updateModel, int id)
        {
            try
            {
                await _updateCategoryCommand.Execute(updateModel, id);
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
                await _disableCategoryCommand.Execute(id);
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
                await _enableCategoryCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
