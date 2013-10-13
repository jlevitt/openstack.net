namespace net.openstack.Providers.Rackspace.Objects
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsDomains
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("domains")]
        private DnsDomain[] _domains;
#pragma warning restore 649

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
