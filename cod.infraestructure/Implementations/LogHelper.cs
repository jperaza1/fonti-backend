using cod.application.Common;
using cod.application.Interfaces;
using cod.domain.Features;
using cod.domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.infraestructure.Implementations
{
    public class LogHelper : ILogHelper
    {
        private IRepository _repository;
        public void CompareField<T>(T oldValue, T newValue, List<CompareModel> compareResult, string Field)
        {
            if (!Equals(oldValue, newValue))
            {
                compareResult.Add(new CompareModel
                {
                    Display = string.Empty,
                    Field = Field,
                    OldValue = oldValue,
                    NewValue = newValue
                });
            }
        }

        public void Initialize(IRepository repository)
        {
            this._repository = repository;
        }

        public void InsertLogs(List<CompareModel> compareResult, string entity, int entityId)
        {
            foreach(var item in compareResult)
            {
                _repository.InsertAsync(new DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = entity,
                    EntityId = entityId,
                    Log = $"({item.Field}) VIEJO: {item.OldValue} - NUEVO: {item.NewValue}"
                });
            }
        }

    }
}
