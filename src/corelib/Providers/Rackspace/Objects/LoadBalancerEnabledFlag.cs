namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerEnabledFlag
    {
        [JsonProperty("enabled")]
        private bool _enabled;

        [JsonConstructor]
        protected LoadBalancerEnabledFlag()
        {
        }

        public LoadBalancerEnabledFlag(bool enabled)
        {
            _enabled = enabled;
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
        }
    }
}
