namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class GetLoadBalancerMetadataItemResponse
    {
        /// <summary>
        /// This is the backing field for the <see cref="MetadataItem"/> property.
        /// </summary>
        [JsonProperty("meta")]
        private LoadBalancerMetadataItem _metadataItem;

        public LoadBalancerMetadataItem MetadataItem
        {
            get
            {
                return _metadataItem;
            }
        }
    }
}
