namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsDomain
    {
        [JsonProperty("emailAddress")]
        private string _emailAddress;

        [JsonProperty("updated")]
        private DateTimeOffset? _updated;

        [JsonProperty("created")]
        private DateTimeOffset? _created;

        [JsonProperty("accountId")]
        private string _accountId;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("id")]
        private string _id;

        [JsonProperty("comment")]
        private string _comment;

        [JsonProperty("nameservers")]
        private IEnumerable<DnsNameserver> _nameservers;

        [JsonProperty("ttl")]
        private int? _timeToLive;

        [JsonProperty("recordsList")]
        private RecordsList _recordsList;

        [JsonProperty("subdomains")]
        private SubdomainsList _subdomains;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }
        }

        public string Accountid
        {
            get
            {
                return _accountId;
            }
        }

        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
        }

        public DateTimeOffset? Created
        {
            get
            {
                return _created;
            }
        }

        public DateTimeOffset? Updated
        {
            get
            {
                return _updated;
            }
        }

        public ReadOnlyCollection<DnsNameserver> Nameservers
        {
            get
            {
                if (_nameservers == null)
                    return null;

                return new ReadOnlyCollection<DnsNameserver>(_nameservers.ToArray());
            }
        }

        public TimeSpan? TimeToLive
        {
            get
            {
                if (_timeToLive == null)
                    return null;

                return TimeSpan.FromSeconds(_timeToLive.Value);
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class RecordsList
        {
            [JsonProperty("records")]
            public IEnumerable<DnsRecord> Records;
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class SubdomainsList
        {
            [JsonProperty("domains")]
            public IEnumerable<DnsSubdomain> Subdomains;
        }
    }
}
