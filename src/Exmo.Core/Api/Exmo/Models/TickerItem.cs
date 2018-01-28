namespace Exmo.Core.Api.Exmo.Models
{
    using System;

    using JsonConverters;

    using Newtonsoft.Json;

    public class TickerItem
    {
        /// <summary>
        /// high - максимальная цена сделки за 24 часа
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public decimal High { get; set; }

        /// <summary>
        /// low - минимальная цена сделки за 24 часа
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public decimal Low { get; set; }

        /// <summary>
        /// avg - средняя цена сделки за 24 часа
        /// </summary>
        [JsonProperty(PropertyName = "avg")]
        public decimal Avg { get; set; }

        /// <summary>
        /// vol - объем всех сделок за 24 часа
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public decimal Vol { get; set; }

        /// <summary>
        /// vol_curr - сумма всех сделок за 24 часа
        /// </summary>
        [JsonProperty(PropertyName = "vol_curr")]
        public decimal VolCurr { get; set; }

        /// <summary>
        /// last_trade - цена последней сделки
        /// </summary>
        [JsonProperty(PropertyName = "last_trade")]
        public decimal LastTrade { get; set; }

        /// <summary>
        /// buy_price - текущая максимальная цена покупки
        /// </summary>
        [JsonProperty(PropertyName = "buy_price")]
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// sell_price - текущая минимальная цена продажи
        /// </summary>
        [JsonProperty(PropertyName = "sell_price")]
        public decimal SellPrice { get; set; }

        [JsonProperty(PropertyName = "close_buy_price")]
        public decimal CloseBuyPrice { get; set; }

        /// <summary>
        /// updated - дата и время обновления данных
        /// </summary>
        [JsonProperty(PropertyName = "updated")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTime Updated { get; set; }

        public override string ToString()
        {
            return $"High: {this.High}, Low: {this.Low}, Avg: {this.Avg}, VolCurr: {this.VolCurr}, LastTrade: {this.LastTrade}, BuyPrice: {this.BuyPrice}, SellPrice: {this.SellPrice}, Updated: {this.Updated}";
        }
    }
}
