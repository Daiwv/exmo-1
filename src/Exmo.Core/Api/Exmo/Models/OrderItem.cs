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

    public class OrderItem
    {
        /// <summary>
        /// order_id - идентификатор ордера
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public ulong OrderId { get; set; }

        /// <summary>
        /// pair - валютная пара
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public string Pair { get; set; }

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
        
        [JsonIgnore]
        public decimal Commission
        {
            get
            {
                return this.Amount * (decimal)0.002;
            }
        }

        /// <summary>
        /// date - дата и время сделки в формате Unix
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset Created { get; set; }       
    }
}
