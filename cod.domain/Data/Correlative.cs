using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Correlatives", Schema = "data")]
    public class Correlative : Entity
    {
        public string Cai { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ulong Current { get; set; }
        public string EmissionPoint { get; set; }
        public string Establishment { get; set; }
        public string DocumentType { get; set; }
    }
}
