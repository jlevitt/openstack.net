namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Concurrent;
    using net.openstack.Core;
    using net.openstack.Core.Domain.Converters;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a load balancing algorithm.
    /// </summary>
    /// <remarks>
    /// This class functions as a strongly-typed enumeration of known load balancing
    /// algorithms, with added support for unknown algorithms returned by a server
    /// extension.
    /// </remarks>
    /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancing_Algorithms-d1e4459.html">List Load Balancing Algorithms (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
    /// <threadsafety static="true" instance="false"/>
    [JsonConverter(typeof(LoadBalancingAlgorithm.Converter))]
    public sealed class LoadBalancingAlgorithm : ExtensibleEnum<LoadBalancingAlgorithm>
    {
        private static readonly ConcurrentDictionary<string, LoadBalancingAlgorithm> _states =
            new ConcurrentDictionary<string, LoadBalancingAlgorithm>(StringComparer.OrdinalIgnoreCase);
        private static readonly LoadBalancingAlgorithm _leastConnections = FromName("LEAST_CONNECTIONS");
        private static readonly LoadBalancingAlgorithm _random = FromName("RANDOM");
        private static readonly LoadBalancingAlgorithm _roundRobin = FromName("ROUND_ROBIN");
        private static readonly LoadBalancingAlgorithm _weightedLeastConnections = FromName("WEIGHTED_LEAST_CONNECTIONS");
        private static readonly LoadBalancingAlgorithm _weightedRoundRobin = FromName("WEIGHTED_ROUND_ROBIN");

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancingAlgorithm"/> class with the specified name.
        /// </summary>
        /// <inheritdoc/>
        private LoadBalancingAlgorithm(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets the <see cref="LoadBalancingAlgorithm"/> instance with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name"/> is empty.</exception>
        public static LoadBalancingAlgorithm FromName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name cannot be empty");

            return _states.GetOrAdd(name, i => new LoadBalancingAlgorithm(i));
        }

        public static LoadBalancingAlgorithm LeastConnections
        {
            get
            {
                return _leastConnections;
            }
        }

        public static LoadBalancingAlgorithm Random
        {
            get
            {
                return _random;
            }
        }

        public static LoadBalancingAlgorithm RoundRobin
        {
            get
            {
                return _roundRobin;
            }
        }

        public static LoadBalancingAlgorithm WeightedLeastConnections
        {
            get
            {
                return _weightedLeastConnections;
            }
        }

        public static LoadBalancingAlgorithm WeightedRoundRobin
        {
            get
            {
                return _weightedRoundRobin;
            }
        }

        /// <summary>
        /// Provides support for serializing and deserializing <see cref="LoadBalancingAlgorithm"/>
        /// objects to JSON string values.
        /// </summary>
        /// <threadsafety static="true" instance="false"/>
        private sealed class Converter : ConverterBase
        {
            /// <inheritdoc/>
            protected override LoadBalancingAlgorithm FromName(string name)
            {
                return LoadBalancingAlgorithm.FromName(name);
            }
        }
    }
}
