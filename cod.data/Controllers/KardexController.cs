using cod.application.Features.Models;
using cod.application.Kardex.Commands.CreateKardexEntry;
using cod.application.Kardex.Common;
using cod.application.Kardex.Queries.GetKardex;
using cod.application.Kardex.Queries.GetProductStocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class KardexController : BaseController
    {
        private readonly IGetKardexQuery _getKardexQuery;
        private readonly IGetProductStocksQuery _getProductStocksQuery;
        private readonly ICreateKardexEntryCommand _createKardexEntryCommand;

        public KardexController(
            IGetKardexQuery getKardexQuery,
            IGetProductStocksQuery getProductStocksQuery,
            ICreateKardexEntryCommand createKardexEntryCommand)
        {
            _getKardexQuery = getKardexQuery;
            _getProductStocksQuery = getProductStocksQuery;
            _createKardexEntryCommand = createKardexEntryCommand;
        }

        [HttpPost]
        public async Task<List<KardexModel>> GetAll([FromBody] KardexFilterModel filter)
        {
            return await _getKardexQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<List<ProductStockModel>> GetStock()
        {
            return await _getProductStocksQuery.Execute();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExit([FromBody] KardexEntryModel model)
        {
            try
            {
                model.IsExit = true;
                await _createKardexEntryCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntry([FromBody] KardexEntryModel model)
        {
            try
            {
                await _createKardexEntryCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
