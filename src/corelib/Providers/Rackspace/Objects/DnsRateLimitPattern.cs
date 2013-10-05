namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRateLimitPattern
    {
        [JsonProperty("uri")]
        private string _uri;

        [JsonProperty("regex")]
        private string _regex;

        [JsonProperty("limit")]
        private DnsRateLimit[] _limit;
    }
}
