using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Customers", Schema = "data")]
    public class Customer : Entity
    {
        public string Name { get; set; }
        public string Rtn { get; set; }
    }
}
