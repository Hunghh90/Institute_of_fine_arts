using Institute_of_fine_arts.Dto;
using MailKit.Security;
using MimeKit.Utils;
using MimeKit;
using MailKit.Net.Smtp;

namespace Institute_of_fine_arts.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDto request)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("IFA Academy", _config.GetSection("EmailPassword").Value));
            message.To.Add(new MailboxAddress(request.Name, request.To));
            message.Subject = request.Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "";
            var image = bodyBuilder.LinkedResources.Add(request.Url);
            image.ContentId = MimeUtils.GenerateMessageId();

            bodyBuilder.HtmlBody = $"<p>{request.Body}</p><img src=\"cid:{image.ContentId}\" alt=\"Image\">";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                client.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
