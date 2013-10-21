namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLoadBalancingProtocolsResponse
    {
        [JsonProperty("protocols")]
        private LoadBalancingProtocol[] _protocols;

        public LoadBalancingProtocol[] Protocols
        {
            get
            {
                return _protocols;
            }
        }
    }
}
