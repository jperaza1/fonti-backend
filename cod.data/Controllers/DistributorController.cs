using cod.application.Distributor.Commands.CreateDistributor;
using cod.application.Distributor.Commands.DisableDistributor;
using cod.application.Distributor.Commands.EnableDistributor;
using cod.application.Distributor.Commands.UpdateDistributor;
using cod.application.Distributor.Common;
using cod.application.Distributor.Queries.GetDistributor;
using cod.application.Distributor.Queries.GetDistributors;
using cod.application.Distributor.Queries.GetProductDistributorPrices;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class DistributorController : BaseController
    {
        private readonly IGetDistributorsQuery _getDistributorsQuery;
        private readonly IGetDistributorQuery _getDistributorQuery;
        private readonly IGetProductDistributorPricesQuery _getProductDistributorPricesQuery;
        private readonly ICreateDistributorCommand _createDistributorCommand;
        private readonly IUpdateDistributorCommand _updateDistributorCommand;
        private readonly IEnableDistributorCommand _enableDistributorCommand;
        private readonly IDisableDistributorCommand _disableDistributorCommand;

        public DistributorController(
            IGetDistributorsQuery getDistributorsQuery,
            IGetDistributorQuery getDistributorQuery,
            IGetProductDistributorPricesQuery getProductDistributorPricesQuery,
            ICreateDistributorCommand createDistributorCommand,
            IUpdateDistributorCommand updateDistributorCommand,
            IEnableDistributorCommand enableDistributorCommand,
            IDisableDistributorCommand disableDistributorCommand)
        {
            _getDistributorsQuery = getDistributorsQuery;
            _getDistributorQuery = getDistributorQuery;
            _getProductDistributorPricesQuery = getProductDistributorPricesQuery;
            _createDistributorCommand = createDistributorCommand;
            _updateDistributorCommand = updateDistributorCommand;
            _enableDistributorCommand = enableDistributorCommand;
            _disableDistributorCommand = disableDistributorCommand;
            this.ControllerName = "Distribuidores";
        }

        [HttpGet]
        public async Task<List<DistributorModel>> GetAll()
        {
            return await _getDistributorsQuery.Execute();
        }

        [HttpGet]
        public async Task<List<DistributorModel>> GetAllActive()
        {
            var response = await _getDistributorsQuery.Execute();
            return response.Where(f => f.IsActive).ToList();
        }

        [HttpGet]
        public async Task<DistributorModel> GetById(int id)
        {
            return await _getDistributorQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<ProductDistributorPricesModel>> GetProductDistributorPrices(int distributorId)
        {
            return await _getProductDistributorPricesQuery.Execute(distributorId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DistributorModel model)
        {
            try
            {
                await _createDistributorCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DistributorModel updateModel, int id)
        {
            try
            {
                await _updateDistributorCommand.Execute(updateModel, id);
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
                await _disableDistributorCommand.Execute(id);
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
                await _enableDistributorCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
