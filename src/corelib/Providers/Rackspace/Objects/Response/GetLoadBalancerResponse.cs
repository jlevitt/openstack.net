namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class GetLoadBalancerResponse
    {
        [JsonProperty("loadBalancer")]
        private LoadBalancer _loadBalancer;

        public LoadBalancer LoadBalancer
        {
            get
            {
                return _loadBalancer;
            }
        }
    }
}
