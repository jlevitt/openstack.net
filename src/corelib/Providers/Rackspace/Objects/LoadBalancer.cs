namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using net.openstack.Core.Domain.Converters;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancer : LoadBalancerConfiguration<Node>
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _id;

        [JsonProperty("cluster", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerCluster _cluster;

        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerStatus _status;

        [JsonProperty("created", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerTimestamp _created;

        [JsonProperty("updated", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private LoadBalancerTimestamp _updated;

        [JsonProperty("sourceAddresses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private Dictionary<string, string> _sourceAddresses;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public LoadBalancerStatus Status
        {
            get
            {
                return _status;
            }
        }

        public DateTimeOffset? Created
        {
            get
            {
                if (_created == null)
                    return null;

                return _created.Time;
            }
        }

        public DateTimeOffset? Updated
        {
            get
            {
                if (_updated == null)
                    return null;

                return _updated.Time;
            }
        }
    }
}
