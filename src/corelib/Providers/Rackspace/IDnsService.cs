namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Core;
    using net.openstack.Providers.Rackspace.Objects;

    /// <summary>
    /// Represents a provider for the Rackspace Cloud DNS service.
    /// </summary>
    /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/overview.html">Rackspace Cloud DNS Developer Guide - API v1.0</seealso>
    public interface IDnsService
    {
        #region Limits

        /// <summary>
        /// Get information about the provider-specific limits of this service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsServiceLimits"/> object containing detailed information about the limits for the service provider.</returns>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_All_Limits.html">List All Limits (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsServiceLimits> ListLimitsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get information about the types of provider-specific limits in place for this service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a collection of <see cref="LimitType"/> objects containing the limit types supported by the service.</returns>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Limit_Types.html">List Limit Types (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LimitType>> ListLimitTypesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get information about the provider-specific limits of this service for a particular <see cref="LimitType"/>.
        /// </summary>
        /// <param name="type">The limit type.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsServiceLimits"/> object containing detailed information about the limits of the specified <paramref name="type"/> for the service provider.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="type"/> is <c>null</c>.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Specific_Limit.html">List Specific Limit (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsServiceLimits> ListLimitsAsync(LimitType type, CancellationToken cancellationToken);

        #endregion

        #region Jobs

        Task<DnsJob> GetJobStatusAsync(DnsJob job, bool showDetails, CancellationToken cancellationToken);

        Task<DnsJob<TResponse>> GetJobStatusAsync<TResponse>(DnsJob<TResponse> job, bool showDetails, CancellationToken cancellationToken);

        #endregion

        #region Domains

        Task<Tuple<IEnumerable<DnsDomain>, int?>> ListDomainsAsync(string domainName, int? offset, int? limit, CancellationToken cancellationToken);

        Task<DnsDomain> ListDomainDetailsAsync(string domainId, bool showRecords, bool showSubdomains, CancellationToken cancellationToken);

        Task<DnsDomainChanges> ListDomainChangesAsync(string domainId, DateTimeOffset? since, CancellationToken cancellationToken);

        Task<DnsJob<ExportedDomain>> ExportDomainAsync(string domainId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<ExportedDomain>> progress);

        Task<DnsJob<DnsDomains>> CreateDomainsAsync(DnsConfiguration configuration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress);

        Task<DnsJob<DnsDomains>> CloneDomainAsync(string domainId, string cloneName, bool? cloneSubdomains, bool? modifyRecordData, bool? modifyEmailAddress, bool? modifyComment, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress);

        Task<DnsJob<DnsDomains>> ImportDomainAsync(IEnumerable<SerializedDomain> serializedDomains, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress);

        Task UpdateDomainsAsync();

        Task<DnsJob> RemoveDomainsAsync(IEnumerable<string> domainIds, bool deleteSubdomains, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress);

        #endregion

        #region Subdomains

        Task<Tuple<IEnumerable<DnsSubdomain>, int?>> ListSubdomainsAsync(string domainId, int? offset, int? limit, CancellationToken cancellationToken);

        #endregion

        #region Records

        Task<Tuple<IEnumerable<DnsRecord>, int?>> ListRecordsAsync(string domainId, string recordType, string recordName, string recordData, int? offset, int? limit, CancellationToken cancellationToken);

        Task<DnsRecord> ListRecordDetailsAsync(string domainId, string recordId, CancellationToken cancellationToken);

        Task<DnsJob<DnsRecordsList>> AddRecordsAsync(string domainId, IEnumerable<DnsDomainRecordConfiguration> recordConfigurations, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsRecordsList>> progress);

        Task UpdateRecordsAsync();

        Task<DnsJob> RemoveRecordsAsync(string domainId, IEnumerable<string> recordId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress);

        #endregion

        #region Reverse DNS

        Task<Tuple<IEnumerable<DnsRecord>, int?>> ListPtrRecordsAsync(string serviceName, Uri deviceResourceUri, int? offset, int? limit, CancellationToken cancellationToken);

        Task<DnsRecord> ListPtrRecordDetailsAsync(string serviceName, Uri deviceResourceUri, string recordId, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="deviceResourceUri"></param>
        /// <param name="recordConfigurations"></param>
        /// <param name="completionOption"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="serviceName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="deviceResourceUri"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="recordConfigurations"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="serviceName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="recordConfigurations"/> contains a record with a <see cref="DnsDomainRecordConfiguration.Type"/> that is not <see cref="DnsRecordType.Ptr"/>.</para>
        /// </exception>
        Task<DnsJob<DnsRecordsList>> AddPtrRecordsAsync(string serviceName, Uri deviceResourceUri, IEnumerable<DnsDomainRecordConfiguration> recordConfigurations, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsRecordsList>> progress);

        Task UpdatePtrRecordsAsync();

        Task<DnsJob> RemovePtrRecordsAsync(string serviceName, Uri deviceResourceUri, IPAddress ipAddress, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress);

        #endregion
    }
}
