namespace Exmo.Core.Api.Exmo.Models
{
    using Newtonsoft.Json;

    public class OrderBookItem
    {
        /// <summary>
        /// ask_quantity - объем всех ордеров на продажу
        /// </summary>
        [JsonProperty(PropertyName = "ask_quantity")]
        public decimal AskQuantity { get; set; }

        /// <summary>
        /// ask_amount - сумма всех ордеров на продажу
        /// </summary>
        [JsonProperty(PropertyName = "ask_amount")]
        public decimal AskAmount { get; set; }

        /// <summary>
        /// ask_top - минимальная цена продажи
        /// </summary>
        [JsonProperty(PropertyName = "ask_top")]
        public decimal AskTop { get; set; }

        /// <summary>
        /// bid_quantity - объем всех ордеров на покупку
        /// </summary>
        [JsonProperty(PropertyName = "bid_quantity")]
        public decimal BidQuantity { get; set; }

        /// <summary>
        /// bid_amount - сумма всех ордеров на покупку
        /// </summary>
        [JsonProperty(PropertyName = "bid_amount")]
        public decimal BidAmount { get; set; }

        /// <summary>
        /// bid_top - максимальная цена покупки
        /// </summary>
        [JsonProperty(PropertyName = "bid_top")]
        public decimal BidTop { get; set; }

        /// <summary>
        /// bid - список ордеров на покупку, где каждая строка это цена, количество и сумма
        /// </summary>
        [JsonProperty(PropertyName = "bid")]
        public decimal[][] Bid { get; set; }

        /// <summary>
        /// ask - список ордеров на продажу, где каждая строка это цена, количество и сумма
        /// </summary>
        [JsonProperty(PropertyName = "ask")]
        public decimal[][] Ask { get; set; }
    }
}
