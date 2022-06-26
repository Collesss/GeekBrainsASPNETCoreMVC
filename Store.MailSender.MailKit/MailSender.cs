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

        void IMailSender<MessageData>.Send(MessageData messageData)
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
            emailClient.Connect(_optionMailSender.SmtpServer, _optionMailSender.SmtpPort, false);
            emailClient.Authenticate(_optionMailSender.SmtpUsername, _optionMailSender.SmtpPassword);
            emailClient.Send(mimeMessage);
            emailClient.Disconnect(true);
        }
    }
}
