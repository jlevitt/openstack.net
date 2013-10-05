namespace net.openstack.Providers.Rackspace.Objects
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class DnsDomainChange
    {
        [JsonProperty("action")]
        private string _action;

        [JsonProperty("targetType")]
        private string _targetType;

        [JsonProperty("targetId")]
        private string _targetId;

        [JsonProperty("changeDetails")]
        private DnsChange[] _details;

        [JsonProperty("accountId")]
        private string _accountId;

        [JsonProperty("domain")]
        private string _domainName;

        public ReadOnlyCollection<DnsChange> Details
        {
            get
            {
                if (_details == null)
                    return null;

                return new ReadOnlyCollection<DnsChange>(_details);
            }
        }
    }
}
