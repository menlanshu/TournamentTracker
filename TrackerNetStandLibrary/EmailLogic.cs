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
            MailAddress fromAddress = new MailAddress(EmailAddress, DisplayName);
            MailAddress toAddress = new MailAddress(to);

            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.Subject = Subject;
            message.Body = body;

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(message);
        }
    }
}
