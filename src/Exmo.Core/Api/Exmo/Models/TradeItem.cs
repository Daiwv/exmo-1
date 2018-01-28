// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TradeItem.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TradeItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exmo.Core.Api.Exmo.Models
{
    using System;

    using JsonConverters;

    using Newtonsoft.Json;

    public class TradeItem
    {
        /// <summary>
        /// trade_id - идентификатор сделки
        /// </summary>
        [JsonProperty(PropertyName = "trade_id")]
        public ulong TradeId { get; set; }

        /// <summary>
        /// type - тип сделки
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// price - цена сделки
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        /// <summary>
        /// quantity - кол-во по сделке
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// amount - сумма сделки
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// date - дата и время сделки в формате Unix
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset Date { get; set; }       
    }
}
