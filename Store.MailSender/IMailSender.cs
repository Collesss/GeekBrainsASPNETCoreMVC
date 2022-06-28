using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.MailSender
{
    public interface IMailSender<in T> where T : IMessageData
    {
        public Task Send(T messageData, CancellationToken cancellationToken);
    }
}
