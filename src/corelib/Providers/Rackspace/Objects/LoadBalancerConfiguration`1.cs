namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerConfiguration<TNodeConfiguration>
        where TNodeConfiguration : NodeConfiguration
    {
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _name;

        [JsonProperty("nodes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private TNodeConfiguration[] _nodes;

        [JsonProperty("nodeCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int? _nodeCount;

        [JsonProperty("protocol", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _protocolName;

        [JsonProperty("halfClosed", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private bool? _halfClosed;

        [JsonProperty("virtualIps", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerVirtualAddress[] _virtualIps;

        [JsonProperty("accessList", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private object _accessList;

        [JsonProperty("algorithm", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancingAlgorithm _algorithm;

        [JsonProperty("connectionLogging", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerEnabledFlag _connectionLogging;

        [JsonProperty("contentCaching", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerEnabledFlag _contentCaching;

        [JsonProperty("connectionThrottle", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private ConnectionThrottles _connectionThrottle;

        [JsonProperty("healthMonitor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private object _healthMonitor;

        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private object _metadata;

        [JsonProperty("port", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int? _port;

        [JsonProperty("timeout", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int? _timeout;

        [JsonProperty("sessionPersistence", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private SessionPersistence _sessionPersistence;

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

        public LoadBalancerConfiguration(string name, IEnumerable<TNodeConfiguration> nodes, LoadBalancingProtocol protocol, IEnumerable<LoadBalancerVirtualAddress> virtualAddresses, bool? halfClosed = null, object accessList = null, LoadBalancingAlgorithm algorithm = null, bool? connectionLogging = null, bool? contentCaching = null, ConnectionThrottles connectionThrottle = null, object healthMonitor = null, object metadata = null, int? port = null, TimeSpan? timeout = null, SessionPersistence sessionPersistence = null)
        {
            _name = name;
            _nodes = nodes != null ? nodes.ToArray() : null;
            _protocolName = protocol != null ? protocol.Name : null;
            _port = protocol != null ? (int?)protocol.Port : null;
            _halfClosed = halfClosed;
            _virtualIps = virtualAddresses != null ? virtualAddresses.ToArray() : null;
            _accessList = accessList;
            _algorithm = algorithm;
            if (connectionLogging.HasValue)
                _connectionLogging = new LoadBalancerEnabledFlag(connectionLogging.Value);
            if (contentCaching.HasValue)
                _contentCaching = new LoadBalancerEnabledFlag(contentCaching.Value);
            _connectionThrottle = connectionThrottle;
            _healthMonitor = healthMonitor;
            _metadata = metadata;
            _timeout = timeout != null ? (int?)timeout.Value.TotalSeconds : null;
            _sessionPersistence = sessionPersistence;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
