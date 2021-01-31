using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Google.GData.Client;
using clearpixels.Logging;
using tradelr.Library;
using HttpUtility = System.Web.HttpUtility;

namespace clearpixels.OAuth
{
    public class OAuthClient : OAuthBase
    {
        // oauth
        // trademe
#if DEBUG
        public const string OAUTH_TRADEME_CONSUMER_KEY = "*********************";
        public const string OAUTH_TRADEME_CONSUMER_SECRET = "*********************";
#else
        public const string OAUTH_TRADEME_CONSUMER_KEY = "*********************";
        public const string OAUTH_TRADEME_CONSUMER_SECRET = "*********************";
#endif

        // twitter
        // for domain admin
        public const string OAUTH_TWITTER_CONSUMER_KEY = "*********************";
        public const string OAUTH_TWITTER_CONSUMER_SECRET = "*********************";

        // for users
        public const string OAUTH_TWITTER_NETWORK_CONSUMER_KEY = "*********************";
        public const string OAUTH_TWITTER_NETWORK_CONSUMER_SECRET = "*********************";

        // yahoo
#if DEBUG
        public const string OAUTH_YAHOO_CONSUMER_KEY = "*********************";
        public const string OAUTH_YAHOO_CONSUMER_SECRET = "*********************";
#else
        public const string OAUTH_YAHOO_CONSUMER_KEY = "*********************--";
        public const string OAUTH_YAHOO_CONSUMER_SECRET = "*********************";
#endif

        public string oauth_token { get; set; }
        public string oauth_secret { get; set; }
        public string authorize_url { get; set; }
        public string guid { get; set; } // unique identifier, usually unique userid for the site

        private string callback_url { get; set; }
        private string consumer_key { get; set; }
        private string consumer_secret { get; set; }
        private OAuthTokenType type { get; set; }
        private string signature_method { get; set; }
        private string oauth_verifier { get; set; }
        private NameValueCollection parameters { get; set; }
        private string baseUrl { get; set; }
        private string requestUrl { get; set; }
        private string scope { get; set; }

        private OAuthClient(OAuthTokenType type, string consumer_key, string consumer_secret, string callback_url ="", string scope ="")
        {
            this.type = type;
            this.consumer_key = consumer_key;
            this.consumer_secret = consumer_secret;
            this.callback_url = callback_url;
            this.scope = scope;
            parameters = new NameValueCollection();
        }

        /// <summary>
        /// for request token request
        /// </summary>
        /// <param name="type"></param>
        /// <param name="consumer_key"></param>
        /// <param name="consumer_secret"></param>
        /// <param name="callback_url"></param>
        /// <param name="signature_method"></param>
        public OAuthClient(OAuthTokenType type, string consumer_key, string consumer_secret, string callback_url = "", string signature_method = "", string scope = "")
            : this(type, consumer_key, consumer_secret, callback_url, scope)
        {
            this.signature_method = signature_method;
        }

        /// <summary>
        /// for access token request
        /// </summary>
        /// <param name="type"></param>
        /// <param name="consumer_key"></param>
        /// <param name="consumer_secret"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_secret"></param>
        /// <param name="oauth_verifier"></param>
        public OAuthClient(OAuthTokenType type, string consumer_key, string consumer_secret, string oauth_token, string oauth_secret, string oauth_verifier, string signature_method, string scope = "")
            :this(type, consumer_key, consumer_secret, "",scope)
        {
            this.oauth_token = oauth_token;
            this.oauth_secret = oauth_secret;
            this.oauth_verifier = oauth_verifier;
            this.signature_method = signature_method;
        }

        private void BuildRequestUrl()
        {
            var nonce = GenerateNonce();
            var timestamp = GenerateTimeStamp();

            switch (type)
            {
                case OAuthTokenType.YAHOO:
                    if (!String.IsNullOrEmpty(oauth_token))
                    {
                        baseUrl = "https://api.login.yahoo.com/oauth/v2/get_token";
                    }
                    else
                    {
                        baseUrl = "https://api.login.yahoo.com/oauth/v2/get_request_token";
                        parameters.Add("oauth_callback", callback_url);
                    }
                    parameters.Add("oauth_signature", OAUTH_YAHOO_CONSUMER_SECRET + "%26" + oauth_secret);
                    
                    break;
                case OAuthTokenType.TRADEME:
#if DEBUG
                    if (!String.IsNullOrEmpty(oauth_token))
                    {
                        baseUrl = "https://secure.tmsandbox.co.nz/Oauth/AccessToken";
                    }
                    else
                    {
                        baseUrl = "https://secure.tmsandbox.co.nz/Oauth/RequestToken";
                        parameters.Add("oauth_callback", callback_url);
                    }
#else
                    if (!String.IsNullOrEmpty(oauth_token))
                    {
                        baseUrl = "https://secure.trademe.co.nz/Oauth/AccessToken";
                    }
                    else
                    {
                        baseUrl = "https://secure.trademe.co.nz/Oauth/RequestToken";
                        parameters.Add("oauth_callback", callback_url);
                    }
#endif
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            parameters.Add("oauth_signature_method", signature_method);
            parameters.Add("oauth_consumer_key", consumer_key);

            if (!String.IsNullOrEmpty(oauth_token))
            {
                parameters.Add("oauth_token", oauth_token);
                parameters.Add("oauth_verifier", oauth_verifier);
            }
            parameters.Add("oauth_nonce", nonce);
            parameters.Add("oauth_timestamp", timestamp);
            parameters.Add("oauth_version", "1.0");
            if (!string.IsNullOrEmpty(scope))
            {
                parameters.Add("scope", scope);
            }

            switch (type)
            {
                case OAuthTokenType.YAHOO:
                    requestUrl = String.Concat(baseUrl, parameters.ToQueryString(false));
                    break;
                case OAuthTokenType.ETSY:
                case OAuthTokenType.TRADEME:
                    requestUrl = String.Concat(baseUrl, parameters.ToQueryString(true));
                    var signature = GenerateSignature(new Uri(requestUrl), consumer_key, consumer_secret,
                                                      oauth_token, oauth_secret, "GET", timestamp,
                                                      nonce);
                    parameters.Add(OAuthSignatureKey, signature);
                    requestUrl = String.Concat(baseUrl, parameters.ToQueryString(true));
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        public bool GetRequestToken()
        {
            BuildRequestUrl();
            
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
                        Syslog.Write(ErrorLevel.ERROR, "oauthrequest Error: " + requestUrl + " " + error);
                    }
                }
                return false;
            }

            parameters = HttpUtility.ParseQueryString(content);

            oauth_token = parameters["oauth_token"];
            oauth_secret = parameters["oauth_token_secret"];
            switch (type)
            {
                case OAuthTokenType.YAHOO:
                    authorize_url = HttpUtility.UrlDecode(parameters["xoauth_request_auth_url"]);
                    break;
                case OAuthTokenType.ETSY:
                    authorize_url = HttpUtility.UrlDecode(parameters["login_url"]);
                    break;
                case OAuthTokenType.TRADEME:
#if DEBUG
                    authorize_url = "https://secure.tmsandbox.co.nz/Oauth/Authorize?oauth_token=" + oauth_token;
#else
                    authorize_url = "https://secure.trademe.co.nz/Oauth/Authorize?oauth_token=" + oauth_token;
#endif
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            if (String.IsNullOrEmpty(oauth_token) || String.IsNullOrEmpty(oauth_secret) || String.IsNullOrEmpty(authorize_url))
            {
                Syslog.Write(ErrorLevel.ERROR, String.Format("{0} OAuth fail", type));
                return false;
            }
            return true;
        }

        public bool GetAccessToken()
        {
            BuildRequestUrl();
            string content;
            WebResponse response;
            try
            {
                var request = WebRequest.Create(requestUrl);
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
                        Syslog.Write(ErrorLevel.ERROR, "oauthrequest Error: " + requestUrl + " " + error);
                    }
                }
                return false;
            }

            parameters = HttpUtility.ParseQueryString(content);

            oauth_token = parameters["oauth_token"];
            oauth_secret = parameters["oauth_token_secret"];
            switch (type)
            {
                case OAuthTokenType.YAHOO:
                    guid = parameters["xoauth_yahoo_guid"];
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}