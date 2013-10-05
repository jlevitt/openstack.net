namespace net.openstack.Providers.Rackspace.Objects
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsDomains
    {
        [JsonProperty("domains")]
        private DnsDomain[] _domains;

        public ReadOnlyCollection<DnsDomain> Domains
        {
            get
            {
                if (_domains == null)
                    return null;

                return new ReadOnlyCollection<DnsDomain>(_domains);
            }
        }
    }
}
