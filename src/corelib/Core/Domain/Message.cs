namespace net.openstack.Core.Domain
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonObject(MemberSerialization.OptIn)]
    public class Message : Message<JObject>
    {
        [JsonConstructor]
        protected Message()
        {
        }

        public Message(TimeSpan timeToLive, JObject body)
            : base(timeToLive, body)
        {
        }
    }
}
