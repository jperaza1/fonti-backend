using cod.application.Main.Attributes;
using cod.domain.Features.Enums;
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
        public string Name { get; set; }
        public bool Tax { get; set; }
        public ProductType ProductType { get; set; }
        public string ProductTypeName { get; set; }
        public decimal? Cost { get; set; }
        public int? UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public bool IsActive { get; set; }
        public List<ProductVendorModel> ProductVendors { get; set; }
    }

    public class ProductVendorModel
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int ProductId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public int Unit { get; set; }
        public decimal Cost { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Discount { get; set; }
        public bool IsActive { get; set; }
    }

    public class VendorProductModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxRate { get; set; }
        public int Unit { get; set; }
    }
}
