using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.MailSender
{
    public interface IMessageData
    {
        public string Subject { get; }
        public string Message { get; }
    }
}
