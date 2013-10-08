namespace net.openstack.Providers.Rackspace.Objects.Request
{
    using System;
    using Newtonsoft.Json;

    /// <preliminary/>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClaimMessagesRequest
    {
        [JsonProperty("ttl")]
        private long _ttl;

        [JsonProperty("grace")]
        private long _gracePeriod;

        public ClaimMessagesRequest(TimeSpan timeToLive, TimeSpan gracePeriod)
        {
            if (timeToLive < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("timeToLive");
            if (gracePeriod < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("gracePeriod");

            _ttl = (long)timeToLive.TotalSeconds;
            _gracePeriod = (long)gracePeriod.TotalSeconds;
        }
    }
}
