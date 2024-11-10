using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PRWebAPI.Models;
using MailKit.Net.Smtp;


namespace PRWebAPI.Service
{
    public class EmailService : IEmailService
    {
        private readonly Emailsettings emailsettings;
        public EmailService(IOptions<Emailsettings> options)
        {
            this.emailsettings = options.Value;
        }
        public async Task SendEmailAsync(Mailrequest mailrequest)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(emailsettings.Displayname, emailsettings.Email));
            mailMessage.To.Add(new MailboxAddress("", mailrequest.ToEmail));
            mailMessage.Subject = mailrequest.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = @"<html><body><h1>" + mailrequest.Body + "</h1></body></html>"
            };
            mailMessage.Body = builder.ToMessageBody();

            using var smtpClient = new SmtpClient();          
                smtpClient.Connect(emailsettings.Host, emailsettings.Port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(emailsettings.Email, emailsettings.Password);
                await smtpClient.SendAsync(mailMessage);
                smtpClient.Disconnect(true);
           

        }
    }
}
