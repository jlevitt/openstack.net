namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLoadBalancerThrottlesResponse
    {
        [JsonProperty("connectionThrottle")]
        private ConnectionThrottles _throttles;

        public ConnectionThrottles Throttles
        {
            get
            {
                return _throttles;
            }
        }
    }
}
