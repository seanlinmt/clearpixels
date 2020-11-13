using System.IO;
using System.Net;
using clearpixels.Helpers;
using clearpixels.Logging;

namespace clearpixels.SMS
{
    public static class Clickatell
    {
        private const string commandUrl =
            "http://api.clickatell.com/http/sendmsg?user=seanlinmt&password=lodgeLodge1976&api_id=3372266&to={0}&text={1}";

        public static bool Send(string message, string number)
        {
            var requestUrl = string.Format(commandUrl, number.ToNumbersOnly(), message + " :Lodge School");
            string content;
            WebResponse response;
            try
            {
                var request = WebRequest.Create(requestUrl);
                request.Method = "GET";
                response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                response = ex.Response;
                if (response != null)
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var error = sr.ReadToEnd();
                        Syslog.Write(ErrorLevel.ERROR, "SMS Error: " + requestUrl + " " + error);
                    }
                }
                return false;
            }

            return true;
        }
    }
}