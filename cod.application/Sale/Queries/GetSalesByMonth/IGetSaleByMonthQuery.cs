using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSalesByMonth
{
    public interface IGetSaleByMonthQuery
    {
        Task<List<GetSalesByMonthModel>> Execute();
    }
}
