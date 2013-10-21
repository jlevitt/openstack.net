namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerMetadataItem
    {
        /// <summary>
        /// This is the backing field for the <see cref="Id"/> property.
        /// </summary>
        [JsonProperty("id")]
        private string _id;

        /// <summary>
        /// This is the backing field for the <see cref="Key"/> property.
        /// </summary>
        [JsonProperty("key")]
        private string _key;

        /// <summary>
        /// This is the backing field for the <see cref="Value"/> property.
        /// </summary>
        [JsonProperty("value")]
        private string _value;

        /// <summary>
        /// Gets the unique ID for the metadata item.
        /// </summary>
        /// <remarks>
        /// Metadata IDs in the load balancer service are only guaranteed to be unique within the
        /// context of the item they are associated with.
        /// </remarks>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the key for this metadata item.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }

        /// <summary>
        /// Gets the value for this metadata item.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
        }
    }
}
