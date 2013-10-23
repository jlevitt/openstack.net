namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using net.openstack.Core.Domain.Converters;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class NodeConfiguration
    {
        [JsonProperty("address")]
        [JsonConverter(typeof(IPAddressSimpleConverter))]
        private IPAddress _address;

        [JsonProperty("port")]
        private int? _port;

        [JsonProperty("condition")]
        private NodeCondition _condition;

        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private NodeType _type;

        [JsonProperty("weight", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int? _weight;
    }
}
