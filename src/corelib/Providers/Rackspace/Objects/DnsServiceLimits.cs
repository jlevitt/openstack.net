namespace net.openstack.Providers.Rackspace.Objects
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsServiceLimits
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("rate")]
        private DnsRateLimitPattern[] _rate;

        [JsonProperty("absolute")]
        private Dictionary<string, long> _absolute;
#pragma warning restore 649

        public ReadOnlyCollection<DnsRateLimitPattern> RateLimits
        {
            get
            {
                return new ReadOnlyCollection<DnsRateLimitPattern>(_rate);
            }
        }

        public Dictionary<string, long> AbsoluteLimits
        {
            get
            {
                return _absolute;
            }
        }
    }
}
