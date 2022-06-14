namespace Store.MailSender.MailKit
{
    public class MessageData : IMessageData
    {
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
