/**
 * @file OAuthController.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using YNote.Common;

namespace YNote.OAuth
{
    public class OAuthController : OAuthUtil.OAuthBase
    {
        private OAuthConfigMgmt _oauthMgmt = new OAuthConfigMgmt();
        public OAuthConfigMgmt OAuthMgmt
        {
            get { return _oauthMgmt; }
        }

        private string _requestToken = string.Empty;
        public string RequestToken
        {
            get { return _requestToken; }
            set { _requestToken = value; }
        }

        private string _requestSecret = string.Empty;
        public string RequestSecret
        {
            get { return _requestSecret; }
            set { _requestSecret = value; }
        }

        private string _accessToken = string.Empty;
        public string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        private string _accessSecret = string.Empty;
        public string AccessSecret
        {
            get { return _accessSecret; }
            set { _accessSecret = value; }
        }

        private string _requestVerifier = string.Empty;
        public string RequestVerifier
        {
            get { return _requestVerifier; }
            set { _requestVerifier = value; }
        }

        private const string _oauthTimeUrl = "http://note.youdao.com/oauth/time";

        private const string _requestTokenUrl = "http://note.youdao.com/oauth/request_token";
        public string RequestTokenUrl
        {
            get { return _requestTokenUrl; }
        }

        private const string _authorUrl = "http://note.youdao.com/oauth/authorize?oauth_token=";
        public string AuthorUrl
        {
            get {return _authorUrl;}
        }
        private const string _accessTokenUrl = "http://note.youdao.com/oauth/access_token";
        public string AccessTokeUrl
        {
            get { return _accessTokenUrl; }
        }

        private const string _oauthTokenKey = "oauth_token";
        private const string _oauthTokenSecretKey = "oauth_token_secret";
        private const string _oauthTokenVerifierKey = "oauth_verifier";
        public string OAuthVerifierKey
        {
            get { return _oauthTokenVerifierKey; }
        }

        public OAuthController()
        {

        }

        public async Task  GetRequestToken()
        {
            string requestUrl = _requestTokenUrl;
            string oauthSignature = string.Empty;
            string result = string.Empty;
            string normalizedUrl = string.Empty;
            string normalizedParams = string.Empty;
            string jsonTime = await GenerateTimeStampFromServer();
            if (string.IsNullOrEmpty(jsonTime))
            {
                return;
            }

            // Construct request for request token
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add(OAuthVersionKey, "1.0");
            paras.Add(OAuthSignatureMethodKey, "HMAC-SHA1");
            paras.Add(OAuthTimestampKey, jsonTime);
            paras.Add(OAuthNonceKey, GenerateNonce());
            paras.Add(OAuthConsumerKeyKey, _oauthMgmt.ConsumerKey);
            paras.Add(OAuthCallbackKey, UrlEncode(_oauthMgmt.CallBack));

            oauthSignature = GenerateSignature(
                new Uri(requestUrl), 
                _oauthMgmt.CallBack, 
                _oauthMgmt.ConsumerKey, 
                _oauthMgmt.ConsumerSecret, 
                string.Empty, 
                string.Empty, 
                string.Empty,
                "GET",
                paras[OAuthTimestampKey], 
                paras[OAuthNonceKey],
                out normalizedUrl,
                out normalizedParams
                );

            paras.Add(OAuthSignatureKey, UrlEncode(oauthSignature));
            result = await HttpGet(requestUrl + "?" + ToQueryStringEx(paras));
            GetOAuthTokenAndSecret(result, ref _requestToken, ref _requestSecret);
        }

        public async Task GetAccessToken()
        {
            string requestUrl = _accessTokenUrl;
            string oauthSignature = "";
            string result = "";
            string normalizedUrl = string.Empty;
            string normalizedParams = string.Empty;

            string jsonTime = await GenerateTimeStampFromServer();
            if (string.IsNullOrEmpty(jsonTime))
            {
                return;
            }

            // Construct request for access token
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add(OAuthVersionKey, "1.0");
            paras.Add(OAuthSignatureMethodKey, "HMAC-SHA1");
            paras.Add(OAuthTimestampKey, jsonTime);
            paras.Add(OAuthNonceKey, GenerateNonce());
            paras.Add(OAuthConsumerKeyKey, _oauthMgmt.ConsumerKey);
            paras.Add(OAuthVerifierKey, _requestVerifier);
            paras.Add(OAuthTokenKey, _requestToken);

            oauthSignature = GenerateSignature(
                new Uri(requestUrl), 
                string.Empty,
                _oauthMgmt.ConsumerKey, 
                _oauthMgmt.ConsumerSecret, 
                _requestToken, 
                _requestSecret, 
                _requestVerifier,
                "GET", 
                paras[OAuthTimestampKey], 
                paras[OAuthNonceKey], 
                out normalizedUrl,
                out normalizedParams
                );

            paras.Add(OAuthSignatureKey, UrlEncode(oauthSignature));
            result = await HttpGet(requestUrl + "?" + ToQueryStringEx(paras));
            GetOAuthTokenAndSecret(result, ref _accessToken, ref _accessSecret);
        }

        async Task<string> GenerateTimeStampFromServer()
        {
            string jsonTime = await HttpGet(_oauthTimeUrl);
            if (!string.IsNullOrEmpty(jsonTime))
            {
                DataContractJsonSerializer jsonSerializeer = new DataContractJsonSerializer(typeof(TimeJson));
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonTime));
                TimeJson obj = (TimeJson)jsonSerializeer.ReadObject(stream);
                return obj.oauth_timestamp;
            }
            return string.Empty;
        }

        private void GetOAuthTokenAndSecret(string paras, ref string oauthToken, ref string oauthSecret)
        {
            if (paras != string.Empty)
            {
                IDictionary<string, string> dictToken = Common.HttpHelper.GetQueryParameters(paras);
                if (dictToken.ContainsKey(_oauthTokenKey))
                {
                    oauthToken = dictToken[_oauthTokenKey];
                }

                if (dictToken.ContainsKey(_oauthTokenSecretKey))
                {
                    oauthSecret = dictToken[_oauthTokenSecretKey];
                }
            }
        }

        public async Task<IDictionary<string, string>> ConstructOpenAPIHeaders(string url, string methord, IDictionary<string, string> queryParas)
        {
            string oauthSignature = "";
            string normalizedUrl = string.Empty;
            string normalizedParams = string.Empty;
            string result = string.Empty;
            IDictionary<string, string> paras = new Dictionary<string, string>();
            string jsonTime = await GenerateTimeStampFromServer();
            if (string.IsNullOrEmpty(jsonTime))
            {
                return paras;
            }

            CollectionHelper.CloneDict(queryParas, paras);
            paras.Add(OAuthVersionKey, "1.0");
            paras.Add(OAuthSignatureMethodKey, "HMAC-SHA1");
            paras.Add(OAuthTimestampKey, jsonTime);
            paras.Add(OAuthNonceKey, GenerateNonce());
            paras.Add(OAuthConsumerKeyKey, _oauthMgmt.ConsumerKey);
            paras.Add(OAuthTokenKey, _accessToken);

            if ((queryParas != null) && (queryParas.Count > 0))
            {
                if (url.IndexOf('?') > 0)
                {
                    url += "&" + ToQueryStringEx(queryParas);
                }
                else
                {
                    url += "?" + ToQueryStringEx(queryParas);
                }
            }

            oauthSignature = GenerateSignature(
                new Uri(url),
                string.Empty,
                _oauthMgmt.ConsumerKey,
                _oauthMgmt.ConsumerSecret,
                _accessToken,
                _accessSecret,
                string.Empty,
                methord,
                paras[OAuthTimestampKey],
                paras[OAuthNonceKey],
                out normalizedUrl,
                out normalizedParams
                );

            paras.Add(OAuthSignatureKey, UrlEncode(oauthSignature));
            return paras;
        }
    }

    [DataContract]
    public class TimeJson
    {
        [DataMember]
        public string unit { get; set; }

        [DataMember]
        public string oauth_timestamp { get; set; }
    }
}
