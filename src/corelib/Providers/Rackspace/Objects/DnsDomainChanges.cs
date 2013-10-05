namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsDomainChanges
    {
        [JsonProperty("totalEntries")]
        private int _totalEntries;

        [JsonProperty("from")]
        private DateTimeOffset _from;

        [JsonProperty("to")]
        private DateTimeOffset _to;

        [JsonProperty("changes")]
        private DnsDomainChange[] _changes;

        public ReadOnlyCollection<DnsDomainChange> Changes
        {
            get
            {
                if (_changes == null)
                    return null;

                return new ReadOnlyCollection<DnsDomainChange>(_changes);
            }
        }
    }
}
