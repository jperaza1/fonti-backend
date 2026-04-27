using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.SalePoint
{
    [Table("SideItems", Schema = "salepoint")]
    public class SideItem : Entity
    {
        public string Name { get; set; }
    }
}
