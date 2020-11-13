using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using clearpixels.Logging;

namespace clearpixels.Network
{
    // used to detect the external ip address of the network we are currently in
    public class IpAddressResolver
    {
        public IPAddress GetPublicIpAddress()
        {
            IPAddress resolvedPublicIP = null;
            var ip = GetPublicIpAddressFromDynDns();

            if (string.IsNullOrEmpty(ip))
            {
                // try the other service
                ip = GetPublicIpAddressFromDnsOMatic();
            }

            if (!string.IsNullOrEmpty(ip))
            {
                if (!IPAddress.TryParse(ip, out resolvedPublicIP))
                {
                    Syslog.Write(ErrorLevel.WARNING, "Failed to parse ipaddr " + ip);
                }
            }
            return resolvedPublicIP;
        }

        /// <summary>
        /// Retrieve the public IP address from DNS-O-Matic at http://myip.dnsomatic.com/
        /// </summary>
        /// <returns>The public IP Address.</returns>
        private string GetPublicIpAddressFromDnsOMatic()
        {
            string uri = "http://myip.dnsomatic.com/";
            string responseBody = string.Empty;

            try
            {
                WebRequest request = WebRequest.Create(uri);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        responseBody = stream.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Syslog.Write(ex);
            }

            var options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

            var regex = new Regex(@"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}", options);

            var match = regex.Match(responseBody);

            if (match.Success)
            {
                return match.Value;
            }

            return null;
        }

        /// <summary>
        /// Get the public IP Address from DynDns at: http://checkip.dyndns.org/
        /// </summary>
        /// <returns>The public IP Address.</returns>
        private string GetPublicIpAddressFromDynDns()
        {
            string uri = "http://checkip.dyndns.org/";
            string responseBody = string.Empty;

            try
            {
                WebRequest request = WebRequest.Create(uri);
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        responseBody = stream.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Syslog.Write(ex);
            }

            var options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

            var regex = new Regex(@"<body>Current IP Address:\s?(\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3})\s?</body>", options);

            var match = regex.Match(responseBody);

            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }

            return null;
        }
    }
}
