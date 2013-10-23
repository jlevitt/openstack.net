namespace net.openstack.Providers.Rackspace.Objects.Request
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class CreateLoadBalancerRequest
    {
        [JsonProperty("loadBalancer")]
        private LoadBalancerConfiguration _configuration;

        public CreateLoadBalancerRequest(LoadBalancerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }
    }
}
