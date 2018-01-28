using System;


namespace Exmo.Core.Api.CryptoCompare.Models
{
    using global::Exmo.Core.Api.JsonConverters;

    using Newtonsoft.Json;

    public class HistoryDataItem
    {
        [JsonProperty(PropertyName = "open")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "high")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "low")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "close")]
        public decimal Close { get; set; }

        [JsonProperty(PropertyName = "volumefrom")]
        public decimal VolumeFrom { get; set; }

        [JsonProperty(PropertyName = "volumeto")]
        public decimal VolumeTo { get; set; }

        [JsonProperty(PropertyName = "time")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTime Time { get; set; }
    }
}
