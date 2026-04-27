using cod.application.Cafeteria.Commands;
using cod.application.Register.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Queries.GetRegistersByCafeteria
{
    public interface IGetRegistersByCafeteriaQuery
    {
        Task<List<RegisterModel>> Execute(int cafeteriaId, bool UseInInventory);
    }
}
