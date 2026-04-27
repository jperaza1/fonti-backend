using cod.application.Sale.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSales
{
    public interface IGetSalesQuery
    {
        Task<List<SaleModel>> Execute(FilterSaleDto filterSale);
    }
}
