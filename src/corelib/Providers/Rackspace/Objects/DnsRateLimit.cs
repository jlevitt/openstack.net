namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using JSIStudios.SimpleRESTServices.Client;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRateLimit
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
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
#pragma warning restore 649
    }
}
