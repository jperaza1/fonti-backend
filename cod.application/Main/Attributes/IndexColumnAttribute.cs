using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Main.Attributes
{
    public class IndexColumnAttribute : Attribute
    {
        public int Index { get; set; }
        public IndexColumnAttribute(int index)
        {
            this.Index = index;
        }
    }
}
