namespace net.openstack.Providers.Rackspace.Objects.Response
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLoadBalancerMetadataResponse
    {
        /// <summary>
        /// This is the backing field for the <see cref="Metadata"/> property.
        /// </summary>
        [JsonProperty("metadata")]
        private LoadBalancerMetadataItem[] _metadata;

        public ReadOnlyCollection<LoadBalancerMetadataItem> Metadata
        {
            get
            {
                if (_metadata == null)
                    return null;

                return new ReadOnlyCollection<LoadBalancerMetadataItem>(_metadata);
            }
        }
    }
}
