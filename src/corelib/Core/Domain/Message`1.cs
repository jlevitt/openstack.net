namespace net.openstack.Core.Domain
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Message<T>
    {
        [JsonProperty("ttl")]
        private long _timeToLive;

        [JsonProperty("body")]
        private T _body;

        [JsonConstructor]
        protected Message()
        {
        }

        public Message(TimeSpan timeToLive, T body)
        {
            if (timeToLive < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("timeToLive");

            _timeToLive = (long)timeToLive.TotalSeconds;
            _body = body;
        }

        public TimeSpan TimeToLive
        {
            get
            {
                return TimeSpan.FromSeconds(_timeToLive);
            }
        }

        public T Body
        {
            get
            {
                return _body;
            }
        }
    }
}
