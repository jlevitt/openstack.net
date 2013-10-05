namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsSubdomainConfiguration
    {
        [JsonProperty("emailAddress")]
        private string _emailAddress;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("comment")]
        private string _comment;

        public DnsSubdomainConfiguration(string emailAddress, string name, string comment)
        {
            _emailAddress = emailAddress;
            _name = name;
            _comment = comment;
        }

        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
        }

        public string Name
        {
            get
            {
                return _name;
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
