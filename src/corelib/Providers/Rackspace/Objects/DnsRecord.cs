namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsRecord
    {
        [JsonProperty("ttl")]
        private int _ttl;

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
    }
}
