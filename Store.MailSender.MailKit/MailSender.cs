using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Store.MailSender.MailKit.Options;

namespace Store.MailSender.MailKit
{
    public class MailSender : IMailSender<MessageData>
    {
        private readonly IOptionMailSender _optionMailSender;

        public MailSender(IOptions<IOptionMailSender> optionMailSender)
        {
            _optionMailSender = optionMailSender.Value;
        }

        async Task IMailSender<MessageData>.Send(MessageData messageData)
        {
            MimeMessage mimeMessage = new();

            mimeMessage.To.AddRange(_optionMailSender.To.Select(mail => new MailboxAddress(mail, mail)));
            mimeMessage.From.Add(new MailboxAddress(_optionMailSender.From, _optionMailSender.From));
            mimeMessage.Subject = messageData.Subject;
            mimeMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = messageData.Message
            };

            using var emailClient = new SmtpClient();
            await emailClient.ConnectAsync(_optionMailSender.SmtpServer, _optionMailSender.SmtpPort, false);
            await emailClient.AuthenticateAsync(_optionMailSender.SmtpUsername, _optionMailSender.SmtpPassword);
            await emailClient.SendAsync(mimeMessage);
            await emailClient.DisconnectAsync(true);
        }
    }
}
