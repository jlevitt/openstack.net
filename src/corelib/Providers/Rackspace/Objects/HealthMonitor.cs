namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(HealthMonitor.Converter))]
    public abstract class HealthMonitor
    {
        [JsonProperty("type")]
        private HealthMonitorType _type;

        [JsonProperty("timeout")]
        private int? _timeout;

        [JsonProperty("delay")]
        private int? _delay;

        [JsonProperty("attemptsBeforeDeactivation")]
        private int? _attemptsBeforeDeactivation;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthMonitor"/> class during JSON deserialization.
        /// </summary>
        protected HealthMonitor()
        {
        }

        protected HealthMonitor(HealthMonitorType type, int attemptsBeforeDeactivation, TimeSpan timeout, TimeSpan delay)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (attemptsBeforeDeactivation <= 0)
                throw new ArgumentOutOfRangeException("attemptsBeforeDeactivation");
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("timeout");
            if (delay <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("delay");

            _type = type;
            _attemptsBeforeDeactivation = attemptsBeforeDeactivation;
            _timeout = (int)timeout.TotalSeconds;
            _delay = (int)delay.TotalSeconds;
        }

        protected class Converter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
