namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using net.openstack.Core.Domain;
    using Newtonsoft.Json;

    /// <preliminary/>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ListCloudQueuesResponse
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value {value}
        [JsonProperty("links")]
        private Link[] _links;

        [JsonProperty("queues")]
        private CloudQueue[] _queues;
#pragma warning restore 649

        public Link[] Links
        {
            get
            {
                return _links;
            }
        }

        public CloudQueue[] Queues
        {
            get
            {
                return _queues;
            }
        }
    }
}
