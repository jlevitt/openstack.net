namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsSubdomain
    {
        [JsonProperty("emailAddress")]
        private string _emailAddress;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("id")]
        private string _id;

        [JsonProperty("comment")]
        private string _comment;

        [JsonProperty("created")]
        private DateTimeOffset? _created;

        [JsonProperty("updated")]
        private DateTimeOffset? _updated;
    }
}
