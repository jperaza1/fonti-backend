using cod.application.Main.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Common
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        [IndexColumnAttribute(1)]
        public string Vendor { get; set; }
        [IndexColumnAttribute(2)]
        public string Name { get; set; }
        public int MeasureId { get; set; }
        [IndexColumnAttribute(3)]
        public string Measure { get; set; }
        [IndexColumnAttribute(4)]
        public int Unit { get; set; }
        [IndexColumnAttribute(5)]
        public double Cost { get; set; }
        public double UnitCost { get; set; }
        [IndexColumnAttribute(6)]
        public double Tax { get; set; }
        [IndexColumnAttribute(7)]
        public double Discount { get; set; }
        public bool IsActive { get; set; }
        public bool SpecialServiceUse { get; set; }
    }
}
