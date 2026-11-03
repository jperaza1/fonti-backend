using cod.application.Kardex.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Kardex.Queries.GetProductStocks
{
    public interface IGetProductStocksQuery
    {
        Task<List<ProductStockModel>> Execute();
    }
}
