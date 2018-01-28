// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExmoPublicApi.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ExmoPublicApi type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo.Core.Api.Exmo
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Threading;
    using System.Threading.Tasks;


    using global::Exmo.Core.Api.Exmo.Base;

    using Models;

    using Newtonsoft.Json.Linq;

    public class ExmoApi : ExmoApiBase
    {
        public ExmoApi()
            : this("K-7cc97c89aed2a2fd9ed7792d48d63f65800c447b", "S-dbefd4ddec7e4930e645645645618c278ba35374f")
        {
            
        }

        public ExmoApi(string key, string secret)
            : base(key, secret)
        {
        }

        /// <summary>
        /// Cписок валют биржи
        /// </summary>
        /// <returns></returns>
        public Task<string[]> GetCurrencies()
        {
            return this.ApiQueryAsync<string[]>("currency");
        }

        /// <summary>
        /// Настройки валютных пар
        /// </summary>
        /// <returns></returns>
        public Task<Dictionary<string, PairSetting>> GetPairSettings()
        {
            return this.ApiQueryAsync<Dictionary<string, PairSetting>>("pair_settings");
        }

        /// <summary>
        /// Cтатистика цен и объемов торгов по валютным парам
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, TickerItem>> GetTicker()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://exmo.com/ctrl/ticker");
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());

                return json["data"]["ticker"].ToObject<Dictionary<string, TickerItem>>();
            }
        }

        /// <summary>
        /// Книга ордеров по валютной паре
        /// </summary>
        /// <param name="pair">
        /// pair - одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)
        /// </param>
        /// <param name="limit">
        /// limit - кол-во отображаемых позиций (по умолчанию 100, максимум 1000)
        /// </param>
        /// <returns>
        /// </returns>
        public Task<Dictionary<string, OrderBookItem>> OrderBook(int? limit, params string[] pair)
        {
            var param = new Dictionary<string, string> { { "pair", string.Join(',', pair) } };

            if (limit.HasValue)
            {
                param.Add("limit", limit.ToString());
            }

            return this.ApiQueryAsync<Dictionary<string, OrderBookItem>>("order_book", param);
        }
        
        /// <summary>
        /// Список сделок по валютной паре
        /// </summary>
        /// <param name="pair">
        /// pair - одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)
        /// </param>
        /// <returns>
        /// </returns>
        public Task<Dictionary<string, TradeItem[]>> Trades(params string[] pair)
        {
            var param = new Dictionary<string, string> { { "pair", string.Join(',', pair) } };
            
            return this.ApiQueryAsync<Dictionary<string, TradeItem[]>>("trades", param);            
        }

        /// <summary>
        /// The chart data.
        /// </summary>
        /// <param name="period">
        /// Период - day,week,month
        /// </param>
        /// <param name="para">
        /// одна несколько валютная пара
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ChartData(string para, string period = "day")
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://exmo.com/ctrl/chartMain?type=0&period={period}&para={para}");
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
               
                foreach (var item in json["data"]["price"].Children())
                {
                    var i = item[0].Value<long>();
                }

                var s = await response.Content.ReadAsStringAsync();
            }

        }

        /// <summary>
        /// Книга ордеров по валютной паре
        /// </summary>
        /// <param name="pair">
        /// pair - одна или несколько валютных пар разделенных запятой (пример BTC_USD,BTC_EUR)
        /// </param>
        /// <param name="limit">
        /// limit - кол-во отображаемых позиций (по умолчанию 100, максимум 1000)
        /// </param>
        /// <param name="offset">
        /// offset - смещение от последней сделки (по умолчанию 0)
        /// </param>
        /// <returns>
        /// </returns>
        public Task<Dictionary<string, UserTradeItem[]>> UserTrades(int limit,int offset, CancellationToken token, params string[] pair)
        {
            var param = new Dictionary<string, string> { { "pair", string.Join(',', pair) } };

            param.Add("limit", limit.ToString());
            param.Add("offset", offset.ToString());

            return this.ApiQueryAsync<Dictionary<string, UserTradeItem[]>>("user_trades", param);
        }

        /// <summary>
        /// Получение списока открытых ордеров пользователя
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<Dictionary<string, OrderItem[]>> OpenOrders(CancellationToken token)
        {
            return this.ApiQueryAsync<Dictionary<string, OrderItem[]>>("user_open_orders");
        }
    }
}
