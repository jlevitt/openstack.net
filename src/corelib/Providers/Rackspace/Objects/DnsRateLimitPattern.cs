namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRateLimitPattern
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("uri")]
        private string _uri;

        [JsonProperty("regex")]
        private string _regex;

        [JsonProperty("limit")]
        private DnsRateLimit[] _limit;
#pragma warning restore 649
    }
}
