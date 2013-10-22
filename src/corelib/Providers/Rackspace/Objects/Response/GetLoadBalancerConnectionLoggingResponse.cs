namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class GetLoadBalancerConnectionLoggingResponse
    {
        [JsonProperty("connectionLogging")]
        private LoadBalancerEnabledFlag _body;

        [JsonConstructor]
        protected GetLoadBalancerConnectionLoggingResponse()
        {
        }

        protected GetLoadBalancerConnectionLoggingResponse(bool enabled)
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
