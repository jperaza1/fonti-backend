using cod.application.DispatchControl.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Queries.GetAllDispatchControl
{
    public interface IGetAllDispatchControlQuery
    {
        Task<List<DispatchControlModel>> Execute(DispatchControlFilterModel filterModel);
    }
}
