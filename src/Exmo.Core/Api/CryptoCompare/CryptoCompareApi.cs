using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exmo.Core.Api.CryptoCompare
{
    using System.Net.Http;
    using System.Threading;

    using global::Exmo.Core.Api.CryptoCompare.Models;
    using global::Exmo.Core.Api.Exmo.Extensions;

    using Newtonsoft.Json;

    public class CryptoCompareApi
    {
        public const string BaseUrl = "https://min-api.cryptocompare.com/data/";
        
        public async Task<CryptoCompareResponse<HistoryDataItem[]>> HistoryMinute(string fsym, string tsym, int aggregate, int limit, CancellationToken token)
        {
            var param = new Dictionary<string, object>();
            param.Add("fsym", fsym);
            param.Add("tsym", tsym);
            param.Add("e", "Exmo");
            param.Add("aggregate", aggregate);
            param.Add("extraParams", "CryptoCompare");
            param.Add("limit", limit);
            param.Add("tryConversion", false);

            return await ApiQueryAsync<CryptoCompareResponse<HistoryDataItem[]>>("histominute", param, token).ConfigureAwait(false);
        }

        public async Task<CryptoCompareResponse<HistoryDataItem[]>> HistoryHour(string fsym, string tsym, int aggregate, int limit, CancellationToken token)
        {
            var param = new Dictionary<string, object>();
            param.Add("fsym", fsym);
            param.Add("tsym", tsym);
            param.Add("e", "Exmo");
            param.Add("aggregate", aggregate);
            param.Add("extraParams", "CryptoCompare");
            param.Add("limit", limit);
            param.Add("tryConversion", false);

            return await ApiQueryAsync<CryptoCompareResponse<HistoryDataItem[]>>("histohour", param, token).ConfigureAwait(false);
        }

        public async Task<CryptoCompareResponse<HistoryDataItem[]>> HistoryDay(string fsym, string tsym, int aggregate, int? limit, CancellationToken token, bool? allData = null)
        {
            var param = new Dictionary<string, object>();
            param.Add("fsym", fsym);
            param.Add("tsym", tsym);
            param.Add("e", "Exmo");
            param.Add("aggregate", aggregate);
            param.Add("extraParams", "CryptoCompare");
           
            param.Add("tryConversion", false);

            if (limit.HasValue)
            {
                param.Add("limit", limit);
            }

            if (allData.HasValue)
            {
                param.Add("allData", allData);
            }

            return await ApiQueryAsync<CryptoCompareResponse<HistoryDataItem[]>>("histoday", param, token).ConfigureAwait(false);
        }

        private static async Task<TResponse> ApiQueryAsync<TResponse>(string apiName, Dictionary<string, object> param, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + apiName + param.ToNameValueCollection().ToQueryString(), token).ConfigureAwait(false);

                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<TResponse>(result);
            }
        }
    }
}
