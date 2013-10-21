namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    /// <summary>
    /// This models the JSON object representing a load balancing protocol.
    /// </summary>
    /// <seealso cref="ILoadBalancerService.ListProtocolsAsync"/>
    /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancing_Protocols-d1e4269.html">List Load Balancing Protocols (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancingProtocol
    {
        [JsonProperty("name")]
        private string _name;

        [JsonProperty("port")]
        private int _port;

        [JsonConstructor]
        private LoadBalancingProtocol()
        {
        }

        /// <summary>
        /// Gets the name of the load balancing protocol.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the port for the load balancing protocol.
        /// </summary>
        /// <value>The default port number used for the protocol, or 0 if no default port is defined for the protocol.</value>
        public int Port
        {
            get
            {
                return _port;
            }
        }
    }
}
