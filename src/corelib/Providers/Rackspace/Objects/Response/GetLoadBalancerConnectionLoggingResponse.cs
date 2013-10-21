namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class GetLoadBalancerConnectionLoggingResponse
    {
        [JsonProperty("connectionLogging")]
        private ConnectionLoggingBody _body;

        [JsonConstructor]
        protected GetLoadBalancerConnectionLoggingResponse()
        {
        }

        protected GetLoadBalancerConnectionLoggingResponse(bool enabled)
        {
            _body = new ConnectionLoggingBody(enabled);
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

        [JsonObject(MemberSerialization.OptIn)]
        protected class ConnectionLoggingBody
        {
            [JsonProperty("enabled")]
            private bool? _enabled;

            [JsonConstructor]
            protected ConnectionLoggingBody()
            {
            }

            public ConnectionLoggingBody(bool enabled)
            {
                _enabled = enabled;
            }

            public bool? Enabled
            {
                get
                {
                    return _enabled;
                }
            }
        }
    }
}
