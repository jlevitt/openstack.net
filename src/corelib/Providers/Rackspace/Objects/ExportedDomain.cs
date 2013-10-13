namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class ExportedDomain : SerializedDomain
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
        [JsonProperty("accountId")]
        private string _accountId;

        [JsonProperty("id")]
        private string _id;
#pragma warning restore 649

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
