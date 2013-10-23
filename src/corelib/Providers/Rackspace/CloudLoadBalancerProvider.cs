namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using JSIStudios.SimpleRESTServices.Client;
    using net.openstack.Core;
    using net.openstack.Core.Domain;
    using net.openstack.Core.Providers;
    using net.openstack.Providers.Rackspace.Objects;
    using net.openstack.Providers.Rackspace.Objects.Request;
    using net.openstack.Providers.Rackspace.Objects.Response;
    using net.openstack.Providers.Rackspace.Validators;
    using Newtonsoft.Json;
    using CancellationToken = System.Threading.CancellationToken;
    using JsonRequestSettings = JSIStudios.SimpleRESTServices.Client.Json.JsonRequestSettings;
    using Stream = System.IO.Stream;
    using StreamReader = System.IO.StreamReader;

    public class CloudLoadBalancerProvider : ProviderBase<ILoadBalancerService>, ILoadBalancerService
    {
        /// <summary>
        /// This field caches the base URI used for accessing the Cloud Load Balancers service.
        /// </summary>
        /// <seealso cref="GetBaseUriAsync"/>
        private Uri _baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudLoadBalancerProvider"/> class with
        /// the specified values.
        /// </summary>
        /// <param name="defaultIdentity">The default identity to use for calls that do not explicitly specify an identity. If this value is <c>null</c>, no default identity is available so all calls must specify an explicit identity.</param>
        /// <param name="defaultRegion">The default region to use for calls that do not explicitly specify a region. If this value is <c>null</c>, the default region for the user will be used; otherwise if the service uses region-specific endpoints all calls must specify an explicit region.</param>
        /// <param name="identityProvider">The identity provider to use for authenticating requests to this provider. If this value is <c>null</c>, a new instance of <see cref="CloudIdentityProvider"/> is created using <paramref name="defaultIdentity"/> as the default identity.</param>
        /// <param name="restService">The implementation of <see cref="IRestService"/> to use for executing REST requests. If this value is <c>null</c>, the provider will use a new instance of <see cref="JsonRestServices"/>.</param>
        public CloudLoadBalancerProvider(CloudIdentity defaultIdentity, string defaultRegion, IIdentityProvider identityProvider, IRestService restService)
            : base(defaultIdentity, defaultRegion, identityProvider, restService, HttpResponseCodeValidator.Default)
        {
        }

        #region ILoadBalancerService Members

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancer>> ListLoadBalancersAsync(string markerId, int? limit, CancellationToken cancellationToken)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException("limit");

            UriTemplate template = new UriTemplate("/loadbalancers?markerId={markerId}&limit={limit}");
            var parameters = new Dictionary<string, string>();
            if (markerId != null)
                parameters.Add("markerId", markerId);
            if (limit != null)
                parameters.Add("limit", limit.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancersResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancersResponse>(cancellationToken);

            Func<Task<ListLoadBalancersResponse>, IEnumerable<LoadBalancer>> resultSelector =
                task => (task.Result != null ? task.Result.LoadBalancers : null) ?? Enumerable.Empty<LoadBalancer>();

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<LoadBalancer> GetLoadBalancerAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}");
            var parameters = new Dictionary<string, string> { { "loadBalancerId", loadBalancerId } };
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerResponse>(cancellationToken);

            Func<Task<GetLoadBalancerResponse>, LoadBalancer> resultSelector =
                task => task.Result.LoadBalancer;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<LoadBalancer> CreateLoadBalancerAsync(LoadBalancerConfiguration configuration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            UriTemplate template = new UriTemplate("/loadbalancers");
            var parameters = new Dictionary<string, string>();

            CreateLoadBalancerRequest requestBody = new CreateLoadBalancerRequest(configuration);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, requestBody);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerResponse>(cancellationToken);

            Func<Task<GetLoadBalancerResponse>, LoadBalancer> resultSelector =
                task =>
                {
                    LoadBalancer loadBalancer = task.Result.LoadBalancer;
                    if (loadBalancer != null && completionOption == DnsCompletionOption.RequestCompleted)
                        loadBalancer = WaitForLoadBalancerToLeaveStateAsync(loadBalancer.Id, LoadBalancerStatus.Build, cancellationToken, progress).Result;

                    return loadBalancer;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        public Task UpdateLoadBalancerAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RemoveLoadBalancerAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}");
            var parameters = new Dictionary<string, string> { { "loadBalancerId", loadBalancerId } };
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task RemoveLoadBalancerRangeAsync(IEnumerable<string> loadBalancerIds, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer[]> progress)
        {
            if (loadBalancerIds == null)
                throw new ArgumentNullException("loadBalancerIds");

            return RemoveLoadBalancerRangeAsync(loadBalancerIds.ToArray(), completionOption, cancellationToken, progress);
        }

        /// <summary>
        /// Removes one or more load balancers.
        /// </summary>
        /// <param name="loadBalancerIds">The IDs of load balancers to remove. These is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerIds"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerIds"/> contains any <c>null</c> or empty values.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Load_Balancer-d1e2093.html">Remove Load Balancer (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        public Task RemoveLoadBalancerRangeAsync(string[] loadBalancerIds, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer[]> progress)
        {
            if (loadBalancerIds == null)
                throw new ArgumentNullException("loadBalancerIds");
            if (loadBalancerIds.Any(string.IsNullOrEmpty))
                throw new ArgumentException("loadBalancerIds cannot contain any null or empty values", "loadBalancerIds");

            if (loadBalancerIds.Length == 0)
            {
                return CompletedTask.Default;
            }
            else if (loadBalancerIds.Length == 1)
            {
                IProgress<LoadBalancer> wrapper = null;
                if (progress != null)
                    wrapper = new ArrayElementProgressWrapper<LoadBalancer>(progress);

                return RemoveLoadBalancerAsync(loadBalancerIds[0], completionOption, cancellationToken, wrapper);
            }
            else
            {
                UriTemplate template = new UriTemplate("/loadbalancers?id={id}");
                var parameters = new Dictionary<string, string> { { "id", string.Join(",", loadBalancerIds) } };

                Func<Uri, Uri> uriTransform =
                    uri =>
                    {
                        string path = uri.GetLeftPart(UriPartial.Path);
                        string query = uri.Query.Replace(",", "&id=").Replace("%2c", "&id=");
                        return new Uri(path + query);
                    };

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters, uriTransform);

                Func<Task<HttpWebRequest>, Task<string>> requestResource =
                    GetResponseAsyncFunc(cancellationToken);

                return AuthenticateServiceAsync(cancellationToken)
                    .ContinueWith(prepareRequest)
                    .ContinueWith(requestResource).Unwrap();
            }
        }

        /// <inheritdoc/>
        public Task<string> GetErrorPageAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/errorpage");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerErrorPageResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerErrorPageResponse>(cancellationToken);

            Func<Task<GetLoadBalancerErrorPageResponse>, string> resultSelector =
                task => task.Result != null ? task.Result.Content : null;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task SetErrorPageAsync(string loadBalancerId, string content, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (content == null)
                throw new ArgumentNullException("content");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("content cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/errorpage");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            SetLoadBalancerErrorPageRequest requestBody = new SetLoadBalancerErrorPageRequest(content);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, requestBody);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            Func<Task<string>, string> resultSelector =
                task =>
                {
                    task.PropagateExceptions();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        WaitForLoadBalancerToLeaveStateAsync(loadBalancerId, LoadBalancerStatus.PendingUpdate, cancellationToken, progress).Wait(cancellationToken);

                    return task.Result;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task RemoveErrorPageAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/errorpage");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            Func<Task<string>, string> resultSelector =
                task =>
                {
                    task.PropagateExceptions();
                    if (completionOption == DnsCompletionOption.RequestCompleted)
                        WaitForLoadBalancerToLeaveStateAsync(loadBalancerId, LoadBalancerStatus.PendingUpdate, cancellationToken, progress).Wait(cancellationToken);

                    return task.Result;
                };

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<LoadBalancerStatistics> GetStatisticsAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/stats");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<LoadBalancerStatistics>> requestResource =
                GetResponseAsyncFunc<LoadBalancerStatistics>(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        public Task<IEnumerable<Node>> ListNodesAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Node> GetNodeAsync(string loadBalancerId, string nodeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Node> AddNodeAsync(string loadBalancerId, NodeConfiguration nodeConfiguration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node> progress)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Node>> AddNodeRangeAsync(string loadBalancerId, IEnumerable<NodeConfiguration> nodeConfiguration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node[]> progress)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNodeAsync(string loadBalancerId, string nodeId, NodeCondition condition, NodeType type, int? weight, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node> progress)
        {
            throw new NotImplementedException();
        }

        public Task RemoveNodeAsync(string loadBalancerId, string nodeId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node> progress)
        {
            throw new NotImplementedException();
        }

        public Task RemoveNodeRangeAsync(string loadBalancerId, IEnumerable<string> nodeIds, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node[]> progress)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NodeServiceEvent>> ListNodeServiceEvents(string loadBalancerId, string marker, int? limit, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ListVirtualAddressesAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveVirtualAddressAsync(string loadBalancerId, string virtualAddressId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            throw new NotImplementedException();
        }

        public Task RemoveVirtualAddressRangeAsync(string loadBalancerId, IEnumerable<string> virtualAddressId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<string>> ListAllowedDomainsAsync(CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/loadbalancers/alloweddomains");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListAllowedDomainsResponse>> requestResource =
                GetResponseAsyncFunc<ListAllowedDomainsResponse>(cancellationToken);

            Func<Task<ListAllowedDomainsResponse>, IEnumerable<string>> resultSelector =
                task => task.Result.AllowedDomains;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        public Task<IEnumerable<LoadBalancer>> ListBillableLoadBalancersAsync(DateTimeOffset startTime, DateTimeOffset endTime, int? offset, int? limit, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LoadBalancerUsage>> ListAccountLevelUsageAsync(DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LoadBalancerUsage>> ListHistoricalUsageAsync(string loadBalancerId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LoadBalancerUsage>> ListCurrentUsageAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NetworkItem>> ListAccessListAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CreateAccessListAsync(string loadBalancerId, NetworkItem networkItem, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task CreateAccessListAsync(string loadBalancerId, IEnumerable<NetworkItem> networkItems, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAccessListAsync(string loadBalancerId, string networkItemId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAccessListAsync(string loadBalancerId, IEnumerable<string> networkItemIds, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ClearAccessListAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SessionPersistence> GetSessionPersistenceAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/sessionpersistence");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<SessionPersistence>> requestResource =
                GetResponseAsyncFunc<SessionPersistence>(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task SetSessionPersistenceAsync(string loadBalancerId, SessionPersistence sessionPersistence, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (sessionPersistence == null)
                throw new ArgumentNullException("sessionPersistence");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/sessionpersistence");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, sessionPersistence);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task RemoveSessionPersistenceAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/sessionpersistence");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<bool> GetConnectionLoggingAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/connectionlogging");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerConnectionLoggingResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerConnectionLoggingResponse>(cancellationToken);

            Func<Task<GetLoadBalancerConnectionLoggingResponse>, bool> resultSelector =
                task => task.Result != null ? task.Result.Enabled ?? false : false;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task SetConnectionLoggingAsync(string loadBalancerId, bool enabled, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/connectionlogging");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            SetLoadBalancerConnectionLoggingRequest request = new SetLoadBalancerConnectionLoggingRequest(enabled);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, request);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<ConnectionThrottles> ListThrottlesAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/connectionthrottle");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancerThrottlesResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancerThrottlesResponse>(cancellationToken);

            Func<Task<ListLoadBalancerThrottlesResponse>, ConnectionThrottles> resultSelector =
                task => task.Result.Throttles;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task UpdateThrottlesAsync(string loadBalancerId, ConnectionThrottles throttleConfiguration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (throttleConfiguration == null)
                throw new ArgumentNullException("throttleConfiguration");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/connectionthrottle");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, throttleConfiguration);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task RemoveThrottlesAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/connectionthrottle");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<bool> GetContentCachingAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/contentcaching");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerContentCachingResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerContentCachingResponse>(cancellationToken);

            Func<Task<GetLoadBalancerContentCachingResponse>, bool> resultSelector =
                task => task.Result != null ? task.Result.Enabled ?? false : false;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task SetContentCachingAsync(string loadBalancerId, bool enabled, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/contentcaching");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId }
            };

            SetLoadBalancerContentCachingRequest request = new SetLoadBalancerContentCachingRequest(enabled);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, request);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancingProtocol>> ListProtocolsAsync(CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/loadbalancers/protocols");
            var parameters = new Dictionary<string, string>();

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancingProtocolsResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancingProtocolsResponse>(cancellationToken);

            Func<Task<ListLoadBalancingProtocolsResponse>, IEnumerable<LoadBalancingProtocol>> resultSelector =
                task => (task.Result != null ? task.Result.Protocols : null) ?? Enumerable.Empty<LoadBalancingProtocol>();

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancingAlgorithm>> ListAlgorithmsAsync(CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/loadbalancers/algorithms");
            var parameters = new Dictionary<string, string>();

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancingAlgorithmsResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancingAlgorithmsResponse>(cancellationToken);

            Func<Task<ListLoadBalancingAlgorithmsResponse>, IEnumerable<LoadBalancingAlgorithm>> resultSelector =
                task => (task.Result != null ? task.Result.Algorithms : null) ?? Enumerable.Empty<LoadBalancingAlgorithm>();

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancerMetadataItem>> ListLoadBalancerMetadataAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancerMetadataResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancerMetadataResponse>(cancellationToken);

            Func<Task<ListLoadBalancerMetadataResponse>, IEnumerable<LoadBalancerMetadataItem>> resultSelector =
                task => (task.Result != null ? task.Result.Metadata : null) ?? Enumerable.Empty<LoadBalancerMetadataItem>();

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<LoadBalancerMetadataItem> GetLoadBalancerMetadataItemAsync(string loadBalancerId, string metadataId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (metadataId == null)
                throw new ArgumentNullException("metadataId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(metadataId))
                throw new ArgumentException("metadataId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata/{metaId}");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
                { "metaId", metadataId },
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerMetadataItemResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerMetadataItemResponse>(cancellationToken);

            Func<Task<GetLoadBalancerMetadataItemResponse>, LoadBalancerMetadataItem> resultSelector =
                task => task.Result != null ? task.Result.MetadataItem : null;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancerMetadataItem>> ListNodeMetadataAsync(string loadBalancerId, string nodeId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (nodeId == null)
                throw new ArgumentNullException("nodeId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentException("nodeId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
                { "nodeId", nodeId },
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancerMetadataResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancerMetadataResponse>(cancellationToken);

            Func<Task<ListLoadBalancerMetadataResponse>, IEnumerable<LoadBalancerMetadataItem>> resultSelector =
                task => (task.Result != null ? task.Result.Metadata : null) ?? Enumerable.Empty<LoadBalancerMetadataItem>();

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<LoadBalancerMetadataItem> GetNodeMetadataItemAsync(string loadBalancerId, string nodeId, string metadataId, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (nodeId == null)
                throw new ArgumentNullException("nodeId");
            if (metadataId == null)
                throw new ArgumentNullException("metadataId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentException("nodeId cannot be empty");
            if (string.IsNullOrEmpty(metadataId))
                throw new ArgumentException("metadataId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata/{metaId}");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
                { "nodeId", nodeId },
                { "metaId", metadataId },
            };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<GetLoadBalancerMetadataItemResponse>> requestResource =
                GetResponseAsyncFunc<GetLoadBalancerMetadataItemResponse>(cancellationToken);

            Func<Task<GetLoadBalancerMetadataItemResponse>, LoadBalancerMetadataItem> resultSelector =
                task => task.Result != null ? task.Result.MetadataItem : null;

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancerMetadataItem>> AddLoadBalancerMetadataAsync(string loadBalancerId, IEnumerable<KeyValuePair<string, string>> metadata, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (metadata == null)
                throw new ArgumentNullException("metadata");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
            };

            AddLoadBalancerMetadataRequest requestBody = new AddLoadBalancerMetadataRequest(metadata);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, requestBody);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancerMetadataResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancerMetadataResponse>(cancellationToken);

            Func<Task<ListLoadBalancerMetadataResponse>, IEnumerable<LoadBalancerMetadataItem>> resultSelector =
                task => (task.Result != null ? task.Result.Metadata : null) ?? Enumerable.Empty<LoadBalancerMetadataItem>();

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LoadBalancerMetadataItem>> AddNodeMetadataAsync(string loadBalancerId, string nodeId, IEnumerable<KeyValuePair<string, string>> metadata, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (nodeId == null)
                throw new ArgumentNullException("nodeId");
            if (metadata == null)
                throw new ArgumentNullException("metadata");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentException("nodeId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
                { "nodeId", nodeId },
            };

            AddLoadBalancerMetadataRequest requestBody = new AddLoadBalancerMetadataRequest(metadata);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, requestBody);

            Func<Task<HttpWebRequest>, Task<ListLoadBalancerMetadataResponse>> requestResource =
                GetResponseAsyncFunc<ListLoadBalancerMetadataResponse>(cancellationToken);

            Func<Task<ListLoadBalancerMetadataResponse>, IEnumerable<LoadBalancerMetadataItem>> resultSelector =
                task => (task.Result != null ? task.Result.Metadata : null) ?? Enumerable.Empty<LoadBalancerMetadataItem>();

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task UpdateLoadBalancerMetadataItemAsync(string loadBalancerId, string metadataId, string value, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (metadataId == null)
                throw new ArgumentNullException("metadataId");
            if (value == null)
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(metadataId))
                throw new ArgumentException("metadataId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata/{metaId}");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
                { "metaId", metadataId }
            };

            UpdateLoadBalancerMetadataItemRequest requestBody = new UpdateLoadBalancerMetadataItemRequest(value);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, requestBody);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task UpdateNodeMetadataItemAsync(string loadBalancerId, string nodeId, string metadataId, string value, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (nodeId == null)
                throw new ArgumentNullException("nodeId");
            if (metadataId == null)
                throw new ArgumentNullException("metadataId");
            if (value == null)
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentException("nodeId cannot be empty");
            if (string.IsNullOrEmpty(metadataId))
                throw new ArgumentException("metadataId cannot be empty");

            UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata/{metaId}");
            var parameters = new Dictionary<string, string>()
            {
                { "loadBalancerId", loadBalancerId },
                { "nodeId", nodeId },
                { "metaId", metadataId }
            };

            UpdateLoadBalancerMetadataItemRequest requestBody = new UpdateLoadBalancerMetadataItemRequest(value);
            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, requestBody);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task RemoveLoadBalancerMetadataItemAsync(string loadBalancerId, IEnumerable<string> metadataIds, CancellationToken cancellationToken)
        {
            if (metadataIds == null)
                throw new ArgumentNullException("metadataIds");

            return RemoveLoadBalancerMetadataItemAsync(loadBalancerId, metadataIds.ToArray(), cancellationToken);
        }

        /// <summary>
        /// Removes one or more metadata items associated with a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="metadataIds">The metadata item IDs. These are obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> contains any <c>null</c> or empty values.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Metadata-d1e2675.html">Remove Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        public Task RemoveLoadBalancerMetadataItemAsync(string loadBalancerId, string[] metadataIds, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (metadataIds == null)
                throw new ArgumentNullException("metadataIds");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (metadataIds.Any(string.IsNullOrEmpty))
                throw new ArgumentException("metadataIds cannot contain any null or empty values", "metadataIds");

            if (metadataIds.Length == 0)
            {
                return CompletedTask.Default;
            }
            else if (metadataIds.Length == 1)
            {
                UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata/{metaId}");
                var parameters = new Dictionary<string, string>()
                {
                    { "loadBalancerId", loadBalancerId },
                    { "metaId", metadataIds[0] }
                };

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

                Func<Task<HttpWebRequest>, Task<string>> requestResource =
                    GetResponseAsyncFunc(cancellationToken);

                return AuthenticateServiceAsync(cancellationToken)
                    .ContinueWith(prepareRequest)
                    .ContinueWith(requestResource).Unwrap();
            }
            else
            {
                UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata?id={id}");
                var parameters = new Dictionary<string, string>()
                {
                    { "loadBalancerId", loadBalancerId },
                    { "id", string.Join(",", metadataIds) }
                };

                Func<Uri, Uri> uriTransform =
                    uri =>
                    {
                        string path = uri.GetLeftPart(UriPartial.Path);
                        string query = uri.Query.Replace(",", "&id=").Replace("%2c", "&id=");
                        return new Uri(path + query);
                    };

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters, uriTransform);

                Func<Task<HttpWebRequest>, Task<string>> requestResource =
                    GetResponseAsyncFunc(cancellationToken);

                return AuthenticateServiceAsync(cancellationToken)
                    .ContinueWith(prepareRequest)
                    .ContinueWith(requestResource).Unwrap();
            }
        }

        /// <inheritdoc/>
        public Task RemoveNodeMetadataItemAsync(string loadBalancerId, string nodeId, IEnumerable<string> metadataIds, CancellationToken cancellationToken)
        {
            if (metadataIds == null)
                throw new ArgumentNullException("metadataIds");

            return RemoveNodeMetadataItemAsync(loadBalancerId, nodeId, metadataIds.ToArray(), cancellationToken);
        }

        /// <summary>
        /// Removes one or more metadata items associated with a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="metadataIds">The metadata item IDs. These are obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is empty.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> contains any <c>null</c> or empty values.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Metadata-d1e2675.html">Remove Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        public Task RemoveNodeMetadataItemAsync(string loadBalancerId, string nodeId, string[] metadataIds, CancellationToken cancellationToken)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (nodeId == null)
                throw new ArgumentNullException("nodeId");
            if (metadataIds == null)
                throw new ArgumentNullException("metadataIds");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentException("nodeId cannot be empty");
            if (metadataIds.Any(string.IsNullOrEmpty))
                throw new ArgumentException("metadataIds cannot contain any null or empty values", "metadataIds");

            if (metadataIds.Length == 0)
            {
                return CompletedTask.Default;
            }
            else if (metadataIds.Length == 1)
            {
                UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata/{metaId}");
                var parameters = new Dictionary<string, string>()
                {
                    { "loadBalancerId", loadBalancerId },
                    { "nodeId", nodeId },
                    { "metaId", metadataIds[0] }
                };

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

                Func<Task<HttpWebRequest>, Task<string>> requestResource =
                    GetResponseAsyncFunc(cancellationToken);

                return AuthenticateServiceAsync(cancellationToken)
                    .ContinueWith(prepareRequest)
                    .ContinueWith(requestResource).Unwrap();
            }
            else
            {
                UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata?id={id}");
                var parameters = new Dictionary<string, string>()
                {
                    { "loadBalancerId", loadBalancerId },
                    { "nodeId", nodeId },
                    { "id", string.Join(",", metadataIds) }
                };

                Func<Uri, Uri> uriTransform =
                    uri =>
                    {
                        string path = uri.GetLeftPart(UriPartial.Path);
                        string query = uri.Query.Replace(",", "&id=").Replace("%2c", "&id=");
                        return new Uri(path + query);
                    };

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters, uriTransform);

                Func<Task<HttpWebRequest>, Task<string>> requestResource =
                    GetResponseAsyncFunc(cancellationToken);

                return AuthenticateServiceAsync(cancellationToken)
                    .ContinueWith(prepareRequest)
                    .ContinueWith(requestResource).Unwrap();
            }
        }

        #endregion

        protected Task<LoadBalancer> WaitForLoadBalancerToLeaveStateAsync(string loadBalancerId, LoadBalancerStatus state, CancellationToken cancellationToken, IProgress<LoadBalancer> progress)
        {
            if (loadBalancerId == null)
                throw new ArgumentNullException("loadBalancerId");
            if (string.IsNullOrEmpty(loadBalancerId))
                throw new ArgumentException("loadBalancerId cannot be empty");

            Func<LoadBalancer> func =
                () =>
                {
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        LoadBalancer updatedLoadBalancer = GetLoadBalancerAsync(loadBalancerId, cancellationToken).Result;
                        if (updatedLoadBalancer == null || updatedLoadBalancer.Id != loadBalancerId)
                            throw new InvalidOperationException("Could not obtain status for load balancer.");

                        if (progress != null)
                            progress.Report(updatedLoadBalancer);

                        if (updatedLoadBalancer.Status != state)
                            return updatedLoadBalancer;

                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                };

            return Task.Factory.StartNew(func);
        }

        protected override Task<Uri> GetBaseUriAsync(CancellationToken cancellationToken)
        {
            if (_baseUri != null)
            {
                return InternalTaskExtensions.CompletedTask(_baseUri);
            }

            return Task.Factory.StartNew(
                () =>
                {
                    Endpoint endpoint = GetServiceEndpoint(null, "rax:load-balancer", "cloudLoadBalancers", null);
                    _baseUri = new Uri(endpoint.PublicURL);
                    return _baseUri;
                });
        }

        private class ArrayElementProgressWrapper<T> : IProgress<T>
        {
            private readonly IProgress<T[]> _delegate;

            public ArrayElementProgressWrapper(IProgress<T[]> @delegate)
            {
                if (@delegate == null)
                    throw new ArgumentNullException("delegate");

                _delegate = @delegate;
            }

            public void Report(T value)
            {
                _delegate.Report(new T[] { value });
            }
        }
    }
}
