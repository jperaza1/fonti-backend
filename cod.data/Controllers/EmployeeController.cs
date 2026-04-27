using cod.application.Employee.Commands.CreateEmployee;
using cod.application.Employee.Commands.RequestVacation;
using cod.application.Employee.Commands.UpdateEmployee;
using cod.application.Employee.Common;
using cod.application.Employee.Queries.GetEmployee;
using cod.application.Employee.Queries.GetEmployees;
using cod.application.Employee.Queries.GetVacationRequestPdf;
using cod.application.Features.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IGetEmployeesQuery _getEmployeesQuery;
        private readonly IGetEmployeeQuery _getEmployeeQuery;
        private readonly ICreateEmployeeCommand _createEmployeeCommand;
        private readonly IUpdateEmployeeCommand _updateEmployeeCommand;
        private readonly IRequestVacationCommand _requestVacationCommand;
        private readonly IGetVacationRequestPdfQuery _getVacationRequestPdfQuery;
        public EmployeeController(
            IGetEmployeesQuery getEmployeesQuery,
            IGetEmployeeQuery getEmployeeQuery,
            IGetVacationRequestPdfQuery getVacationRequestPdfQuery,
            ICreateEmployeeCommand createEmployeeCommand,
            IUpdateEmployeeCommand updateEmployeeCommand,
            IRequestVacationCommand requestVacationCommand)
        {
            _createEmployeeCommand = createEmployeeCommand;
            _updateEmployeeCommand = updateEmployeeCommand;
            _getEmployeesQuery = getEmployeesQuery;
            _getEmployeeQuery = getEmployeeQuery;
            _requestVacationCommand = requestVacationCommand;
            _getVacationRequestPdfQuery = getVacationRequestPdfQuery;
        }

        [HttpPost]
        public async Task<List<EmployeeModel>> GetAll()
        {
            return await _getEmployeesQuery.Execute();
        }

        [HttpGet]
        public async Task<EmployeeModel> GetById(int id)
        {
            return await _getEmployeeQuery.Execute(id);
        }

        [HttpGet]
        public async Task<string> GetRequestVacatioFile(int id)
        {
            return await _getVacationRequestPdfQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeModel model)
        {
            try
            {
                await _createEmployeeCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RequestVacation([FromBody] RequestVacationModel model)
        {
            try
            {
                await _requestVacationCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmployeeModel updateModel, int id)
        {
            try
            {
                await _updateEmployeeCommand.Execute(updateModel, id);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
