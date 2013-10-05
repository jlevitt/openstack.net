namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsDomainConfiguration
    {
        [JsonProperty("recordsList")]
        private RecordsList _recordsList;

        [JsonProperty("subdomains")]
        private SubdomainsList _subdomains;

        [JsonProperty("emailAddress")]
        private string _emailAddress;

        [JsonProperty("ttl")]
        private int? _timeToLive;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("comment")]
        private string _comment;

        public DnsDomainConfiguration(string name, TimeSpan? timeToLive, string emailAddress, string comment, IEnumerable<DnsDomainRecordConfiguration> records, IEnumerable<DnsSubdomainConfiguration> subdomains)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name cannot be empty");

            _name = name;
            _emailAddress = emailAddress;
            _comment = comment;
            if (timeToLive.HasValue)
                _timeToLive = (int)timeToLive.Value.TotalSeconds;

            _recordsList = new RecordsList(records);
            _subdomains = new SubdomainsList(subdomains);
        }

        private class RecordsList
        {
            [JsonProperty("records")]
            public IEnumerable<DnsDomainRecordConfiguration> Records;

            public RecordsList(IEnumerable<DnsDomainRecordConfiguration> records)
            {
                if (records == null)
                    throw new ArgumentNullException("records");

                Records = records.ToArray();
            }
        }

        private class SubdomainsList
        {
            [JsonProperty("domains")]
            public IEnumerable<DnsSubdomainConfiguration> Subdomains;

            public SubdomainsList(IEnumerable<DnsSubdomainConfiguration> subdomains)
            {
                if (subdomains == null)
                    throw new ArgumentNullException("subdomains");

                Subdomains = subdomains.ToArray();
            }
        }
    }
}
