using cod.application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface ILogHelper
    {
        void CompareField<T>(T oldValue, T newValue, List<CompareModel> compareResult, string Field);
        void Initialize(IRepository repository);
        void InsertLogs(List<CompareModel> compareResult, string entity, int entityId);
    }
}
