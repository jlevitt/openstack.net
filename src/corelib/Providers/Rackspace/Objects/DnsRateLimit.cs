namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using JSIStudios.SimpleRESTServices.Client;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRateLimit
    {
        [JsonProperty("verb")]
        [JsonConverter(typeof(StringEnumConverter))]
        private HttpMethod _verb;

        [JsonProperty("unit")]
        private string _unit;

        [JsonProperty("value")]
        private long? _value;

        [JsonProperty("remaining")]
        private long? _remaining;

        [JsonProperty("next-available")]
        private DateTimeOffset _nextAvailable;
    }
}
