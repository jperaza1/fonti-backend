using cod.application.BankAccount.Commands.CreateBankAccount;
using cod.application.BankAccount.Commands.DisableBankAccount;
using cod.application.BankAccount.Commands.EnableBankAccount;
using cod.application.BankAccount.Commands.UpdateBankAccount;
using cod.application.BankAccount.Common;
using cod.application.BankAccount.Queries.GetActiveBankAccounts;
using cod.application.BankAccount.Queries.GetBankAccount;
using cod.application.BankAccount.Queries.GetBankAccounts;
using cod.application.Customer.Common;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class BankAccountController : BaseController
    {
        private readonly IGetBankAccountsQuery _getBankAccountsQuery;
        private readonly IGetActiveBankAccountsQuery _getActiveBankAccountsQuery;
        private readonly IGetBankAccountQuery _getBankAccountQuery;
        private readonly ICreateBankAccountCommand _createBankAccountCommand;
        private readonly IUpdateBankAccountCommand _updateBankAccountCommand;
        private readonly IEnableBankAccountCommand _enableBankAccountCommand;
        private readonly IDisableBankAccountCommand _disableBankAccountCommand;

        public BankAccountController(
            IGetBankAccountsQuery getBankAccountsQuery,
            IGetActiveBankAccountsQuery getActiveBankAccountsQuery,
            IGetBankAccountQuery getBankAccountQuery,
            ICreateBankAccountCommand createBankAccountCommand,
            IUpdateBankAccountCommand updateBankAccountCommand,
            IEnableBankAccountCommand enableBankAccountCommand,
            IDisableBankAccountCommand disableBankAccountCommand)
        {
            _getBankAccountsQuery = getBankAccountsQuery;
            _getActiveBankAccountsQuery = getActiveBankAccountsQuery;
            _getBankAccountQuery = getBankAccountQuery;
            _createBankAccountCommand = createBankAccountCommand;
            _updateBankAccountCommand = updateBankAccountCommand;
            _enableBankAccountCommand = enableBankAccountCommand;
            _disableBankAccountCommand = disableBankAccountCommand;
        }

        [HttpGet]
        public async Task<List<BankAccountModel>> GetAll()
        {
            return await _getBankAccountsQuery.Execute();
        }

        [HttpGet]
        public async Task<List<BankAccountModel>> GetAllActive()
        {
            return await _getActiveBankAccountsQuery.Execute();
        }

        [HttpGet]
        public async Task<BankAccountModel> GetById(int id)
        {
            return await _getBankAccountQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BankAccountModel model)
        {
            try
            {
                await _createBankAccountCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BankAccountModel updateModel, int id)
        {
            try
            {
                await _updateBankAccountCommand.Execute(updateModel, id);
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
                await _disableBankAccountCommand.Execute(id);
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
                await _enableBankAccountCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
