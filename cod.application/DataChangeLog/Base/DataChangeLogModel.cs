using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DataChangeLog.Base
{
    public class DataChangeLogModel
    {
        public int EntityId { get; set; }
        public string Entity { get; set; }
        public DateTime Date { get; set; }
        public string Log { get; set; }
        public int Id { get; set; }
        public string CreatorUser { get; set; }
    }
}
