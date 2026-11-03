using cod.application.Features.Models;
using cod.application.ProductRecipe.Commands.SaveProductRecipe;
using cod.application.ProductRecipe.Common;
using cod.application.ProductRecipe.Queries.GetProductRecipe;
using cod.application.ProductRecipe.Queries.GetProductRecipes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class ProductRecipeController : BaseController
    {
        private readonly IGetProductRecipesQuery _getProductRecipesQuery;
        private readonly IGetProductRecipeQuery _getProductRecipeQuery;
        private readonly ISaveProductRecipeCommand _saveProductRecipeCommand;

        public ProductRecipeController(
            IGetProductRecipesQuery getProductRecipesQuery,
            IGetProductRecipeQuery getProductRecipeQuery,
            ISaveProductRecipeCommand saveProductRecipeCommand)
        {
            _getProductRecipesQuery = getProductRecipesQuery;
            _getProductRecipeQuery = getProductRecipeQuery;
            _saveProductRecipeCommand = saveProductRecipeCommand;
        }

        [HttpGet]
        public async Task<List<ProductRecipeModel>> GetAll()
        {
            return await _getProductRecipesQuery.Execute();
        }

        [HttpGet]
        public async Task<ProductRecipeModel> GetByProduct(int productId)
        {
            return await _getProductRecipeQuery.Execute(productId);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ProductRecipeModel model)
        {
            try
            {
                await _saveProductRecipeCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
