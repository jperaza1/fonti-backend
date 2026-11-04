using cod.application.Features.Models;
using cod.application.PaymentSchedule.Commands.AddPaymentScheduleDetails;
using cod.application.PaymentSchedule.Commands.CreatePaymentSchedule;
using cod.application.PaymentSchedule.Commands.RegisterPaymentScheduleDetail;
using cod.application.PaymentSchedule.Common;
using cod.application.PaymentSchedule.Queries.GetPaymentSchedule;
using cod.application.PaymentSchedule.Queries.GetPaymentScheduleCarryForward;
using cod.application.PaymentSchedule.Queries.GetPaymentSchedules;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class PaymentScheduleController : BaseController
    {
        private readonly IGetPaymentSchedulesQuery _getPaymentSchedulesQuery;
        private readonly IGetPaymentScheduleQuery _getPaymentScheduleQuery;
        private readonly IGetPaymentScheduleCarryForwardQuery _getPaymentScheduleCarryForwardQuery;
        private readonly ICreatePaymentScheduleCommand _createPaymentScheduleCommand;
        private readonly IAddPaymentScheduleDetailsCommand _addPaymentScheduleDetailsCommand;
        private readonly IRegisterPaymentScheduleDetailCommand _registerPaymentScheduleDetailCommand;

        public PaymentScheduleController(
            IGetPaymentSchedulesQuery getPaymentSchedulesQuery,
            IGetPaymentScheduleQuery getPaymentScheduleQuery,
            IGetPaymentScheduleCarryForwardQuery getPaymentScheduleCarryForwardQuery,
            ICreatePaymentScheduleCommand createPaymentScheduleCommand,
            IAddPaymentScheduleDetailsCommand addPaymentScheduleDetailsCommand,
            IRegisterPaymentScheduleDetailCommand registerPaymentScheduleDetailCommand)
        {
            _getPaymentSchedulesQuery = getPaymentSchedulesQuery;
            _getPaymentScheduleQuery = getPaymentScheduleQuery;
            _getPaymentScheduleCarryForwardQuery = getPaymentScheduleCarryForwardQuery;
            _createPaymentScheduleCommand = createPaymentScheduleCommand;
            _addPaymentScheduleDetailsCommand = addPaymentScheduleDetailsCommand;
            _registerPaymentScheduleDetailCommand = registerPaymentScheduleDetailCommand;
        }

        [HttpGet]
        public async Task<List<PaymentScheduleModel>> GetAll()
        {
            return await _getPaymentSchedulesQuery.Execute();
        }

        [HttpGet]
        public async Task<PaymentScheduleModel> GetById(int id)
        {
            return await _getPaymentScheduleQuery.Execute(id);
        }

        [HttpGet]
        public async Task<PaymentScheduleCarryForwardModel> GetCarryForward()
        {
            return await _getPaymentScheduleCarryForwardQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentScheduleModel model)
        {
            try
            {
                var result = await _createPaymentScheduleCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName, Data = result });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDetails([FromBody] AddPaymentScheduleDetailsModel model)
        {
            try
            {
                var result = await _addPaymentScheduleDetailsCommand.Execute(model);
                return Ok(new ResponseModel<List<int>> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName, Data = result });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPayment([FromBody] RegisterPaymentScheduleDetailModel model)
        {
            try
            {
                await _registerPaymentScheduleDetailCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
