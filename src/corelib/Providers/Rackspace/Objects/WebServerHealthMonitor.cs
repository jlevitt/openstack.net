namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class WebServerHealthMonitor : HealthMonitor
    {
        /// <summary>
        /// This is the backing field for the <see cref="BodyRegex"/> property.
        /// </summary>
        [JsonProperty("bodyRegex")]
        private string _bodyRegex;

        /// <summary>
        /// This is the backing field for the <see cref="Path"/> property.
        /// </summary>
        [JsonProperty("path")]
        private string _path;

        /// <summary>
        /// This is the backing field for the <see cref="StatusRegex"/> property.
        /// </summary>
        [JsonProperty("statusRegex")]
        private string _statusRegex;

        /// <summary>
        /// This is the backing field for the <see cref="HostHeader"/> property.
        /// </summary>
        [JsonProperty("hostHeader", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _hostHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerHealthMonitor"/> class
        /// during JSON deserialization.
        /// </summary>
        [JsonConstructor]
        protected WebServerHealthMonitor()
        {
        }

        public WebServerHealthMonitor(bool https, int attemptsBeforeDeactivation, TimeSpan timeout, TimeSpan delay, string bodyRegex, string path, string statusRegex)
            : this(https, attemptsBeforeDeactivation, timeout, delay, bodyRegex, path, statusRegex, null)
        {
        }

        public WebServerHealthMonitor(bool https, int attemptsBeforeDeactivation, TimeSpan timeout, TimeSpan delay, string bodyRegex, string path, string statusRegex, string hostHeader)
            : base(https ? HealthMonitorType.Https : HealthMonitorType.Http, attemptsBeforeDeactivation, timeout, delay)
        {
            _bodyRegex = bodyRegex;
            _path = path;
            _statusRegex = statusRegex;
            _hostHeader = hostHeader;
        }

        /// <summary>
        /// Gets a regular expression that will be used to evaluate the contents of the body of the response.
        /// </summary>
        public string BodyRegex
        {
            get
            {
                return _bodyRegex;
            }
        }

        /// <summary>
        /// Gets the HTTP path that will be used in the sample request.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// Gets a regular expression that will be used to evaluate the HTTP status code returned in the response.
        /// </summary>
        public string StatusRegex
        {
            get
            {
                return _statusRegex;
            }
        }

        /// <summary>
        /// Gets the optional name of a host for which the health monitors will check.
        /// </summary>
        public string HostHeader
        {
            get
            {
                return _hostHeader;
            }
        }
    }
}
