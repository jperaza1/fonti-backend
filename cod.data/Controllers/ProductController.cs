using cod.application.Cafeteria.Commands;
using cod.application.Features.Models;
using cod.application.Product.Commands.CreateProduct;
using cod.application.Product.Commands.DisableProduct;
using cod.application.Product.Commands.EnableProduct;
using cod.application.Product.Commands.UpdateProduct;
using cod.application.Product.Common;
using cod.application.Product.Queries.GetActiveProductsByType;
using cod.application.Product.Queries.GetProducts;
using cod.application.Product.Queries.GetProdut;
using cod.domain.Features.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IGetProductsQuery _getProductsQuery;
        private readonly IGetProdutQuery _getprodutQuery;
        private readonly IGetActiveProductsByTypeQuery _getActiveProductsByTypeQuery;
        private readonly ICreateProductCommand _createProductCommand;
        private readonly IUpdateProductCommand _updateProductCommand;
        private readonly IEnableProductCommand _enableProductCommand;
        private readonly IDisableProductCommand _disableProductCommand;

        public ProductController(
            IGetProductsQuery getProductsQuery,
            IGetProdutQuery getprodutQuery,
            IGetActiveProductsByTypeQuery getActiveProductsByTypeQuery,
            ICreateProductCommand createProductCommand,
            IUpdateProductCommand updateProductCommand,
            IEnableProductCommand enableProductCommand,
            IDisableProductCommand disableProductCommand)
        {
            _getProductsQuery = getProductsQuery;
            _getprodutQuery = getprodutQuery;
            _getActiveProductsByTypeQuery = getActiveProductsByTypeQuery;
            _createProductCommand = createProductCommand;
            _updateProductCommand = updateProductCommand;
            _enableProductCommand = enableProductCommand;
            _disableProductCommand = disableProductCommand;
        }

        [HttpGet]
        public async Task<List<ProductModel>> GetAll()
        {
            return await _getProductsQuery.Execute();
        }

        [HttpGet]
        public async Task<ProductModel> GetById(int id)
        {
            return await _getprodutQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<ProductModel>> GetAllActive(ProductType productType)
        {
            return await _getActiveProductsByTypeQuery.Execute(productType);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductModel model)
        {
            try
            {
                await _createProductCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductModel updateModel, int id)
        {
            try
            {
                await _updateProductCommand.Execute(updateModel, id);
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
                await _disableProductCommand.Execute(id);
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
                await _enableProductCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
