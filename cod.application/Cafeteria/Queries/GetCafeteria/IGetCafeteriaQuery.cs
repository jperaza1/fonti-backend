using cod.application.Cafeteria.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Queries.GetCafeteria
{
    public interface IGetCafeteriaQuery
    {
        Task<CafeteriaModel> Execute(int id);
    }
}
