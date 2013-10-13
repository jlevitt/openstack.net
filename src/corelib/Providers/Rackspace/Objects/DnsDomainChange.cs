namespace net.openstack.Providers.Rackspace.Objects
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class DnsDomainChange
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value
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
#pragma warning restore 649

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
