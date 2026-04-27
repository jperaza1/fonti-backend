using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Commands
{
    public class CafeteriaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int PurchaseOrderCorelative { get; set; }
        public int SpecialServicesCorelative { get; set; }
        public int OfficeRequisitionCorelative { get; set; }
        public int InventoryCorelative { get; set; }
        public bool IsActive { get; set; }
        public List<CustomerCafeteriaModel> Customers { get; set; }
    }

    public class CustomerCafeteriaModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public int CafeteriaId { get; set; }
    }
}
