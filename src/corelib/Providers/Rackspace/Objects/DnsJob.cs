namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using JSIStudios.SimpleRESTServices.Client;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsJob
    {
        [JsonProperty("request")]
        private string _request;

        [JsonProperty("status")]
        private DnsJobStatus _status;

        [JsonProperty("verb")]
        [JsonConverter(typeof(StringEnumConverter))]
        private HttpMethod? _verb;

        [JsonProperty("jobId")]
        private string _jobId;

        [JsonProperty("callbackUrl")]
        private string _callbackUrl;

        [JsonProperty("requestUrl")]
        private string _requestUrl;

        [JsonProperty("error")]
        private JObject _error;

        public string Id
        {
            get
            {
                return _jobId;
            }
        }

        public DnsJobStatus Status
        {
            get
            {
                return _status;
            }
        }

        public Uri CallbackUri
        {
            get
            {
                if (_callbackUrl == null)
                    return null;

                return new Uri(_callbackUrl);
            }
        }

        public Uri RequestUri
        {
            get
            {
                if (_requestUrl == null)
                    return null;

                return new Uri(_requestUrl);
            }
        }

        public HttpMethod? Verb
        {
            get
            {
                return _verb;
            }
        }

        public string Request
        {
            get
            {
                return _request;
            }
        }

        public JObject Error
        {
            get
            {
                return _error;
            }
        }
    }
}
