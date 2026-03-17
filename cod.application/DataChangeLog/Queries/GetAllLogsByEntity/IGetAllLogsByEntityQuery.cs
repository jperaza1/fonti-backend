using cod.application.DataChangeLog.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DataChangeLog.Queries.GetAllLogsByEntity
{
    public interface IGetAllLogsByEntityQuery
    {
        Task<List<DataChangeLogModel>> Execute(string entity, int entityId);
    }
}
