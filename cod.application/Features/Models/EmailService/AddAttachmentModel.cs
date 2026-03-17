using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Features.Models.EmailService
{
    public class AddAttachmentModel
    {
        public string AttachmentName { get; set; }
        public Stream Attachment { get; set; }
        public ContentType ContentType { get; set; }
    }
}
