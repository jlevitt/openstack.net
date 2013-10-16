namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class SerializedDomain
    {
        [JsonProperty("contents")]
        private string _contents;

        [JsonProperty("contentType")]
        private SerializedDomainFormat _contentType;

        [JsonConstructor]
        protected SerializedDomain()
        {
        }

        public SerializedDomain(string contents, SerializedDomainFormat contentType)
        {
            if (contents == null)
                throw new ArgumentNullException("contents");
            if (contentType == null)
                throw new ArgumentNullException("contentType");
            if (string.IsNullOrEmpty(contents))
                throw new ArgumentException("contents cannot be empty");

            _contents = contents;
            _contentType = contentType;
        }

        public string Contents
        {
            get
            {
                return _contents;
            }
        }

        public SerializedDomainFormat ContentType
        {
            get
            {
                return _contentType;
            }
        }
    }
}
