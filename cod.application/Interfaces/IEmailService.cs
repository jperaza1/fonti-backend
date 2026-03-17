using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cod.application.Features.Models.EmailService;

namespace cod.application.Interfaces
{
    public interface IEmailService
    {
        void SetReplyTo(List<string> replyTo);
        void AddAttachment(AddAttachmentModel attachment);
        void ClearAttachment();
        void AddAttachments(List<AddAttachmentModel> attachments);
        Task SendAsync(string subject, string body, List<string> contacts, List<string> bccs, List<string> ccs);
        Task SendAsync(string subject, string body, List<string> contacts);
    }
}
