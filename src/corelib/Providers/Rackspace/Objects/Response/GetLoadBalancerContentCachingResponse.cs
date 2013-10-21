namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class GetLoadBalancerContentCachingResponse
    {
        [JsonProperty("contentCaching")]
        private ContentCachingBody _body;

        [JsonConstructor]
        protected GetLoadBalancerContentCachingResponse()
        {
        }

        protected GetLoadBalancerContentCachingResponse(bool enabled)
        {
            _body = new ContentCachingBody(enabled);
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
        protected class ContentCachingBody
        {
            [JsonProperty("enabled")]
            private bool? _enabled;

            [JsonConstructor]
            protected ContentCachingBody()
            {
            }

            public ContentCachingBody(bool enabled)
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
