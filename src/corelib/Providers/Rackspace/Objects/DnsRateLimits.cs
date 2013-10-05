namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRateLimits
    {
        [JsonProperty("rate")]
        private DnsRateLimitPattern[] _rate;
    }
}
