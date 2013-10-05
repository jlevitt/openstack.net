namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsJob<TRespose> : DnsJob
    {
        [JsonProperty("response")]
        private TRespose _response;

        public TRespose Response
        {
            get
            {
                return _response;
            }
        }
    }
}
