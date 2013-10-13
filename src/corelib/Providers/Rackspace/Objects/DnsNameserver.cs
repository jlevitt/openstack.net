namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsNameserver
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("name")]
        private string _name;
#pragma warning restore 649

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
