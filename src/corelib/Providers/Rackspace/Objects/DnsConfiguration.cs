namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsConfiguration
    {
        [JsonProperty("domains")]
        private IEnumerable<DnsDomainConfiguration> _domainConfiguration;

        public DnsConfiguration(params DnsDomainConfiguration[] domainConfigurations)
            : this(domainConfigurations.AsEnumerable())
        {
        }

        public DnsConfiguration(IEnumerable<DnsDomainConfiguration> domainConfigurations)
        {
            if (domainConfigurations == null)
                throw new ArgumentNullException("domainConfigurations");

            _domainConfiguration = domainConfigurations.ToArray();
        }
    }
}
