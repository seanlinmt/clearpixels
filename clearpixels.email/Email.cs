using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using clearpixels.Logging;

namespace clearpixels.email
{
    public class Email
    {
        private string mailServer;
        private string username;
        private string password;

        public Email()
        {
            
        }

        public Email(string server, string username, string password)
        {
            this.mailServer = server;
            this.username = username;
            this.password = password;
        }

        public bool SendMail(MailAddress from, MailAddress to, string subject, string content, bool isHtml, IEnumerable<string> ccList = null)
        {

            var msg = new MailMessage(from, to)
            {
                Body = content,
                IsBodyHtml = isHtml,
                BodyEncoding = Encoding.UTF8,
                Subject = subject,
                SubjectEncoding = Encoding.UTF8
            };

            // cclist
            if (ccList != null)
            {
                foreach (var email in ccList)
                {
                    msg.CC.Add(new MailAddress(email));
                }
            }

            SmtpClient smtp = string.IsNullOrEmpty(mailServer) ? new SmtpClient() : new SmtpClient(mailServer);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                smtp.Credentials = new NetworkCredential(username, password);
            }
                
            bool sendOK = false;
            try
            {
                smtp.Send(msg);
                sendOK = true;
            }
            catch (Exception ex)
            {
                Syslog.Write(ex);
            }

            return sendOK;
        }
    }
}
