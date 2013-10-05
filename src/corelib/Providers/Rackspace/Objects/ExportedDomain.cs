namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class ExportedDomain : SerializedDomain
    {
        [JsonProperty("accountId")]
        private string _accountId;

        [JsonProperty("id")]
        private string _id;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string AccountId
        {
            get
            {
                return _accountId;
            }
        }
    }
}
