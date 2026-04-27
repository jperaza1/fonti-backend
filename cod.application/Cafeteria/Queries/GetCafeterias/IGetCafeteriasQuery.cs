using cod.application.Cafeteria.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Queries.GetCafeterias
{
    public interface IGetCafeteriasQuery
    {
        Task<List<CafeteriaModel>> Execute();
    }
}
