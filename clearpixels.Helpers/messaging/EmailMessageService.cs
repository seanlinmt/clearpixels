
/*
 * LICENSE NOTE:
 *
 * Copyright  2012-2013 Clear Pixels Limited, All Rights Reserved.
 *
 * Unless explicitly acquired and licensed from Licensor under another license, the
 * contents of this file are subject to the Reciprocal Public License ("RPL")
 * Version 1.5, or subsequent versions as allowed by the RPL, and You may not copy
 * or use this file in either source code or executable form, except in compliance
 * with the terms and conditions of the RPL. 
 *
 * All software distributed under the RPL is provided strictly on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND LICENSOR HEREBY
 * DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT, OR
 * NON-INFRINGEMENT. See the RPL for specific language governing rights and
 * limitations under the RPL.
 *
 * @author         Sean Lin Meng Teck <seanlinmt@clearpixels.co.nz>
 * @copyright      2012-2013 Clear Pixels Limited
 */
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using clearpixels.Logging;
using clearpixels.Models;

namespace clearpixels.Helpers.messaging
{
    public class EmailMessageService : IMessageService
    {
        private SmtpClient smtp;

        public EmailMessageService(string server, NetworkCredential credential)
        {
            smtp = new SmtpClient(server) {Credentials = credential};
        }

        public bool Send(IdName dest, IdName src, string subject, string body, string cclist = null, string replyto = null)
        {
            // need to check for invalid email address
            if (!IsValidEmail(dest.id))
            {
                return false;
            }

            var from = new MailAddress(src.id, src.name, Encoding.UTF8);
            var to = new MailAddress(dest.id, dest.name, Encoding.UTF8);
            var msg = new MailMessage(from, to)
            {
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                Subject = subject,
                SubjectEncoding = Encoding.UTF8
            };
            if (!string.IsNullOrEmpty(replyto))
            {
                msg.ReplyToList.Add(replyto);
            }

            // cclist
            if (!string.IsNullOrEmpty(cclist))
            {
                msg.CC.Add(cclist);
            }

            bool success = true;

            try
            {
                smtp.Send(msg);
            }
            catch (SmtpFailedRecipientException ex)
            {
                Syslog.Write(ex);
            }
            catch (Exception ex)
            {
                Syslog.Write(ex);
                success = false;
            }

            return success;
        }

        private bool IsValidEmail(string inputEmail)
        {
            if (inputEmail == null)
            {
                inputEmail = "";
            }
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}