using cod.application.Sale.Queries.GetSalesByMonth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetDailySalesByCafeteria
{
    public interface IGetDailySalesByCafeteriaQuery
    {
        Task<List<GetSalesByMonthModel>> Execute();
    }
}
