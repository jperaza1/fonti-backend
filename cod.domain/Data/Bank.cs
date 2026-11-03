using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Banks", Schema = "data")]
    public class Bank : Entity
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Code { get; set; }
    }
}
