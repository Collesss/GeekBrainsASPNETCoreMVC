using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Store.MailSender.MailKit.Options;

namespace Store.MailSender.MailKit
{
    public class MailSender : IMailSender<MessageData>, IDisposable, IAsyncDisposable
    {
        private readonly IOptionMailSender _optionMailSender;
        private readonly SmtpClient _smtpClient;

        public MailSender(IOptions<IOptionMailSender> optionMailSender)
        {
            _optionMailSender = optionMailSender.Value;
            _smtpClient = new SmtpClient();
        }

        public void Dispose()
        {
            if (_smtpClient.IsConnected)
                _smtpClient.Disconnect(true);

            _smtpClient.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_smtpClient.IsConnected)
                await _smtpClient.DisconnectAsync(true);

            _smtpClient.Dispose();
        }

        async Task IMailSender<MessageData>.Send(MessageData messageData, CancellationToken cancellationToken)
        {
            MimeMessage mimeMessage = new();

            mimeMessage.To.AddRange(_optionMailSender.To.Select(mail => new MailboxAddress(mail, mail)));
            mimeMessage.From.Add(new MailboxAddress(_optionMailSender.From, _optionMailSender.From));
            mimeMessage.Subject = messageData.Subject;
            mimeMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = messageData.Message
            };

            //using var emailClient = new SmtpClient();
            await _smtpClient.ConnectAsync(_optionMailSender.SmtpServer, _optionMailSender.SmtpPort, false, cancellationToken);
            await _smtpClient.AuthenticateAsync(_optionMailSender.SmtpUsername, _optionMailSender.SmtpPassword, cancellationToken);
            await _smtpClient.SendAsync(mimeMessage, cancellationToken);
            //await _smtpClient.DisconnectAsync(true);
        }
    }
}
