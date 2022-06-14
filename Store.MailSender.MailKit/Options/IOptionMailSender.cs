using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.MailSender.MailKit.Options
{
    public interface IOptionMailSender
    {
        public string SmtpServer { get; }
        public int SmtpPort { get; }
        public string SmtpUsername { get; }
        public string SmtpPassword { get; }
        public string[] To { get; }
        public string From { get; }
    }
}
