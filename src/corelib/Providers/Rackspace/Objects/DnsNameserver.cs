namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsNameserver
    {
        [JsonProperty("name")]
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
