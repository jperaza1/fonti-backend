using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Features.Models.EmailService;
using cod.application.Interfaces;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.infraestructure.Implementations
{
    public class EmailService : IEmailService
    {

        private readonly IConfigurationService _configurationService;
        private string smtpHost, smtpUser, smtpPass;
        private int smtpPort;
        private List<Attachment> attachments;
        private List<string> replyTo;

        public EmailService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            LoadSettings();
            this.attachments = new List<Attachment>();
            this.replyTo = new List<string>();
        }

        private void LoadSettings()
        {
            var task = Task.Run(async () => await LoadSettingsAsync());
            task.Wait();
        }

        private async Task LoadSettingsAsync()
        {
            this.smtpHost = await this._configurationService.Get<string>("EMAILSERVICE_HOST");
            this.smtpUser = await this._configurationService.Get<string>("EMAILSERVICE_USER");
            this.smtpPass = await this._configurationService.Get<string>("EMAILSERVICE_PASSWORD");
            this.smtpPort = await this._configurationService.Get<int>("EMAILSERVICE_PORT");
        }

        public void AddAttachment(AddAttachmentModel attachment)
        {
            var attachmentRow = new Attachment(attachment.Attachment, attachment.AttachmentName, attachment.ContentType.MediaType);
            this.attachments.Add(attachmentRow);
        }

        public void AddAttachments(List<AddAttachmentModel> attachments)
        {
            foreach (var attachment in attachments) AddAttachment(attachment);
        }

        public async Task SendAsync(string subject, string body, List<string> contacts, List<string> bccs, List<string> ccs)
        {
            try
            {
                var client = new SmtpClient(smtpHost, smtpPort)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass)
                }; ;

                var message = new MailMessage(this.smtpUser, contacts.Aggregate((a, b) => $"{a}, {b}"), subject, body);

                message.IsBodyHtml = true;
                if (bccs.Any())
                    message.Bcc.Add(bccs.Aggregate((a, b) => $"{a}, {b}"));
                if (ccs.Any())
                    message.CC.Add(ccs.Aggregate((a, b) => $"{a}, {b}"));
                if (replyTo.Any())
                    message.ReplyToList.Add(this.replyTo.Aggregate((a, b) => $"{a}, {b}"));

                foreach (var attachment in attachments) message.Attachments.Add(attachment);

                await client.SendMailAsync(message);

                this.attachments = new List<Attachment>();
                this.replyTo = new List<string>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error sending email.", "Email Service");
            }
        }

        public Task SendAsync(string subject, string body, List<string> contacts)
        {
            return SendAsync(subject, body, contacts, new List<string>(), new List<string>());
        }


        public void SetReplyTo(List<string> replyTo)
        {
            foreach (var reply in replyTo)
                this.replyTo.Add(reply);
        }

        public void ClearAttachment()
        {
            this.attachments = new List<Attachment>();
        }
    }
}
