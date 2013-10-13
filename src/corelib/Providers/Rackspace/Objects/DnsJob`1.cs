namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class DnsJob<TRespose> : DnsJob
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("response")]
        private TRespose _response;
#pragma warning restore 649

        public TRespose Response
        {
            get
            {
                return _response;
            }
        }
    }
}
