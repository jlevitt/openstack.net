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
    using net.openstack.Providers.Rackspace.Objects.Request;
    using net.openstack.Providers.Rackspace.Objects.Response;
    using net.openstack.Providers.Rackspace.Validators;
    using Newtonsoft.Json;
    using CancellationToken = System.Threading.CancellationToken;
    using StreamReader = System.IO.StreamReader;
    using JsonRequestSettings = JSIStudios.SimpleRESTServices.Client.Json.JsonRequestSettings;
    using Stream = System.IO.Stream;

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
        public Task<bool> GetConnectionLoggingAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SetConnectionLoggingAsync(string loadBalancerId, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ConnectionThrottles> ListThrottlesAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task UpdateThrottlesAsync(string loadBalancerId, ConnectionThrottles throttleConfiguration, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RemoveThrottlesAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> GetContentCachingAsync(string loadBalancerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SetContentCachingAsync(string loadBalancerId, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
                List<string> queryString = new List<string>();
                for (int i = 0; i < metadataIds.Length; i++)
                    queryString.Add(string.Format("id={{id{0}}}", i));

                UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/metadata&" + string.Join("&", queryString));
                var parameters = new Dictionary<string, string>()
                {
                    { "loadBalancerId", loadBalancerId },
                };
                for (int i = 0; i < metadataIds.Length; i++)
                    parameters.Add("id" + i, metadataIds[i]);

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

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
                List<string> queryString = new List<string>();
                for (int i = 0; i < metadataIds.Length; i++)
                    queryString.Add(string.Format("id={{id{0}}}", i));

                UriTemplate template = new UriTemplate("/loadbalancers/{loadBalancerId}/nodes/{nodeId}/metadata&" + string.Join("&", queryString));
                var parameters = new Dictionary<string, string>()
                {
                    { "loadBalancerId", loadBalancerId },
                    { "nodeId", nodeId }
                };
                for (int i = 0; i < metadataIds.Length; i++)
                    parameters.Add("id" + i, metadataIds[i]);

                Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                    PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

                Func<Task<HttpWebRequest>, Task<string>> requestResource =
                    GetResponseAsyncFunc(cancellationToken);

                return AuthenticateServiceAsync(cancellationToken)
                    .ContinueWith(prepareRequest)
                    .ContinueWith(requestResource).Unwrap();
            }
        }

        #endregion

        private Task<Uri> GetBaseUriAsync(CancellationToken cancellationToken)
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

        private Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> PrepareRequestAsyncFunc(HttpMethod method, UriTemplate template, IDictionary<string, string> parameters)
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
                    return task.Result.GetResponseAsync(cancellationToken);
                };
            Func<Task<WebResponse>, string> readResult =
                task =>
                {
                    using (StreamReader reader = new StreamReader(task.Result.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
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
                    return task.Result.GetResponseAsync(cancellationToken);
                };
            Func<Task<WebResponse>, HttpWebResponse> checkResult =
                task =>
                {
                    return (HttpWebResponse)task.Result;
                };
            Func<Task<HttpWebResponse>, Tuple<HttpWebResponse, string>> readResult =
                task =>
                {
                    using (StreamReader reader = new StreamReader(task.Result.GetResponseStream()))
                    {
                        return Tuple.Create(task.Result, reader.ReadToEnd());
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
                        return JsonConvert.DeserializeObjectAsync<T>(task.Result.Item2);
#endif
                    };
            }

            Func<Task<HttpWebRequest>, Task<T>> result =
                task =>
                {
                    return task.ContinueWith(requestResource).Unwrap()
                        .ContinueWith(checkResult)
                        .ContinueWith(readResult)
                        .ContinueWith(parseResult).Unwrap();
                };

            return result;
        }
    }
}
