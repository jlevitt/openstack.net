namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class SerializedDomain
    {
        [JsonProperty("content")]
        private string _content;

        [JsonProperty("contentType")]
        private string _contentType;

        public string Content
        {
            get
            {
                return _content;
            }
        }

        public string ContentType
        {
            get
            {
                return _contentType;
            }
        }
    }
}
