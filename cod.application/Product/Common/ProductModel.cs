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
        public ProductType ProductType { get; set; }
        public string ProductTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
