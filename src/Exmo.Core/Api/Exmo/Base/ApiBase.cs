namespace Exmo.Core.Api.Exmo.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using Newtonsoft.Json;

    public class ExmoApiBase
    {
        private static long _nounce;
        // API settings
        private string _key;
        private string _secret;
        private string _url = "http://api.exmo.com/v1/{0}";

        static ExmoApiBase()
        {
            _nounce = GetTimestamp();
        }

        public ExmoApiBase(string key, string secret)
        {
            this._key = key;
            this._secret = secret;
        }

        public async Task<TResponse> ApiQueryAsync<TResponse>(string apiName, IDictionary<string, string> req = null, CancellationToken token = default(CancellationToken))
        {
            var result = await this.ApiQueryAsync(apiName, req, token);
            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        public async Task<string> ApiQueryAsync(string apiName, IDictionary<string, string> req = null, CancellationToken token = default(CancellationToken))
        {
            using (var client = new HttpClient())
            {
                var n = Interlocked.Increment(ref _nounce);

                if (req == null)
                {
                    req = new Dictionary<string, string>();
                }

                req.Add("nonce", Convert.ToString(n));
                var message = this.ToQueryString(req);

                var sign = Sign(this._secret, message);

                var content = new FormUrlEncodedContent(req);
                content.Headers.Add("Sign", sign);
                content.Headers.Add("Key", this._key);

                var response = await client.PostAsync(string.Format(this._url, apiName), content, token);

                return await response.Content.ReadAsStringAsync();
            }
        }

        private string ToQueryString(IDictionary<string, string> dic)
        {
            var array = (from key in dic.Keys
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(dic[key])))
                .ToArray();
            return string.Join("&", array);
        }

        private static string Sign(string key, string message)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return ByteToString(b);
            }
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary).ToLowerInvariant();
        }

        private static long GetTimestamp()
        {
            var d = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return (long)d;
        }
    }

}