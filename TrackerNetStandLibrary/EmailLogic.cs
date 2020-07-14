using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Diagnostics.CodeAnalysis;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        private static string EmailAddress = GlobalConfig.GetAppSettingValue("emailAddress");
        private static string DisplayName = GlobalConfig.GetAppSettingValue("displayName");
        public static void SendEmail(string to, string Subject, string body)
        {
            SendEmail(new List<string> { to }, new List<string>(), Subject, body);
        }
        public static void SendEmail(List<string> to, List<string> bcc, string Subject, string body)
        {
            MailAddress fromAddress = new MailAddress(EmailAddress, DisplayName);

            MailMessage message = new MailMessage();

            message.From = fromAddress;

            to.ForEach(t => message.To.Add(t));
            bcc.ForEach(t => message.Bcc.Add(t));
            message.Subject = Subject;

            message.IsBodyHtml = true;
            message.Body = body;

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(message);
        }
    }
}
