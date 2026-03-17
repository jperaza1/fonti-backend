using cod.application.Register.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Queries.GetRegister
{
    public interface IGetRegisterQuery
    {
        Task<RegisterModel> Execute(int id);
    }
}
