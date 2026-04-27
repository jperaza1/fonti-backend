using cod.application.Cafeteria.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Queries.GetActiveCafeterias
{
    public interface IGetActiveCafeteriasQuery
    {
        Task<List<CafeteriaModel>> Execute();
    }
}
