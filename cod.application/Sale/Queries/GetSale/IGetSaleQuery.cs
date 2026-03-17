using cod.application.Sale.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSale
{
    public interface IGetSaleQuery
    {
        Task<SaleModel> Execute(int id);
    }
}
