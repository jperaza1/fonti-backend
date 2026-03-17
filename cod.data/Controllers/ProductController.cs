using cod.application.Cafeteria.Commands;
using cod.application.Features.Models;
using cod.application.Product.Commands.CreateProduct;
using cod.application.Product.Commands.DisableProduct;
using cod.application.Product.Commands.EnableProduct;
using cod.application.Product.Commands.UpdateProduct;
using cod.application.Product.Common;
using cod.application.Product.Queries.GetActiveProducts;
using cod.application.Product.Queries.GetProducts;
using cod.application.Product.Queries.GetProductsBySpecialService;
using cod.application.Product.Queries.GetProductsByVendor;
using cod.application.Product.Queries.GetProdut;
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
        private readonly IGetActiveProductsQuery _getActiveProductsQuery;
        private readonly IGetProductsByVendorQuery _getProductsByVendorQuery;
        private readonly IGetProductsBySpecialServiceQuery _getProductsBySpecialServiceQuery;
        private readonly ICreateProductCommand _createProductCommand;
        private readonly IUpdateProductCommand _updateProductCommand;
        private readonly IEnableProductCommand _enableProductCommand;
        private readonly IDisableProductCommand _disableProductCommand;

        public ProductController(
            IGetProductsQuery getProductsQuery,
            IGetProdutQuery getprodutQuery,
            IGetActiveProductsQuery getActiveProductsQuery,
            IGetProductsByVendorQuery getProductsByVendorQuery,
            IGetProductsBySpecialServiceQuery getProductsBySpecialServiceQuery,
            ICreateProductCommand createProductCommand,
            IUpdateProductCommand updateProductCommand,
            IEnableProductCommand enableProductCommand,
            IDisableProductCommand disableProductCommand)
        {
            _getProductsQuery = getProductsQuery;
            _getprodutQuery = getprodutQuery;
            _getActiveProductsQuery = getActiveProductsQuery;
            _getProductsByVendorQuery = getProductsByVendorQuery;
            _getProductsBySpecialServiceQuery = getProductsBySpecialServiceQuery;
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
        public async Task<List<ProductModel>> GetAllActive()
        {
            return await _getActiveProductsQuery.Execute();
        }

        [HttpGet]
        public async Task<List<ProductModel>> GetBySpecialService()
        {
            return await _getProductsBySpecialServiceQuery.Execute();
        }

        [HttpGet]
        public async Task<List<ProductModel>> GetByVendor(int vendorId)
        {
            return await _getProductsByVendorQuery.Execute(vendorId);
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
