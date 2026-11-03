
using cod.application.Bank.Commands.CreateBank;
using cod.application.Bank.Commands.DisableBank;
using cod.application.Bank.Commands.EnableBank;
using cod.application.Bank.Commands.UpdateBank;
using cod.application.Bank.Common;
using cod.application.Bank.Queries.GetActiveBanks;
using cod.application.Bank.Queries.GetBank;
using cod.application.Bank.Queries.GetBanks;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class BankController : BaseController
    {
        private readonly IGetBanksQuery _getBanksQuery;
        private readonly IGetActiveBanksQuery _getActiveBanksQuery;
        private readonly IGetBankQuery _getBankQuery;
        private readonly ICreateBankCommand _createBankCommand;
        private readonly IUpdateBankCommand _updateBankCommand;
        private readonly IEnableBankCommand _enableBankCommand;
        private readonly IDisableBankCommand _disableBankCommand;

        public BankController(
            IGetBanksQuery getBanksQuery,
            IGetActiveBanksQuery getActiveBanksQuery,
            IGetBankQuery getBankQuery,
            ICreateBankCommand createBankCommand,
            IUpdateBankCommand updateBankCommand,
            IEnableBankCommand enableBankCommand,
            IDisableBankCommand disableBankCommand)
        {
            _getBanksQuery = getBanksQuery;
            _getActiveBanksQuery = getActiveBanksQuery;
            _getBankQuery = getBankQuery;
            _createBankCommand = createBankCommand;
            _updateBankCommand = updateBankCommand;
            _enableBankCommand = enableBankCommand;
            _disableBankCommand = disableBankCommand;
        }

        [HttpGet]
        public async Task<List<BankModel>> GetAll()
        {
            return await _getBanksQuery.Execute();
        }

        [HttpGet]
        public async Task<List<BankModel>> GetAllActive()
        {
            return await _getActiveBanksQuery.Execute();
        }

        [HttpGet]
        public async Task<BankModel> GetById(int id)
        {
            return await _getBankQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BankModel model)
        {
            try
            {
                await _createBankCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BankModel updateModel, int id)
        {
            try
            {
                await _updateBankCommand.Execute(updateModel, id);
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
                await _disableBankCommand.Execute(id);
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
                await _enableBankCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
