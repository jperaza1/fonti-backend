using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Main
{
    public class AttachmentModel
    {
        public byte[] FileBytes { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
    }
}
