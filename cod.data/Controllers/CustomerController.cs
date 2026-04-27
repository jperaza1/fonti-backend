using cod.application.Customer.Commands.CreateCustomer;
using cod.application.Customer.Commands.DisableCustomer;
using cod.application.Customer.Commands.EnableCustomer;
using cod.application.Customer.Commands.UpdateCustomer;
using cod.application.Customer.Common;
using cod.application.Customer.Queries.GetActiveCustomers;
using cod.application.Customer.Queries.GetCustomer;
using cod.application.Customer.Queries.GetCustomers;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly IGetCustomerQuery _getCustomerQuery;
        private readonly IGetCustomersQuery _getCustomersQuery;
        private readonly IGetActiveCustomersQuery _getActiveCustomersQuery;
        private readonly ICreateCustomerCommand _createCustomerCommand;
        private readonly IUpdateCustomerCommand _updateCustomerCommand;
        private readonly IEnableCustomerCommand _enableCustomerCommand;
        private readonly IDisableCustomerCommand _disableCustomerCommand;

        public CustomerController(
            IGetCustomerQuery getCustomerQuery,
            IGetCustomersQuery getCustomersQuery,
            IGetActiveCustomersQuery getActiveCustomersQuery,
            ICreateCustomerCommand createCustomerCommand,
            IUpdateCustomerCommand updateCustomerCommand,
            IEnableCustomerCommand enableCustomerCommand,
            IDisableCustomerCommand disableCustomerCommand)
        {
            _getCustomerQuery = getCustomerQuery;
            _getCustomersQuery = getCustomersQuery;
            _getActiveCustomersQuery = getActiveCustomersQuery;
            _createCustomerCommand = createCustomerCommand;
            _updateCustomerCommand = updateCustomerCommand;
            _enableCustomerCommand = enableCustomerCommand;
            _disableCustomerCommand = disableCustomerCommand;
        }

        [HttpGet]
        public async Task<List<CustomerModel>> GetAll()
        {
            return await _getCustomersQuery.Execute();
        }

        [HttpGet]
        public async Task<CustomerModel> GetById(int id)
        {
            return await _getCustomerQuery.Execute(id);
        }

        [HttpGet]
        public async Task<List<CustomerModel>> GetAllActive()
        {
            return await _getActiveCustomersQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerModel model)
        {
            try
            {
                await _createCustomerCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerModel updateModel, int id)
        {
            try
            {
                await _updateCustomerCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Enable(int id)
        {
            try
            {
                await _enableCustomerCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                await _disableCustomerCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
