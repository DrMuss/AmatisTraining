using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Services.Messaging
{
    public class EmailConfig
    {
        public string SMTPServer { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromWho { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
