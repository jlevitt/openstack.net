namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerConfiguration : LoadBalancerConfiguration<NodeConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerConfiguration"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is used during JSON deserialization of derived types.
        /// </remarks>
        [JsonConstructor]
        protected LoadBalancerConfiguration()
        {
        }

        public LoadBalancerConfiguration(string name, IEnumerable<NodeConfiguration> nodes, LoadBalancingProtocol protocol, IEnumerable<LoadBalancerVirtualAddress> virtualAddresses, bool? halfClosed = null, object accessList = null, LoadBalancingAlgorithm algorithm = null, bool? connectionLogging = null, bool? contentCaching = null, ConnectionThrottles connectionThrottle = null, object healthMonitor = null, object metadata = null, int? port = null, TimeSpan? timeout = null, SessionPersistence sessionPersistence = null)
            : base(name, nodes, protocol, virtualAddresses, halfClosed, accessList, algorithm, connectionLogging, contentCaching, connectionThrottle, healthMonitor, metadata, port, timeout, sessionPersistence)
        {
        }
    }
}
