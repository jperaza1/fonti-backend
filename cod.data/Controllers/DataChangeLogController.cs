using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cod.application.DataChangeLog.Base;
using cod.application.DataChangeLog.Queries.GetAllLogsByEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class DataChangeLogController : BaseController
    {
        private readonly IGetAllLogsByEntityQuery _getAllLogsByEntityQuery;

        public DataChangeLogController(
            IGetAllLogsByEntityQuery getAllLogsByEntityQuery)
        {
            this._getAllLogsByEntityQuery = getAllLogsByEntityQuery;
        }

        [HttpGet]
        public async Task<List<DataChangeLogModel>> GetByEntity(string entity, int entityId)
        {
            return await _getAllLogsByEntityQuery.Execute(entity, entityId);
        }
    }
}
