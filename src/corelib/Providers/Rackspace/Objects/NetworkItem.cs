namespace net.openstack.Providers.Rackspace.Objects
{
    using System.Net;
    using net.openstack.Core.Domain.Converters;
    using Newtonsoft.Json;

    public class NetworkItem
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        private string _id;

        [JsonProperty("address")]
        [JsonConverter(typeof(IPAddressSimpleConverter))]
        private IPAddress _address;

        [JsonProperty("type")]
        private AccessType _type;
    }
}
