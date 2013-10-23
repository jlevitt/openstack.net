namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerTimestamp
    {
        [JsonProperty("time")]
        private DateTimeOffset? _time;

        public DateTimeOffset? Time
        {
            get
            {
                return _time;
            }
        }
    }
}
