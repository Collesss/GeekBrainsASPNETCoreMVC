using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.MailSender.MailKit.Options
{
    public class OptionMailSender : IOptionMailSender
    {
        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public string[] To { get; set; }

        public string From { get; set; }
    }
}
