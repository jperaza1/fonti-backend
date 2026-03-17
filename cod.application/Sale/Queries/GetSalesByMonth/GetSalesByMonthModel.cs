using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSalesByMonth
{
    public class GetSalesByMonthModel
    {
        public double Total { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Cafeteria { get; set; }
    }
}
