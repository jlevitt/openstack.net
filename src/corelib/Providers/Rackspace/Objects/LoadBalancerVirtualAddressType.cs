﻿namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Concurrent;
    using net.openstack.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a virtual address type in the load balancer service.
    /// </summary>
    /// <remarks>
    /// This class functions as a strongly-typed enumeration of known virtual address types,
    /// with added support for unknown types returned by a server extension.
    /// </remarks>
    /// <threadsafety static="true" instance="false"/>
    /// <preliminary/>
    [JsonConverter(typeof(LoadBalancerVirtualAddressType.Converter))]
    public sealed class LoadBalancerVirtualAddressType : ExtensibleEnum<LoadBalancerVirtualAddressType>
    {
        private static readonly ConcurrentDictionary<string, LoadBalancerVirtualAddressType> _types =
            new ConcurrentDictionary<string, LoadBalancerVirtualAddressType>(StringComparer.OrdinalIgnoreCase);
        private static readonly LoadBalancerVirtualAddressType _public = FromName("PUBLIC");
        private static readonly LoadBalancerVirtualAddressType _servicenet = FromName("SERVICENET");

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerVirtualAddressType"/> class with the specified name.
        /// </summary>
        /// <inheritdoc/>
        private LoadBalancerVirtualAddressType(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets the <see cref="LoadBalancerVirtualAddressType"/> instance with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name"/> is empty.</exception>
        public static LoadBalancerVirtualAddressType FromName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name cannot be empty");

            return _types.GetOrAdd(name, i => new LoadBalancerVirtualAddressType(i));
        }

        /// <summary>
        /// Gets a <see cref="LoadBalancerVirtualAddressType"/> representing <placeholder>placeholder</placeholder>.
        /// </summary>
        public static LoadBalancerVirtualAddressType Public
        {
            get
            {
                return _public;
            }
        }

        /// <summary>
        /// Gets a <see cref="LoadBalancerVirtualAddressType"/> representing <placeholder>placeholder</placeholder>.
        /// </summary>
        public static LoadBalancerVirtualAddressType ServiceNet
        {
            get
            {
                return _servicenet;
            }
        }

        /// <summary>
        /// Provides support for serializing and deserializing <see cref="LoadBalancerVirtualAddressType"/>
        /// objects to JSON string values.
        /// </summary>
        /// <threadsafety static="true" instance="false"/>
        /// <preliminary/>
        private sealed class Converter : ConverterBase
        {
            /// <inheritdoc/>
            protected override LoadBalancerVirtualAddressType FromName(string name)
            {
                return LoadBalancerVirtualAddressType.FromName(name);
            }
        }
    }
}
