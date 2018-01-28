using System;

namespace Exmo.Core.Api.CryptoCompare.Models
{
    using global::Exmo.Core.Api.JsonConverters;

    using Newtonsoft.Json;

    public class CryptoCompareResponse<T>
    {
        [JsonProperty(PropertyName = "Response")]
        public string Response { get; set; }

        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "Type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "Aggregated")]
        public bool Aggregated { get; set; }

        [JsonProperty(PropertyName = "FirstValueInArray")]
        public bool FirstValueInArray { get; set; }

        [JsonProperty(PropertyName = "TimeTo")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTime TimeTo { get; set; }

        [JsonProperty(PropertyName = "TimeFrom")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTime TimeFrom { get; set; }

        [JsonProperty(PropertyName = "Data")]
        public T Data { get; set; }
    }
}
