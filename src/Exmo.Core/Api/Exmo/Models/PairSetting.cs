namespace Exmo.Core.Api.Exmo.Models
{
    using Newtonsoft.Json;

    public class PairSetting
    {
        /// <summary>
        /// min_quantity - минимальное кол-во по ордеру
        /// </summary>
        [JsonProperty(PropertyName = "min_quantity")]
        public decimal MinQuantity { get; set; }

        /// <summary>
        /// max_quantity - максимальное кол-во по ордеру
        /// </summary>
        [JsonProperty(PropertyName = "max_quantity")]
        public decimal MaxQuantity { get; set; }

        /// <summary>
        /// min_price - минимальная цена по ордеру
        /// </summary>
        [JsonProperty(PropertyName = "min_price")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// max_price - максимальная цена по ордеру
        /// </summary>
        [JsonProperty(PropertyName = "max_price")]
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// max_amount - максимальная сумма по ордеру
        /// </summary>
        [JsonProperty(PropertyName = "max_amount")]
        public decimal MaxAmount { get; set; }

        /// <summary>
        /// min_amount - минимальная сумма по ордеру
        /// </summary>
        [JsonProperty(PropertyName = "min_amount")]
        public decimal MinAmount { get; set; }
    }
}
