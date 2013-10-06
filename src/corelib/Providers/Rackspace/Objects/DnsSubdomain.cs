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

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
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
    }
}
