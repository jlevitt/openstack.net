namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLoadBalancersResponse
    {
        [JsonProperty("loadBalancers")]
        private LoadBalancer[] _loadBalancers;

        public LoadBalancer[] LoadBalancers
        {
            get
            {
                return _loadBalancers;
            }
        }
    }
}
