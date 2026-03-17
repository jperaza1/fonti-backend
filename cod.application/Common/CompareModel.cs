using System;
using System.Collections.Generic;
using System.Text;

namespace cod.application.Common
{
    public class CompareModel
    {
        public string Field { get; set; }
        public string Display { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}
