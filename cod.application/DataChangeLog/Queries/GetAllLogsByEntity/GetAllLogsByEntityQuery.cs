using Microsoft.EntityFrameworkCore;
using cod.application.DataChangeLog.Base;
using cod.application.Features.Base;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DataChangeLog.Queries.GetAllLogsByEntity
{
    public class GetAllLogsByEntityQuery : BaseQuery, IGetAllLogsByEntityQuery
    {
        public GetAllLogsByEntityQuery(IRepository repository) : base(repository) { }

        public Task<List<DataChangeLogModel>> Execute(string entity, int entityId)
        {
            return _repository.AllAsync<domain.General.DataChangeLog>().Where(f => f.Entity == entity && f.EntityId == entityId)
                .Select(log => new DataChangeLogModel
                {
                    CreatorUser = log.CreatorUser,
                    Date = log.Date,
                    Entity = log.Entity,
                    EntityId = log.EntityId,
                    Id = log.Id,
                    Log = log.Log
                }).ToListAsync();
        }
    }
}
