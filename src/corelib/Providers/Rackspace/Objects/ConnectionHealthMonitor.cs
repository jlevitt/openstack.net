namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class ConnectionHealthMonitor : HealthMonitor
    {
        [JsonConstructor]
        protected ConnectionHealthMonitor()
        {
        }

        public ConnectionHealthMonitor(int attemptsBeforeDeactivation, TimeSpan timeout, TimeSpan delay)
            : base(HealthMonitorType.Connect, attemptsBeforeDeactivation, timeout, delay)
        {
        }
    }
}
