using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Service
{
    public class Email
    {
        public static string SMTPServer { get; set; }
        public static int SMTPPort { get; set; }
        public static bool SMTPSSL { get; set; }
        public static string From { get; set; }
        public static string FromPassword { get; set; }
        public static string To { get; set; }
    }
}
