using cod.application.Expense.Commands.CreateExpense;
using cod.application.Expense.Commands.DisableExpense;
using cod.application.Expense.Commands.EnableExpense;
using cod.application.Expense.Commands.UpdateExpense;
using cod.application.Expense.Common;
using cod.application.Expense.Queries.GetExpense;
using cod.application.Expense.Queries.GetExpenses;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class ExpenseController : BaseController
    {
        private readonly IGetExpensesQuery _getExpensesQuery;
        private readonly IGetExpenseQuery _getExpenseQuery;
        private readonly ICreateExpenseCommand _createExpenseCommand;
        private readonly IUpdateExpenseCommand _updateExpenseCommand;
        private readonly IEnableExpenseCommand _enableExpenseCommand;
        private readonly IDisableExpenseCommand _disableExpenseCommand;

        public ExpenseController(
            IGetExpensesQuery getExpensesQuery,
            IGetExpenseQuery getExpenseQuery,
            ICreateExpenseCommand createExpenseCommand,
            IUpdateExpenseCommand updateExpenseCommand,
            IEnableExpenseCommand enableExpenseCommand,
            IDisableExpenseCommand disableExpenseCommand)
        {
            _getExpensesQuery = getExpensesQuery;
            _getExpenseQuery = getExpenseQuery;
            _createExpenseCommand = createExpenseCommand;
            _updateExpenseCommand = updateExpenseCommand;
            _enableExpenseCommand = enableExpenseCommand;
            _disableExpenseCommand = disableExpenseCommand;
        }

        [HttpPost]
        public async Task<List<ExpenseModel>> GetAll([FromBody] ExpenseFilterModel filter)
        {
            return await _getExpensesQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<ExpenseModel> GetById(int id)
        {
            return await _getExpenseQuery.Execute(id);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ExpenseModel model, IFormFile invoiceFile)
        {
            try
            {
                await _createExpenseCommand.Execute(model, invoiceFile);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ExpenseModel updateModel, int id)
        {
            try
            {
                await _updateExpenseCommand.Execute(updateModel, id);
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
                await _disableExpenseCommand.Execute(id);
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
                await _enableExpenseCommand.Execute(id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
