namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRecord
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("ttl")]
        private int? _timeToLive;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("id")]
        private string _id;

        [JsonProperty("type")]
        private DnsRecordType _type;

        [JsonProperty("comment")]
        private string _comment;

        [JsonProperty("data")]
        private string _data;

        [JsonProperty("priority")]
        private int? _priority;

        [JsonProperty("created")]
        private DateTimeOffset? _created;

        [JsonProperty("updated")]
        private DateTimeOffset? _updated;
#pragma warning restore 649

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public DnsRecordType Type
        {
            get
            {
                return _type;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Data
        {
            get
            {
                return _data;
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

        public int? Priority
        {
            get
            {
                return _priority;
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }
        }
    }
}
