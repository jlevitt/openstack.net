namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using JSIStudios.SimpleRESTServices.Client;
    using JSIStudios.SimpleRESTServices.Client.Json;
    using net.openstack.Core;
    using net.openstack.Core.Domain;
    using net.openstack.Core.Exceptions;
    using net.openstack.Core.Providers;
    using net.openstack.Providers.Rackspace.Objects.Request;
    using net.openstack.Providers.Rackspace.Objects.Response;
    using net.openstack.Providers.Rackspace.Validators;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provides an implementation of <see cref="IQueueingService"/> for operating
    /// with Rackspace's Cloud Queues product.
    /// </summary>
    /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1">OpenStack Marconi API v1 Blueprint</seealso>
    /// <threadsafety static="true" instance="false"/>
    public class CloudQueuesProvider : ProviderBase<IQueueingService>, IQueueingService
    {
        /// <summary>
        /// Specifies whether the <see cref="Endpoint.PublicURL"/> or <see cref="Endpoint.InternalURL"/>
        /// should be used for accessing the Cloud Queues API.
        /// </summary>
        private readonly bool _internalUrl;

        /// <summary>
        /// The value of the Client-Id header to send with message requests from this service.
        /// </summary>
        private readonly Guid _clientId;

        /// <summary>
        /// This field caches the base URI used for accessing the Cloud Queues service.
        /// </summary>
        /// <seealso cref="GetBaseUriAsync"/>
        private Uri _baseUri;

        /// <summary>
        /// This field caches the home document returned by <see cref="GetHomeAsync"/>.
        /// </summary>
        private HomeDocument _homeDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudQueuesProvider"/> class with
        /// the specified values.
        /// </summary>
        /// <param name="defaultIdentity">The default identity to use for calls that do not explicitly specify an identity. If this value is <c>null</c>, no default identity is available so all calls must specify an explicit identity.</param>
        /// <param name="defaultRegion">The default region to use for calls that do not explicitly specify a region. If this value is <c>null</c>, the default region for the user will be used; otherwise if the service uses region-specific endpoints all calls must specify an explicit region.</param>
        /// <param name="clientId">The value of the <strong>Client-Id</strong> header to send with message requests from this service.</param>
        /// <param name="internalUrl"><c>true</c> to use the endpoint's <see cref="Endpoint.InternalURL"/>; otherwise <c>false</c> to use the endpoint's <see cref="Endpoint.PublicURL"/>.</param>
        /// <param name="identityProvider">The identity provider to use for authenticating requests to this provider. If this value is <c>null</c>, a new instance of <see cref="CloudIdentityProvider"/> is created using <paramref name="defaultIdentity"/> as the default identity.</param>
        /// <param name="restService">The implementation of <see cref="IRestService"/> to use for executing REST requests. If this value is <c>null</c>, the provider will use a new instance of <see cref="JsonRestServices"/>.</param>
        public CloudQueuesProvider(CloudIdentity defaultIdentity, string defaultRegion, Guid clientId, bool internalUrl, IIdentityProvider identityProvider, IRestService restService)
            : base(defaultIdentity, defaultRegion, identityProvider, restService, HttpResponseCodeValidator.Default)
        {
            _clientId = clientId;
            _internalUrl = internalUrl;
        }

        #region IQueueingService Members

        /// <inheritdoc/>
        public Task<HomeDocument> GetHomeAsync(CancellationToken cancellationToken)
        {
            if (_homeDocument != null)
            {
                return InternalTaskExtensions.CompletedTask(_homeDocument);
            }

            UriTemplate template = new UriTemplate("/v1/");
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, new Dictionary<string, string>());

            Func<Task<HttpWebRequest>, Task<HomeDocument>> requestResource =
                GetResponseAsyncFunc<HomeDocument>(cancellationToken);

            Func<Task<HomeDocument>, HomeDocument> cacheResult =
                task =>
                {
                    _homeDocument = task.Result;
                    return task.Result;
                };

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(cacheResult);
        }

        /// <inheritdoc/>
        public Task GetNodeHealthAsync(CancellationToken cancellationToken)
        {
            UriTemplate template = new UriTemplate("/v1/health");
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.HEAD, template, new Dictionary<string, string>());

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<bool> CreateQueueAsync(string queueName, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}");
            var parameters = new Dictionary<string, string>
                {
                    { "queue_name", queueName }
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters);

            Func<Task<Tuple<HttpWebResponse, string>>, Task<bool>> parseResult =
                task =>
                {
                    if (task.Result.Item1.StatusCode == HttpStatusCode.Created)
                        return InternalTaskExtensions.CompletedTask(true);
                    else
                        return InternalTaskExtensions.CompletedTask(false);
                };

            Func<Task<HttpWebRequest>, Task<bool>> requestResource =
                GetResponseAsyncFunc(cancellationToken, parseResult);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<CloudQueue>> ListQueuesAsync(string marker, int? limit, bool detailed, CancellationToken cancellationToken)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            UriTemplate template = new UriTemplate("/v1/queues?marker={marker}&limit={limit}&detailed={detailed}");
            var parameters = new Dictionary<string, string>
                {
                    { "detailed", detailed.ToString().ToLowerInvariant() },
                };
            if (marker != null)
                parameters.Add("marker", marker);
            if (limit.HasValue)
                parameters.Add("limit", limit.ToString());

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListCloudQueuesResponse>> requestResource =
                GetResponseAsyncFunc<ListCloudQueuesResponse>(cancellationToken);

            Func<Task<ListCloudQueuesResponse>, IEnumerable<CloudQueue>> resultSelector =
                task => (task.Result != null ? task.Result.Queues : null) ?? Enumerable.Empty<CloudQueue>();

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<bool> QueueExistsAsync(string queueName, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}");
            var parameters = new Dictionary<string, string>() { { "queue_name", queueName } };
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.HEAD, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            Func<Task<string>, bool> interpretResult =
                task =>
                {
                    // a WebException would have been thrown if the queue didn't exist
                    if (task.Status == TaskStatus.RanToCompletion)
                        return true;

                    task.Exception.Flatten().Handle(
                        ex =>
                        {
                            WebException webException = ex as WebException;
                            if (webException == null)
                                return false;

                            HttpWebResponse response = webException.Response as HttpWebResponse;
                            if (response == null)
                                return false;

                            if (response.StatusCode != HttpStatusCode.NotFound)
                                return false;

                            // a 404 error means the queue does not exist
                            return true;
                        });

                    return false;
                };

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(interpretResult);
        }

        /// <inheritdoc/>
        public Task DeleteQueueAsync(string queueName, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}");
            var parameters = new Dictionary<string, string>
                {
                    { "queue_name", queueName }
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task SetQueueMetadataAsync<T>(string queueName, T metadata, CancellationToken cancellationToken)
            where T : class
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/metadata");
            var parameters = new Dictionary<string, string>
                {
                    { "queue_name", queueName }
                };

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.PUT, template, parameters, metadata);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<JObject> GetQueueMetadataAsync(string queueName, CancellationToken cancellationToken)
        {
            return GetQueueMetadataAsync<JObject>(queueName, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<T> GetQueueMetadataAsync<T>(string queueName, CancellationToken cancellationToken)
            where T : class
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/metadata");
            var parameters = new Dictionary<string, string>
                {
                    { "queue_name", queueName }
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<T>> requestResource =
                GetResponseAsyncFunc<T>(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<QueueStatistics> GetQueueStatisticsAsync(string queueName, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/stats");
            var parameters = new Dictionary<string, string>() { { "queue_name", queueName } };
            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<QueueStatistics>> requestResource =
                GetResponseAsyncFunc<QueueStatistics>(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<QueuedMessage>> ListMessagesAsync(string queueName, string marker, int? limit, bool echo, bool includeClaimed, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");
            if (limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages?marker={marker}&limit={limit}&echo={echo}&include_claimed={include_claimed}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                    { "echo", echo.ToString() },
                    { "include_claimed", includeClaimed.ToString() }
                };
            if (marker != null)
                parameters["marker"] = marker;
            if (limit != null)
                parameters["limit"] = limit.ToString();

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<ListCloudQueueMessagesResponse>> requestResource =
                GetResponseAsyncFunc<ListCloudQueueMessagesResponse>(cancellationToken);

            Func<Task<ListCloudQueueMessagesResponse>, IEnumerable<QueuedMessage>> resultSelector =
                task => task.Result.Messages;

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<QueuedMessage> GetMessageAsync(string queueName, string messageId, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages/{message_id}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                    { "message_id", messageId },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<QueuedMessage>> requestResource =
                GetResponseAsyncFunc<QueuedMessage>(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<QueuedMessage>> GetMessagesAsync(string queueName, IEnumerable<string> messageIds, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (messageIds == null)
                throw new ArgumentNullException("messageIds");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages?ids={ids}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                    { "ids", string.Join(",", messageIds.ToArray()) },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.GET, template, parameters);

            Func<Task<HttpWebRequest>, Task<IEnumerable<QueuedMessage>>> requestResource =
                GetResponseAsyncFunc<IEnumerable<QueuedMessage>>(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task PostMessagesAsync(string queueName, IEnumerable<Message> messages, CancellationToken cancellationToken)
        {
            return PostMessagesAsync(queueName, cancellationToken, messages.ToArray());
        }

        /// <inheritdoc/>
        public Task PostMessagesAsync(string queueName, CancellationToken cancellationToken, params Message[] messages)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (messages == null)
                throw new ArgumentNullException("messages");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, messages);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task PostMessagesAsync<T>(string queueName, IEnumerable<Message<T>> messages, CancellationToken cancellationToken)
        {
            return PostMessagesAsync(queueName, cancellationToken, messages.ToArray());
        }

        /// <inheritdoc/>
        public Task PostMessagesAsync<T>(string queueName, CancellationToken cancellationToken, params Message<T>[] messages)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (messages == null)
                throw new ArgumentNullException("messages");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, messages);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task DeleteMessageAsync(string queueName, string messageId, Claim claim, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages/{message_id}?claim_id={claim_id}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                    { "message_id", messageId },
                };
            if (claim != null)
                parameters["claim_id"] = claim.Id;

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task DeleteMessagesAsync(string queueName, IEnumerable<string> messageIds, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (messageIds == null)
                throw new ArgumentNullException("messageIds");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/messages?ids={ids}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                    { "ids", string.Join(",", messageIds.ToArray()) },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
        }

        /// <inheritdoc/>
        public Task<Claim> ClaimMessageAsync(string queueName, int? limit, TimeSpan timeToLive, TimeSpan gracePeriod, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");
            if (limit < 0)
                throw new ArgumentOutOfRangeException("limit");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/claims?limit={limit}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                };
            if (limit != null)
                parameters["limit"] = limit.ToString();

            var request = new ClaimMessagesRequest(timeToLive, gracePeriod);

            Func<Task<Tuple<IdentityToken, Uri>>, Task<HttpWebRequest>> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.POST, template, parameters, request);

            Func<Task<Tuple<HttpWebResponse, string>>, Task<Tuple<Uri, IEnumerable<QueuedMessage>>>> parseResult =
                task =>
                {
                    string location = task.Result.Item1.Headers[HttpResponseHeader.Location];
                    Uri locationUri = location != null ? new Uri(_baseUri, location) : null;

                    if (task.Result.Item1.StatusCode == HttpStatusCode.NoContent)
                    {
                        // the queue did not contain any messages to claim
                        Tuple<Uri, IEnumerable<QueuedMessage>> result = Tuple.Create(locationUri, Enumerable.Empty<QueuedMessage>());
                        return InternalTaskExtensions.CompletedTask(result);
                    }

                    IEnumerable<QueuedMessage> messages = JsonConvert.DeserializeObject<IEnumerable<QueuedMessage>>(task.Result.Item2);

                    return InternalTaskExtensions.CompletedTask(Tuple.Create(locationUri, messages));
                };
            Func<Task<HttpWebRequest>, Task<Tuple<Uri, IEnumerable<QueuedMessage>>>> requestResource =
                GetResponseAsyncFunc<Tuple<Uri, IEnumerable<QueuedMessage>>>(cancellationToken, parseResult);

            Func<Task<Tuple<Uri, IEnumerable<QueuedMessage>>>, Claim> resultSelector =
                task => new Claim(this, queueName, task.Result.Item1, timeToLive, TimeSpan.Zero, true, task.Result.Item2);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest).Unwrap()
                .ContinueWith(requestResource).Unwrap()
                .ContinueWith(resultSelector);
        }

        /// <inheritdoc/>
        public Task<Claim> QueryClaimAsync(string queueName, Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task UpdateClaimAsync(string queueName, Claim claim, TimeSpan timeToLive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task ReleaseClaimAsync(string queueName, Claim claim, CancellationToken cancellationToken)
        {
            if (queueName == null)
                throw new ArgumentNullException("queueName");
            if (claim == null)
                throw new ArgumentNullException("claim");
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName cannot be empty");

            UriTemplate template = new UriTemplate("/v1/queues/{queue_name}/claims/{claim_id}");

            var parameters =
                new Dictionary<string, string>()
                {
                    { "queue_name", queueName },
                    { "claim_id", claim.Id },
                };

            Func<Task<Tuple<IdentityToken, Uri>>, HttpWebRequest> prepareRequest =
                PrepareRequestAsyncFunc(HttpMethod.DELETE, template, parameters);

            Func<Task<HttpWebRequest>, Task<string>> requestResource =
                GetResponseAsyncFunc(cancellationToken);

            // authenticate -> request resource -> check result -> parse result -> cache result -> return
            return AuthenticateServiceAsync(cancellationToken)
                .ContinueWith(prepareRequest)
                .ContinueWith(requestResource).Unwrap();
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
                    try
                    {
                        Endpoint endpoint = GetServiceEndpoint(null, "queues", "cloudQueues", null);
                        Uri baseUri = new Uri(_internalUrl ? endpoint.InternalURL : endpoint.PublicURL);
                        _baseUri = baseUri;
                        return baseUri;
                    }
                    catch (UserAuthorizationException)
                    {
                        string effectiveRegion = null;
                        if (!string.IsNullOrEmpty(DefaultRegion))
                        {
                            effectiveRegion = DefaultRegion;
                        }
                        else
                        {
                            UserAccess userAccess = IdentityProvider.GetUserAccess(GetDefaultIdentity(null));
                            if (!string.IsNullOrEmpty(userAccess.User.DefaultRegion))
                                effectiveRegion = userAccess.User.DefaultRegion;
                        }

                        Uri baseUri;
                        switch (effectiveRegion.ToUpperInvariant())
                        {
                        case "ORD":
                            if (_internalUrl)
                                baseUri = new Uri("https://snet-ord.queues.api.rackspacecloud.com/");
                            else
                                baseUri = new Uri("https://preview.queue.api.rackspacecloud.com/");
                            break;

                        case "LON":
                            if (_internalUrl)
                                baseUri = new Uri("https://snet-lon.queues.api.rackspacecloud.com/");
                            else
                                baseUri = new Uri("https://lon.queues.api.rackspacecloud.com/");
                            break;

                        default:
                            throw;
                        }

                        _baseUri = baseUri;
                        return baseUri;
                    }
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
                    request.Headers["Client-Id"] = _clientId.ToString("D");
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
                    request.Headers["Client-Id"] = _clientId.ToString("D");
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
            //Func<Task<WebResponse>, HttpWebResponse> checkResult =
            //    task =>
            //    {
            //        return (HttpWebResponse)task.Result;
            //    };
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
                        //.ContinueWith(checkResult)
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
