namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using JSIStudios.SimpleRESTServices.Client;
    using net.openstack.Core;
    using net.openstack.Core.Domain;
    using net.openstack.Core.Providers;
    using net.openstack.Providers.Rackspace.Objects;
    using net.openstack.Providers.Rackspace.Validators;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using CancellationToken = System.Threading.CancellationToken;
    using JsonRequestSettings = JSIStudios.SimpleRESTServices.Client.Json.JsonRequestSettings;
    using Stream = System.IO.Stream;
    using StreamReader = System.IO.StreamReader;
    using Thread = System.Threading.Thread;

    public class CloudDnsProvider : ProviderBase<IDnsService>, IDnsService
    {
        /// <summary>
        /// Specifies whether the <see cref="Endpoint.PublicURL"/> or <see cref="Endpoint.InternalURL"/>
        /// should be used for accessing the Cloud DNS API.
        /// </summary>
        private readonly bool _internalUrl;

        /// <summary>
        /// This field caches the base URI used for accessing the Cloud DNS service.
        /// </summary>
        /// <seealso cref="GetBaseUriAsync"/>
        private Uri _baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudDnsProvider"/> class with
        /// the specified values.
        /// </summary>
        /// <param name="defaultIdentity">The default identity to use for calls that do not explicitly specify an identity. If this value is <c>null</c>, no default identity is available so all calls must specify an explicit identity.</param>
        /// <param name="defaultRegion">The default region to use for calls that do not explicitly specify a region. If this value is <c>null</c>, the default region for the user will be used; otherwise if the service uses region-specific endpoints all calls must specify an explicit region.</param>
        /// <param name="internalUrl"><c>true</c> to use the endpoint's <see cref="Endpoint.InternalURL"/>; otherwise <c>false</c> to use the endpoint's <see cref="Endpoint.PublicURL"/>.</param>
        /// <param name="identityProvider">The identity provider to use for authenticating requests to this provider. If this value is <c>null</c>, a new instance of <see cref="CloudIdentityProvider"/> is created using <paramref name="defaultIdentity"/> as the default identity.</param>
        /// <param name="restService">The implementation of <see cref="IRestService"/> to use for executing REST requests. If this value is <c>null</c>, the provider will use a new instance of <see cref="JsonRestServices"/>.</param>
        public CloudDnsProvider(CloudIdentity defaultIdentity, string defaultRegion, bool internalUrl, IIdentityProvider identityProvider, IRestService restService)
            : base(defaultIdentity, defaultRegion, identityProvider, restService, HttpResponseCodeValidator.Default)
        {
            _internalUrl = internalUrl;
        }

        #region IDnsService Members

        /// <inheritdoc/>
        public Task<DnsServiceLimits> ListLimitsAsync(CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/limits");
            var parameters = new Dictionary<string, string>();

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsServiceLimits> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken limits = result["limits"];
                    if (limits == null)
                        return null;

                    return limits.ToObject<DnsServiceLimits>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LimitType>> ListLimitTypesAsync(CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/limits/types");
            var parameters = new Dictionary<string, string>();

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, IEnumerable<LimitType>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken limitTypes = result["limitTypes"];
                    if (limitTypes == null)
                        return null;

                    return limitTypes.ToObject<LimitType[]>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsServiceLimits> ListLimitsAsync(LimitType type, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/limits/{type}");
            var parameters = new Dictionary<string, string>
                {
                    { "type", type.Name.ToLowerInvariant() }
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsServiceLimits> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken limits = result["limits"];
                    if (limits == null)
                        return null;

                    return limits.ToObject<DnsServiceLimits>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob> GetJobStatusAsync(DnsJob job, bool showDetails, CancellationToken cancellationToken)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            UriTemplate template = new UriTemplate("/status/{jobId}?showDetails={showDetails}");
            var parameters = new Dictionary<string, string>
                {
                    { "jobId", job.Id },
                    { "showDetails", showDetails ? "true" : "false" },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    return result.ToObject<DnsJob>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<TResult>> GetJobStatusAsync<TResult>(DnsJob<TResult> job, bool showDetails, CancellationToken cancellationToken)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            UriTemplate template = new UriTemplate("/status/{jobId}?showDetails={showDetails}");
            var parameters = new Dictionary<string, string>
                {
                    { "jobId", job.Id },
                    { "showDetails", showDetails ? "true" : "false" },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<TResult>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    return result.ToObject<DnsJob<TResult>>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<Tuple<IEnumerable<DnsDomain>, int?>> ListDomainsAsync(string domainName, int? offset, int? limit, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/domains/?name={name}&offset={offset}&limit={limit}");
            var parameters = new Dictionary<string, string>();
            if (domainName != null)
                parameters.Add("name", domainName);
            if (offset != null)
                parameters.Add("offset", offset.ToString());
            if (limit != null)
                parameters.Add("limit", limit.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, Tuple<IEnumerable<DnsDomain>, int?>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken domains = result["domains"];
                    if (domains == null)
                        return null;

                    int? totalEntries = null;
                    JToken totalEntriesToken = result["totalEntries"];
                    if (totalEntriesToken != null)
                        totalEntries = totalEntriesToken.ToObject<int>();

                    return Tuple.Create(domains.ToObject<IEnumerable<DnsDomain>>(), totalEntries);
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsDomain> ListDomainDetailsAsync(string domainId, bool showRecords, bool showSubdomains, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}?showRecords={showRecords}&showSubdomains={showSubdomains}");
            var parameters = new Dictionary<string, string>()
            {
                { "domainId", domainId },
                { "showRecords", showRecords ? "true" : "false" },
                { "showSubdomains", showSubdomains ? "true" : "false" },
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsDomain> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    return result.ToObject<DnsDomain>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsDomainChanges> ListDomainChangesAsync(string domainId, DateTimeOffset? since, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/changes?since={since}");
            var parameters = new Dictionary<string, string>()
                {
                    { "domainId", domainId },
                };
            if (since.HasValue)
                parameters.Add("since", since.Value.ToString("G"));

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsDomainChanges> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    return result.ToObject<DnsDomainChanges>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<ExportedDomain>> ExportDomainAsync(string domainId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<ExportedDomain>> progress)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/export");
            var parameters = new Dictionary<string, string>()
                {
                    { "domainId", domainId },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<ExportedDomain>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob<ExportedDomain> job = task.Result.ToObject<DnsJob<ExportedDomain>>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<DnsDomains>> CreateDomainsAsync(DnsConfiguration configuration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress)
        {
            UriTemplate template = new UriTemplate("/domains");
            var parameters = new Dictionary<string, string>();

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, configuration);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<DnsDomains>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob<DnsDomains> job = task.Result.ToObject<DnsJob<DnsDomains>>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<DnsDomains>> CloneDomainAsync(string domainId, string cloneName, bool? cloneSubdomains, bool? modifyRecordData, bool? modifyEmailAddress, bool? modifyComment, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/clone?cloneName={cloneName}&cloneSubdomains={cloneSubdomains}&modifyRecordData={modifyRecordData}&modifyEmailAddress={modifyEmailAddress}&modifyComment={modifyComment}");
            var parameters = new Dictionary<string, string> { { "domainId", domainId }, { "cloneName", cloneName } };
            if (cloneSubdomains != null)
                parameters.Add("cloneSubdomains", cloneSubdomains.Value ? "true" : "false");
            if (modifyRecordData != null)
                parameters.Add("modifyRecordData", modifyRecordData.Value ? "true" : "false");
            if (modifyEmailAddress != null)
                parameters.Add("modifyEmailAddress", modifyEmailAddress.Value ? "true" : "false");
            if (modifyComment != null)
                parameters.Add("modifyComment", modifyComment.Value ? "true" : "false");

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<DnsDomains>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob<DnsDomains> job = task.Result.ToObject<DnsJob<DnsDomains>>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<DnsDomains>> ImportDomainAsync(IEnumerable<SerializedDomain> serializedDomains, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsDomains>> progress)
        {
            if (serializedDomains == null)
                throw new ArgumentNullException("serializedDomains");

            UriTemplate template = new UriTemplate("/domains/import");
            var parameters = new Dictionary<string, string>();

            JObject request = new JObject(new JProperty("domains", JArray.FromObject(serializedDomains)));
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, request);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<DnsDomains>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob<DnsDomains> job = task.Result.ToObject<DnsJob<DnsDomains>>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task UpdateDomainsAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DnsJob> RemoveDomainsAsync(IEnumerable<string> domainIds, bool deleteSubdomains, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress)
        {
            UriTemplate template = new UriTemplate("/domains?deleteSubdomains={deleteSubdomains}");
            var parameters = new Dictionary<string, string>()
            {
                { "deleteSubdomains", deleteSubdomains ? "true" : "false" },
            };

            Func<Uri, Uri> transform =
                uri =>
                {
                    UriBuilder builder = new UriBuilder(uri);
                    builder.Query = builder.Query.TrimStart('?') + string.Concat(domainIds.Select(domainId => "&id=" + domainId));
                    return builder.Uri;
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters, transform);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob job = task.Result.ToObject<DnsJob>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<Tuple<IEnumerable<DnsSubdomain>, int?>> ListSubdomainsAsync(string domainId, int? offset, int? limit, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/subdomains?offset={offset}&limit={limit}");
            var parameters = new Dictionary<string, string>
                {
                    { "domainId", domainId }
                };
            if (offset != null)
                parameters.Add("offset", offset.ToString());
            if (limit != null)
                parameters.Add("limit", limit.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, Tuple<IEnumerable<DnsSubdomain>, int?>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken domains = result["domains"];
                    if (domains == null)
                        return null;

                    int? totalEntries = null;
                    JToken totalEntriesToken = result["totalEntries"];
                    if (totalEntriesToken != null)
                        totalEntries = totalEntriesToken.ToObject<int>();

                    return Tuple.Create(domains.ToObject<IEnumerable<DnsSubdomain>>(), totalEntries);
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<Tuple<IEnumerable<DnsRecord>, int?>> ListRecordsAsync(string domainId, string recordType, string recordName, string recordData, int? offset, int? limit, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/records?type={recordType}&name={recordName}&data={recordData}&offset={offset}&limit={limit}");
            var parameters = new Dictionary<string, string>
                {
                    { "domainId", domainId }
                };
            if (recordType != null)
                parameters.Add("recordType", recordType);
            if (recordName != null)
                parameters.Add("recordName", recordName);
            if (recordData != null)
                parameters.Add("recordData", recordData);
            if (offset != null)
                parameters.Add("offset", offset.ToString());
            if (limit != null)
                parameters.Add("limit", limit.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, Tuple<IEnumerable<DnsRecord>, int?>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken records = result["records"];
                    if (records == null)
                        return null;

                    int? totalEntries = null;
                    JToken totalEntriesToken = result["totalEntries"];
                    if (totalEntriesToken != null)
                        totalEntries = totalEntriesToken.ToObject<int>();

                    return Tuple.Create(records.ToObject<IEnumerable<DnsRecord>>(), totalEntries);
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsRecord> ListRecordDetailsAsync(string domainId, string recordId, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/records/{recordId}");
            var parameters = new Dictionary<string, string>
                {
                    { "domainId", domainId },
                    { "recordId", recordId }
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsRecord> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    return result.ToObject<DnsRecord>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<DnsRecordsList>> AddRecordsAsync(string domainId, IEnumerable<DnsDomainRecordConfiguration> recordConfigurations, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsRecordsList>> progress)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/records");
            var parameters = new Dictionary<string, string>()
                {
                    { "domainId", domainId }
                };

            JObject request = new JObject(new JProperty("records", JArray.FromObject(recordConfigurations)));
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, request);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<DnsRecordsList>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob<DnsRecordsList> job = task.Result.ToObject<DnsJob<DnsRecordsList>>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task UpdateRecordsAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DnsJob> RemoveRecordsAsync(string domainId, IEnumerable<string> recordIds, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress)
        {
            UriTemplate template = new UriTemplate("/domains/{domainId}/records");
            var parameters = new Dictionary<string, string>()
            {
                { "domainId", domainId },
            };

            Func<Uri, Uri> transform =
                uri =>
                {
                    UriBuilder builder = new UriBuilder(uri);
                    builder.Query = builder.Query.TrimStart('?') + string.Concat(recordIds.Select(recordId => "&id=" + recordId));
                    return builder.Uri;
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters, transform);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob job = task.Result.ToObject<DnsJob>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<Tuple<IEnumerable<DnsRecord>, int?>> ListPtrRecordsAsync(string serviceName, Uri deviceResourceUri, int? offset, int? limit, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/rdns/{serviceName}?href={deviceResourceUri}&offset={offset}&limit={limit}");
            var parameters = new Dictionary<string, string>
                {
                    { "serviceName", serviceName },
                    { "deviceResourceUri", deviceResourceUri.AbsoluteUri },
                };
            if (offset != null)
                parameters.Add("offset", offset.ToString());
            if (limit != null)
                parameters.Add("limit", limit.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, Tuple<IEnumerable<DnsRecord>, int?>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    JToken records = result["records"];
                    if (records == null)
                        return null;

                    int? totalEntries = null;
                    JToken totalEntriesToken = result["totalEntries"];
                    if (totalEntriesToken != null)
                        totalEntries = totalEntriesToken.ToObject<int>();

                    return Tuple.Create(records.ToObject<IEnumerable<DnsRecord>>(), totalEntries);
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsRecord> ListPtrRecordDetailsAsync(string serviceName, Uri deviceResourceUri, string recordId, CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/rdns/{serviceName}/{recordId}?href={deviceResourceUri}");
            var parameters = new Dictionary<string, string>
                {
                    { "serviceName", serviceName },
                    { "deviceResourceUri", deviceResourceUri.AbsoluteUri },
                    { "recordId", recordId },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsRecord> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    return result.ToObject<DnsRecord>();
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<DnsJob<DnsRecordsList>> AddPtrRecordsAsync(string serviceName, Uri deviceResourceUri, IEnumerable<DnsDomainRecordConfiguration> recordConfigurations, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob<DnsRecordsList>> progress)
        {
            UriTemplate template = new UriTemplate("/rdns");
            var parameters = new Dictionary<string, string>();

            JObject request = new JObject(
                new JProperty("link", new JObject(
                    new JProperty("href", new JValue(deviceResourceUri.AbsoluteUri)),
                    new JProperty("rel", new JValue(serviceName)),
                    new JProperty("content", new JValue(string.Empty)))),
                new JProperty("records", JArray.FromObject(recordConfigurations)));
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, request);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob<DnsRecordsList>> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob<DnsRecordsList> job = task.Result.ToObject<DnsJob<DnsRecordsList>>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task UpdatePtrRecordsAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DnsJob> RemovePtrRecordsAsync(string serviceName, Uri deviceResourceUri, IPAddress ipAddress, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<DnsJob> progress)
        {
            UriTemplate template = new UriTemplate("/rdns/{serviceName}?href={deviceResourceUri}&ip={ipAddress}");
            var parameters = new Dictionary<string, string>()
                {
                    { "serviceName", serviceName },
                    { "deviceResourceUri", deviceResourceUri.AbsoluteUri },
                };
            if (ipAddress != null)
                parameters.Add("ipAddress", ipAddress.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<JObject>> requestResource =
                GetResponseAsyncFunc<JObject>(cancellationToken);

            Func<Task<JObject>, DnsJob> resultSelector =
                task =>
                {
                    JObject result = task.Result;
                    if (result == null)
                        return null;

                    DnsJob job = task.Result.ToObject<DnsJob>();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        job = WaitForJobAsync(job, true, cancellationToken, progress).Result;

                    return job;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        #endregion

        protected Task<DnsJob> WaitForJobAsync(DnsJob job, bool showDetails, CancellationToken cancellationToken, IProgress<DnsJob> progress)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            Func<DnsJob> func =
                () =>
                {
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        DnsJob updatedJob = GetJobStatusAsync(job, showDetails, cancellationToken).Result;
                        if (updatedJob == null || updatedJob.Id != job.Id)
                            throw new InvalidOperationException("Could not obtain status for job.");

                        if (progress != null)
                            progress.Report(updatedJob);

                        if (updatedJob.Status == DnsJobStatus.Completed || updatedJob.Status == DnsJobStatus.Error)
                            return updatedJob;

                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                };

            return Task.Factory.StartNew(func);
        }

        protected Task<DnsJob<TResult>> WaitForJobAsync<TResult>(DnsJob<TResult> job, bool showDetails, CancellationToken cancellationToken, IProgress<DnsJob<TResult>> progress)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            Func<DnsJob<TResult>> func =
                () =>
                {
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        DnsJob<TResult> updatedJob = GetJobStatusAsync(job, showDetails, cancellationToken).Result;
                        if (updatedJob == null || updatedJob.Id != job.Id)
                            throw new InvalidOperationException("Could not obtain status for job.");

                        if (progress != null)
                            progress.Report(updatedJob);

                        if (updatedJob.Status == DnsJobStatus.Completed || updatedJob.Status == DnsJobStatus.Error)
                            return updatedJob;

                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                };

            return Task.Factory.StartNew(func);
        }

        private Task<Uri> GetBaseUriAsync(CancellationToken cancellationToken)
        {
            if (_baseUri != null)
            {
                return InternalTaskExtensions.CompletedTask(_baseUri);
            }

            return Task.Factory.StartNew(
                () =>
                {
                    Endpoint endpoint = GetServiceEndpoint(null, "rax:dns", "cloudDNS", null);
                    Uri baseUri = new Uri(_internalUrl ? endpoint.InternalURL : endpoint.PublicURL);
                    _baseUri = baseUri;
                    return baseUri;
                });
        }

        private Task<Tuple<IdentityToken, Uri>> AuthenticateServiceAsync(CancellationToken cancellationToken)
        {
            Task<IdentityToken> authenticate;
            IIdentityService identityService = IdentityProvider as IIdentityService;
            if (identityService != null)
                authenticate = identityService.GetTokenAsync(GetDefaultIdentity(null));
            else
                authenticate = Task.Factory.StartNew(() => IdentityProvider.GetToken(GetDefaultIdentity(null)));

            Func<Task<IdentityToken>, Task<Tuple<IdentityToken, Uri>>> getBaseUri =
                task =>
                {
                    Task[] tasks = { task, GetBaseUriAsync(cancellationToken) };
                    return Task.Factory.ContinueWhenAll(tasks,
                        ts =>
                        {
                            Task<IdentityToken> first = (Task<IdentityToken>)ts[0];
                            Task<Uri> second = (Task<Uri>)ts[1];
                            return Tuple.Create(first.Result, second.Result);
                        });
                };

            return authenticate.ContinueWith(getBaseUri).Unwrap();
        }

        private Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> PrepareRequestAsyncFunc(HttpMethod method, UriTemplate template, IDictionary<string, string> parameters, Func<Uri, Uri> uriTransform = null)
        {
            return
                task =>
                {
                    Uri baseUri = task.Result.Item2;
                    Uri boundUri = template.BindByName(baseUri, parameters);
                    if (uriTransform != null)
                        boundUri = uriTransform(boundUri);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(boundUri);
                    request.Method = method.ToString().ToUpperInvariant();
                    request.Accept = JsonRequestSettings.JsonContentType;
                    request.Headers["X-Auth-Token"] = task.Result.Item1.Id;
                    request.UserAgent = UserAgentGenerator.UserAgent;
                    request.Timeout = (int)TimeSpan.FromSeconds(14400).TotalMilliseconds;
                    if (ConnectionLimit.HasValue)
                        request.ServicePoint.ConnectionLimit = ConnectionLimit.Value;

                    return request;
                };
        }

        private Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> PrepareRequestAsyncFunc<TBody>(HttpMethod method, UriTemplate template, IDictionary<string, string> parameters, TBody body)
        {
            return
                task =>
                {
                    Uri baseUri = task.Result.Item2;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(template.BindByName(baseUri, parameters));
                    request.Method = method.ToString().ToUpperInvariant();
                    request.Accept = JsonRequestSettings.JsonContentType;
                    request.Headers["X-Auth-Token"] = task.Result.Item1.Id;
                    request.UserAgent = UserAgentGenerator.UserAgent;
                    request.Timeout = (int)TimeSpan.FromSeconds(14400).TotalMilliseconds;
                    if (ConnectionLimit.HasValue)
                        request.ServicePoint.ConnectionLimit = ConnectionLimit.Value;

                    string bodyText = JsonConvert.SerializeObject(body);
                    byte[] encodedBody = Encoding.UTF8.GetBytes(bodyText);
                    request.ContentType = JsonRequestSettings.JsonContentType + "; charset=UTF-8";
                    request.ContentLength = encodedBody.Length;

                    Task<Stream> streamTask = Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream(null, null), request.EndGetRequestStream);
                    return
                        streamTask.ContinueWith(subTask =>
                        {
                            using (Stream stream = subTask.Result)
                            {
                                stream.Write(encodedBody, 0, encodedBody.Length);
                            }

                            return request;
                        });
                };
        }

        private Func<Task<HttpWebRequest>, Task<string>> GetResponseAsyncFunc(CancellationToken cancellationToken)
        {
            Func<Task<HttpWebRequest>, Task<WebResponse>> requestResource =
                task =>
                {
                    Console.WriteLine("{0} (Request) {1} {2}", DateTime.Now, task.Result.Method, task.Result.RequestUri);
                    return task.Result.GetResponseAsync(cancellationToken);
                };
            Func<Task<WebResponse>, string> readResult =
                task =>
                {
                    HttpWebResponse response;
                    if (task.IsFaulted)
                    {
                        WebException webException = task.Exception.Flatten().InnerException as WebException;
                        if (webException == null)
                            task.PropagateExceptions();

                        response = webException.Response as HttpWebResponse;
                        if (response == null)
                            task.PropagateExceptions();
                    }
                    else
                    {
                        response = (HttpWebResponse)task.Result;
                    }

                    Console.WriteLine("{0} (Result) {1}", DateTime.Now, response.ResponseUri);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string body = reader.ReadToEnd();
                        task.PropagateExceptions();
                        return body;
                    }
                };

            Func<Task<HttpWebRequest>, Task<string>> result =
                task =>
                {
                    return task.ContinueWith(requestResource).Unwrap()
                        .ContinueWith(readResult);
                };

            return result;
        }

        private Func<Task<HttpWebRequest>, Task<T>> GetResponseAsyncFunc<T>(CancellationToken cancellationToken, Func<Task<Tuple<HttpWebResponse, string>>, Task<T>> parseResult = null)
        {
            Func<Task<HttpWebRequest>, Task<WebResponse>> requestResource =
                task =>
                {
                    Console.WriteLine("{0} (Request) {1} {2}", DateTime.Now, task.Result.Method, task.Result.RequestUri);
                    return task.Result.GetResponseAsync(cancellationToken);
                };
            Func<Task<WebResponse>, Tuple<HttpWebResponse, string>> readResult =
                task =>
                {
                    HttpWebResponse response;
                    if (task.IsFaulted)
                    {
                        WebException webException = task.Exception.Flatten().InnerException as WebException;
                        if (webException == null)
                            task.PropagateExceptions();

                        response = webException.Response as HttpWebResponse;
                        if (response == null)
                            task.PropagateExceptions();
                    }
                    else
                    {
                        response = (HttpWebResponse)task.Result;
                    }

                    Console.WriteLine("{0} (Result) {1}", DateTime.Now, response.ResponseUri);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string body = reader.ReadToEnd();
                        task.PropagateExceptions();
                        return Tuple.Create(response, body);
                    }
                };
            if (parseResult == null)
            {
                parseResult =
                    task =>
                    {
#if NET35
                        return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(task.Result.Item2));
#else
                        //Newtonsoft.Json.Linq.JObject deserialized = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(task.Result.Item2);
                        //Console.WriteLine(JsonConvert.SerializeObject(deserialized, Formatting.Indented));
                        return JsonConvert.DeserializeObjectAsync<T>(task.Result.Item2);
#endif
                    };
            }

            Func<Task<HttpWebRequest>, Task<T>> result =
                task =>
                {
                    return task.ContinueWith(requestResource).Unwrap()
                        .ContinueWith(readResult)
                        .ContinueWith(parseResult).Unwrap();
                };

            return result;
        }
    }
}
