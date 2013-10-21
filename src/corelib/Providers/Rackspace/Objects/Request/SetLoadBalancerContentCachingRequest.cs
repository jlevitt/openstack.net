namespace net.openstack.Providers.Rackspace.Objects.Request
{
    using net.openstack.Providers.Rackspace.Objects.Response;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class SetLoadBalancerContentCachingRequest : GetLoadBalancerContentCachingResponse
    {
        public SetLoadBalancerContentCachingRequest(bool enabled)
            : base(enabled)
        {
        }
    }
}
