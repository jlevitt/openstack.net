namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLoadBalancingAlgorithmsResponse
    {
        [JsonProperty("algorithms")]
        private SerializedLoadBalancingAlgorithm[] _algorithms;

        public IEnumerable<LoadBalancingAlgorithm> Algorithms
        {
            get
            {
                return _algorithms.Select(i => i._name);
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        private class SerializedLoadBalancingAlgorithm
        {
            [JsonProperty("name")]
            public LoadBalancingAlgorithm _name;
        }
    }
}
