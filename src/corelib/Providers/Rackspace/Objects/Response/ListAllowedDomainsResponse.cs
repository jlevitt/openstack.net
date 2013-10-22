namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class ListAllowedDomainsResponse
    {
        [JsonProperty("allowedDomains")]
        private AllowedDomain[] _allowedDomains;

        public IEnumerable<string> AllowedDomains
        {
            get
            {
                if (_allowedDomains == null)
                    return null;

                return _allowedDomains.Select(i => i.Name);
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        protected class AllowedDomain
        {
            [JsonProperty("allowedDomain")]
            private AllowedDomainDescriptor _allowedDomain;

            public string Name
            {
                get
                {
                    if (_allowedDomain == null)
                        return null;

                    return _allowedDomain.Name;
                }
            }

            [JsonObject(MemberSerialization.OptIn)]
            protected class AllowedDomainDescriptor
            {
                [JsonProperty("name")]
                private string _name;

                public string Name
                {
                    get
                    {
                        return _name;
                    }
                }
            }
        }
    }
}
