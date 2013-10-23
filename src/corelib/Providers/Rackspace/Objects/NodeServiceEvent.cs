namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class NodeServiceEvent
    {
        [JsonProperty("id")]
        private string _id;

        [JsonProperty("loadBalancerId")]
        private string _loadBalancerId;

        [JsonProperty("nodeId")]
        private string _nodeId;

        [JsonProperty("detailedMessage")]
        private string _detailedMessage;

        [JsonProperty("type")]
        private NodeServiceEventType _type;

        [JsonProperty("category")]
        private NodeServiceEventCategory _category;

        [JsonProperty("severity")]
        private NodeServiceEventSeverity _severity;

        [JsonProperty("description")]
        private string _description;

        [JsonProperty("relativeUri")]
        private string _relativeUri;

        [JsonProperty("accountId")]
        private string _accountId;

        [JsonProperty("title")]
        private string _title;

        [JsonProperty("author")]
        private string _author;

        [JsonProperty("created")]
        private DateTimeOffset? _created;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string LoadBalancerId
        {
            get
            {
                return _loadBalancerId;
            }
        }

        public string NodeId
        {
            get
            {
                return _nodeId;
            }
        }

        public string DetailedMessage
        {
            get
            {
                return _detailedMessage;
            }
        }

        public NodeServiceEventType Type
        {
            get
            {
                return _type;
            }
        }

        public NodeServiceEventCategory Category
        {
            get
            {
                return _category;
            }
        }

        public NodeServiceEventSeverity Severity
        {
            get
            {
                return _severity;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public Uri RelativeUri
        {
            get
            {
                if (_relativeUri == null)
                    return null;

                return new Uri(_relativeUri);
            }
        }

        public string AccountId
        {
            get
            {
                return _accountId;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
        }

        public DateTimeOffset? Created
        {
            get
            {
                return _created;
            }
        }
    }
}
