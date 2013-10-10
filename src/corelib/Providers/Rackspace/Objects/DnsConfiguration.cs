﻿namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the configuration of a collection of domains being added to the DNS service.
    /// </summary>
    /// <seealso cref="IDnsService.CreateDomainsAsync"/>
    /// <preliminary/>
    [JsonObject(MemberSerialization.OptIn)]
    public class DnsConfiguration
    {
        /// <summary>
        /// This is the backing field for the <see cref="DomainConfigurations"/> property.
        /// </summary>
        [JsonProperty("domains")]
        private readonly DnsDomainConfiguration[] _domainConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsConfiguration"/> class for the
        /// specified domains.
        /// </summary>
        /// <param name="domainConfigurations">A collection of <see cref="DnsDomainConfiguration"/> objects describing the domains.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="domainConfigurations"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="domainConfigurations"/> contains a <c>null</c> value.</exception>
        public DnsConfiguration(params DnsDomainConfiguration[] domainConfigurations)
            : this(domainConfigurations.AsEnumerable())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsConfiguration"/> class for the
        /// specified domains.
        /// </summary>
        /// <param name="domainConfigurations">A collection of <see cref="DnsDomainConfiguration"/> objects describing the domains.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="domainConfigurations"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="domainConfigurations"/> contains a <c>null</c> value.</exception>
        public DnsConfiguration(IEnumerable<DnsDomainConfiguration> domainConfigurations)
        {
            if (domainConfigurations == null)
                throw new ArgumentNullException("domainConfigurations");

            _domainConfiguration = domainConfigurations.ToArray();

            if (_domainConfiguration.Contains(null))
                throw new ArgumentException("domainConfigurations cannot contain any null values.", "domainConfigurations");
        }

        /// <summary>
        /// Gets a collection of the <see cref="DnsDomainConfiguration"/> objects describing
        /// domains in this configuration.
        /// </summary>
        public ReadOnlyCollection<DnsDomainConfiguration> DomainConfigurations
        {
            get
            {
                return new ReadOnlyCollection<DnsDomainConfiguration>(_domainConfiguration);
            }
        }
    }
}
