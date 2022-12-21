using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using WebBlog.Models;


namespace WebBlog.Services
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;


        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSender = _mailSettings.EmailAddress ?? Environment.GetEnvironmentVariable("EmailAddress");
            MimeMessage newEmail = new();
            newEmail.Sender = MailboxAddress.Parse(emailSender);
            foreach (var emailAddress in email.Split(";"))
            {
                newEmail.To.Add(MailboxAddress.Parse(emailAddress));
            }
            newEmail.Subject = subject;
            BodyBuilder emailBody = new();
            emailBody.HtmlBody = htmlMessage;
            newEmail.Body = emailBody.ToMessageBody();
            //AT this point lets log into out smtp client
            using SmtpClient smtpClient = new();

            try
            {
                var host = _mailSettings.EmailHost ?? Environment.GetEnvironmentVariable("EmailHost");
                var port = _mailSettings.EmailPort != 0 ? _mailSettings.EmailPort : int.Parse(Environment.GetEnvironmentVariable("EmailPort")!);
                var password = _mailSettings.EmailPassword ?? Environment.GetEnvironmentVariable("EmailPassword");
                await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(emailSender, password);
                await smtpClient.SendAsync(newEmail);
                await smtpClient.DisconnectAsync(true);

            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
        }
    }
}
