namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class GetLoadBalancerContentCachingResponse
    {
        [JsonProperty("contentCaching")]
        private LoadBalancerEnabledFlag _body;

        [JsonConstructor]
        protected GetLoadBalancerContentCachingResponse()
        {
        }

        protected GetLoadBalancerContentCachingResponse(bool enabled)
        {
            _body = new LoadBalancerEnabledFlag(enabled);
        }

        public bool? Enabled
        {
            get
            {
                if (_body == null)
                    return null;

                return _body.Enabled;
            }
        }
    }
}
