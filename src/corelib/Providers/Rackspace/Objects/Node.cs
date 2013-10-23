namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Node : NodeConfiguration
    {
        [JsonProperty("id")]
        private string _id;

        [JsonProperty("status")]
        private NodeStatus _status;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public NodeStatus Status
        {
            get
            {
                return _status;
            }
        }
    }
}
