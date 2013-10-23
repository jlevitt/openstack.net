namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using net.openstack.Core.Domain.Converters;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerVirtualAddress
    {
        [JsonProperty("address", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(IPAddressSimpleConverter))]
        private IPAddress _address;

        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _id;

        [JsonProperty("type")]
        private LoadBalancerVirtualAddressType _type;

        [JsonProperty("ipVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _ipVersion;

        [JsonConstructor]
        protected LoadBalancerVirtualAddress()
        {
        }

        public LoadBalancerVirtualAddress(LoadBalancerVirtualAddressType type)
            : this(type, null)
        {
        }

        public LoadBalancerVirtualAddress(LoadBalancerVirtualAddressType type, AddressFamily? version)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            _type = type;
            if (version.HasValue)
            {
                switch (version)
                {
                case AddressFamily.InterNetwork:
                    _ipVersion = "IPV4";
                    break;

                case AddressFamily.InterNetworkV6:
                    _ipVersion = "IPV6";
                    break;

                default:
                    throw new NotSupportedException("The specified address family is not supported by this service.");
                }
            }
        }
    }
}
