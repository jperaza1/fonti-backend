using cod.application.Warehouse.Common;
using cod.application.Warehouse.Queries.GetWarehouses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class WarehouseController : BaseController
    {
        private readonly IGetWarehousesQuery _getWarehousesQuery;

        public WarehouseController(IGetWarehousesQuery getWarehousesQuery)
        {
            _getWarehousesQuery = getWarehousesQuery;
        }

        [HttpGet]
        public async Task<List<WarehouseModel>> GetAll()
        {
            return await _getWarehousesQuery.Execute();
        }
    }
}
