namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Core;
    using net.openstack.Providers.Rackspace.Objects;
    using JsonSerializationException = Newtonsoft.Json.JsonSerializationException;

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
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsServiceLimits"/> object containing detailed information about the limits for the service provider.</returns>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_All_Limits.html">List All Limits (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsServiceLimits> ListLimitsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get information about the types of provider-specific limits in place for this service.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a collection of <see cref="LimitType"/> objects containing the limit types supported by the service.</returns>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Limit_Types.html">List Limit Types (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LimitType>> ListLimitTypesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get information about the provider-specific limits of this service for a particular <see cref="LimitType"/>.
        /// </summary>
        /// <param name="type">The limit type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsServiceLimits"/> object containing detailed information about the limits of the specified <paramref name="type"/> for the service provider.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Specific_Limit.html">List Specific Limit (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsServiceLimits> ListLimitsAsync(LimitType type, CancellationToken cancellationToken);

        #endregion

        #region Jobs

        /// <summary>
        /// Gets information about an asynchronous task being executed by the DNS service.
        /// </summary>
        /// <param name="job">The <see cref="DnsJob"/> to query.</param>
        /// <param name="showDetails"><c>true</c> to include detailed information about the job; otherwise, <c>false</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob"/> object containing the updated job information.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="job"/> is <c>null</c>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/sync_asynch_responses.html">Synchronous and Asynchronous Responses (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob> GetJobStatusAsync(DnsJob job, bool showDetails, CancellationToken cancellationToken);

        /// <summary>
        /// Gets information about an asynchronous task with a strongly-typed result being executed by the DNS service.
        /// </summary>
        /// <param name="job">The <see cref="DnsJob{TResponse}"/> to query.</param>
        /// <param name="showDetails"><c>true</c> to include detailed information about the job; otherwise, <c>false</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob{TResult}"/> object containing the updated job information.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="job"/> is <c>null</c>.</exception>
        /// <exception cref="JsonSerializationException">If an error occurs while deserializing the response object.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/sync_asynch_responses.html">Synchronous and Asynchronous Responses (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob<TResponse>> GetJobStatusAsync<TResponse>(DnsJob<TResponse> job, bool showDetails, CancellationToken cancellationToken);

        #endregion

        #region Domains

        /// <summary>
        /// Gets information about domains currently listed in the DNS service.
        /// </summary>
        /// <param name="domainName">If specified, the list will be filtered to only include the specified domain and its subdomains (if any exist).</param>
        /// <param name="offset">The index of the last item in the previous page of results. If not specified, the list starts at the beginning.</param>
        /// <param name="limit">The maximum number of domains to return in a single page.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob{TResult}"/> object
        /// containing a tuple of the resulting collection of <see cref="DnsDomain"/> objects and the total number of
        /// domains in the list. If the total number of domains in the list is not available, the second element of
        /// the tuple will be <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="offset"/> is less than 0.
        /// <para>-or-</para>
        /// <para>If <paramref name="limit"/> is less than or equal to 0.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/list_domains.html">List Domains (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/search_domains_w_filters.html">Search Domains with Filtering (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<Tuple<IEnumerable<DnsDomain>, int?>> ListDomainsAsync(string domainName, int? offset, int? limit, CancellationToken cancellationToken);

        /// <summary>
        /// Gets detailed information about a specific domain.
        /// </summary>
        /// <param name="domainId">The domain ID. This is obtained from <see cref="DnsDomain.Id">DnsDomain.Id</see>.</param>
        /// <param name="showRecords"><c>true</c> to populate the <see cref="DnsDomain.Records"/> property of the result; otherwise, <c>false</c>.</param>
        /// <param name="showSubdomains"><c>true</c> to populate the <see cref="DnsDomain.Subdomains"/> property of the result; otherwise, <c>false</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsDomain"/> object containing the DNS information for the requested domain.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="domainId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="domainId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/list_domain_details.html">List Domain Details (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsDomain> ListDomainDetailsAsync(string domainId, bool showRecords, bool showSubdomains, CancellationToken cancellationToken);

        /// <summary>
        /// Gets information about all changes made to a domain since a specified time.
        /// </summary>
        /// <param name="domainId">The domain ID. This is obtained from <see cref="DnsDomain.Id">DnsDomain.Id</see>.</param>
        /// <param name="since">The timestamp of the earliest changes to consider. If this is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully, the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsDomainChanges"/> object describing the changes made to a domain registered in the DNS service.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="domainId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="domainId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Domain_Changes.html">List Domain Changes (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsDomainChanges> ListDomainChangesAsync(string domainId, DateTimeOffset? since, CancellationToken cancellationToken);

        /// <summary>
        /// Exports a domain registered in the DNS service.
        /// </summary>
        /// <remarks>
        /// The exported domain represents a single domain, and does not include subdomains.
        ///
        /// <note>
        /// The <see cref="SerializedDomainFormat.Bind9"/> format does not support comments, so any
        /// comments associated with a domain or its records will not be included in the exported
        /// result.
        /// </note>
        /// </remarks>
        /// <param name="domainId">The domain ID. This is obtained from <see cref="DnsDomain.Id">DnsDomain.Id</see>.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="CompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob{TResponse}"/> object
        /// describing the asynchronous server operation. If <paramref name="completionOption"/> is
        /// <see cref="CompletionOption.RequestCompleted"/>, the job will additionally be in one of the following
        /// states.
        ///
        /// <list type="bullet">
        /// <item><see cref="DnsJobStatus.Completed"/>: In this case the <see cref="DnsJob{TResponse}.Response"/>
        /// property contains an <see cref="ExportedDomain"/> object containing the details of the exported domain.</item>
        /// <item><see cref="DnsJobStatus.Error"/>: In this case the <see cref="DnsJob.Error"/> property provides
        /// additional information about the error which occurred during the asynchronous server operation.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="domainId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="domainId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="completionOption"/> is not a valid <see cref="CompletionOption"/>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/export_domain.html">Export Domain (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob<ExportedDomain>> ExportDomainAsync(string domainId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<ExportedDomain>> progress);

        /// <summary>
        /// Registers one or more new domains in the DNS service.
        /// </summary>
        /// <param name="configuration">A <see cref="DnsConfiguration"/> object describing the domains to register in the DNS service.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="CompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob{TResponse}"/> object
        /// describing the asynchronous server operation. If <paramref name="completionOption"/> is
        /// <see cref="CompletionOption.RequestCompleted"/>, the job will additionally be in one of the following
        /// states.
        ///
        /// <list type="bullet">
        /// <item><see cref="DnsJobStatus.Completed"/>: In this case the <see cref="DnsJob{TResponse}.Response"/>
        /// property contains a <see cref="DnsDomains"/> object containing the details of the new domains.</item>
        /// <item><see cref="DnsJobStatus.Error"/>: In this case the <see cref="DnsJob.Error"/> property provides
        /// additional information about the error which occurred during the asynchronous server operation.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="configuration"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="completionOption"/> is not a valid <see cref="CompletionOption"/>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/create_domains.html">Create Domain(s) (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob<DnsDomains>> CreateDomainsAsync(DnsConfiguration configuration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress);

        /// <summary>
        /// Clones a domain registered in the DNS service, optionally cloning its subdomains as well.
        /// </summary>
        /// <param name="domainId">The domain ID. This is obtained from <see cref="DnsDomain.Id">DnsDomain.Id</see>.</param>
        /// <param name="cloneName">The name of the new (cloned) domain.</param>
        /// <param name="cloneSubdomains"><c>true</c> to recursively clone subdomains; otherwise, <c>false</c> to only clone the top-level domain and its records. Cloned subdomain configurations are modified the same way that cloned top-level domain configurations are modified. If this is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="modifyRecordData"><c>true</c> to replace occurrences of the reference domain name with the new domain name in comments on the cloned (new) domain. If this is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="modifyEmailAddress"><c>true</c> to replace occurrences of the reference domain name with the new domain name in email addresses on the cloned (new) domain. If this is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="modifyComment"><true>true</true> to replace occurrences of the reference domain name with the new domain name in data fields (of records) on the cloned (new) domain. Does not affect NS records. If this is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="CompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob{TResponse}"/> object
        /// describing the asynchronous server operation. If <paramref name="completionOption"/> is
        /// <see cref="CompletionOption.RequestCompleted"/>, the job will additionally be in one of the following
        /// states.
        ///
        /// <list type="bullet">
        /// <item><see cref="DnsJobStatus.Completed"/>: In this case the <see cref="DnsJob{TResponse}.Response"/>
        /// property contains a <see cref="DnsDomains"/> object containing the details of the cloned (new) domains.</item>
        /// <item><see cref="DnsJobStatus.Error"/>: In this case the <see cref="DnsJob.Error"/> property provides
        /// additional information about the error which occurred during the asynchronous server operation.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="domainId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="cloneName"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="domainId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="cloneName"/> is empty.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="completionOption"/> is not a valid <see cref="CompletionOption"/>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/clone_domain-dle846.html">Clone Domain (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob<DnsDomains>> CloneDomainAsync(string domainId, string cloneName, bool? cloneSubdomains, bool? modifyRecordData, bool? modifyEmailAddress, bool? modifyComment, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress);

        /// <summary>
        /// Imports domains into the DNS service.
        /// </summary>
        /// <param name="serializedDomains">A collection of <see cref="SerializedDomain"/> objects containing the serialized domain information to import.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="CompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob{TResponse}"/> object
        /// describing the asynchronous server operation. If <paramref name="completionOption"/> is
        /// <see cref="CompletionOption.RequestCompleted"/>, the job will additionally be in one of the following
        /// states.
        ///
        /// <list type="bullet">
        /// <item><see cref="DnsJobStatus.Completed"/>: In this case the <see cref="DnsJob{TResponse}.Response"/>
        /// property contains a <see cref="DnsDomains"/> object containing the details of the imported domains.</item>
        /// <item><see cref="DnsJobStatus.Error"/>: In this case the <see cref="DnsJob.Error"/> property provides
        /// additional information about the error which occurred during the asynchronous server operation.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="serializedDomains"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="serializedDomains"/> is contains any <c>null</c> values.
        /// <para>-or-</para>
        /// <para>If <paramref name="completionOption"/> is not a valid <see cref="CompletionOption"/>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/import_domain.html">Import Domain (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob<DnsDomains>> ImportDomainAsync(IEnumerable<SerializedDomain> serializedDomains, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress);

        /// <summary>
        /// Removes one or more domains from the DNS service.
        /// </summary>
        /// <param name="domainIds">A collection of IDs for the domains to remove. These are obtained from <see cref="DnsDomain.Id">DnsDomain.Id</see>.</param>
        /// <param name="deleteSubdomains"><c>true</c> to delete any subdomains associated with the specified domains; otherwise, <c>false</c> to promote any subdomains to top-level domains.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="CompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="DnsJob"/> object
        /// describing the asynchronous server operation. If <paramref name="completionOption"/> is
        /// <see cref="CompletionOption.RequestCompleted"/>, the job will additionally be in one of the following
        /// states.
        ///
        /// <list type="bullet">
        /// <item><see cref="DnsJobStatus.Completed"/></item>
        /// <item><see cref="DnsJobStatus.Error"/>: In this case the <see cref="DnsJob.Error"/> property provides
        /// additional information about the error which occurred during the asynchronous server operation.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="domainIds"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="domainIds"/> is contains any <c>null</c> or empty values.
        /// <para>-or-</para>
        /// <para>If <paramref name="completionOption"/> is not a valid <see cref="CompletionOption"/>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/Remove_Domain_s_-d1e4022.html">Remove Domain(s) (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        Task<DnsJob> RemoveDomainsAsync(IEnumerable<string> domainIds, bool deleteSubdomains, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress);

        #endregion

        #region Subdomains

        Task<Tuple<IEnumerable<DnsSubdomain>, int?>> ListSubdomainsAsync(string domainId, int? offset, int? limit, CancellationToken cancellationToken);

        #endregion

        #region Records

        Task<Tuple<IEnumerable<DnsRecord>, int?>> ListRecordsAsync(string domainId, string recordType, string recordName, string recordData, int? offset, int? limit, CancellationToken cancellationToken);

        Task<DnsRecord> ListRecordDetailsAsync(string domainId, string recordId, CancellationToken cancellationToken);

        Task<DnsJob<DnsRecordsList>> AddRecordsAsync(string domainId, IEnumerable<DnsDomainRecordConfiguration> recordConfigurations, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsRecordsList>> progress);

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

        Task<DnsJob> RemovePtrRecordsAsync(string serviceName, Uri deviceResourceUri, IPAddress ipAddress, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress);

        #endregion
    }
}
