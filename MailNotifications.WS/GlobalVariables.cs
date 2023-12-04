using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailNotifications.WS
{
    public class GlobalVariables
    {
        public int Interval { get; set; }
        public string LogFilePath { get; set; }
        public string MailURI { get; set; }
        public string MailMethod { get; set; }
        public string ApiToken { get; set; }
        public string TempAttachmentPath { get; set; }
        public string NotificationMailAddress { get; set; }
        public string CompanyPhoneNo { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWeb { get; set; }
        public string EmailServer { get; set; }
        public int EmailSmtpPort { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public bool EmailEnableSsl { get; set; }
    }

    public static class GlobalStaticVaiables
    {
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string MailURI { get; set; }
        public static string MailMethod { get; set; }
        public static string ApiToken { get; set; }
        public static string TempAttachmentPath { get; set; }
        public static string NotificationMailAddress { get; set; }
        public static string CompanyPhoneNo { get; set; }
        public static string CompanyEmail { get; set; }
        public static string CompanyWeb { get; set; }
        public static string EmailServer { get; set; }
        public static int EmailSmtpPort { get; set; }
        public static string EmailUserName { get; set; }
        public static string EmailPassword { get; set; }
        public static bool EmailEnableSsl { get; set; }
    }
}

