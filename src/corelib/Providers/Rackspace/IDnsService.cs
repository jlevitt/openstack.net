﻿namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Providers.Rackspace.Objects;

    /// <summary>
    /// Represents a provider for the Rackspace Cloud DNS service.
    /// </summary>
    /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/overview.html">Rackspace Cloud DNS Developer Guide - API v1.0</seealso>
    public interface IDnsService
    {
        #region Limits

        Task<DnsServiceLimits> ListLimitsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<LimitType>> ListLimitTypesAsync(CancellationToken cancellationToken);

        Task<DnsServiceLimits> ListLimitsAsync(LimitType type, CancellationToken cancellationToken);

        #endregion

        #region Jobs

        Task<DnsJob> GetJobStatus(string jobId, bool showDetails, CancellationToken cancellationToken);

        Task<DnsJob<TResponse>> GetJobStatus<TResponse>(string jobId, bool showDetails, CancellationToken cancellationToken);

        #endregion

        #region Domains

        Task<IEnumerable<DnsDomain>> ListDomainsAsync(string domainName, int? offset, int? limit, CancellationToken cancellationToken);

        Task<DnsDomain> ListDomainDetailsAsync(string domainId, bool showRecords, bool showSubdomains, CancellationToken cancellationToken);

        Task<DnsDomainChanges> ListDomainChangesAsync(string domainId, DateTimeOffset? since, CancellationToken cancellationToken);

        Task<DnsJob<ExportedDomain>> ExportDomainAsync(string domainId, DnsCompletionOption completionOption, CancellationToken cancellationToken);

        Task<DnsJob<DnsDomains>> CreateDomainsAsync(DnsConfiguration configuration, DnsCompletionOption completionOption, CancellationToken cancellationToken);

        Task<DnsJob<DnsDomains>> CloneDomainAsync(string domainId, string cloneName, bool? cloneSubdomains, bool? modifyRecordData, bool? modifyEmailAddress, bool? modifyComment, DnsCompletionOption completionOption, CancellationToken cancellationToken);

        Task<DnsJob<DnsDomains>> ImportDomainAsync(IEnumerable<SerializedDomain> serializedDomains, DnsCompletionOption completionOption, CancellationToken cancellationToken);

        Task<DnsJob> RemoveDomainsAsync(IEnumerable<string> domainIds, bool deleteSubdomains, DnsCompletionOption completionOption, CancellationToken cancellationToken);

        #endregion

        #region Subdomains

        Task<IEnumerable<DnsSubdomain>> ListSubdomainsAsync(string domainId, int? offset, int? limit, CancellationToken cancellationToken);

        #endregion
    }
}
