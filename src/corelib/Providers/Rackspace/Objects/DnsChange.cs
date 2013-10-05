namespace net.openstack.Providers.Rackspace.Objects
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a change made to a DNS record.
    /// </summary>
    /// <seealso cref="DnsDomainChange"/>
    /// <seealso cref="IDnsService.ListDomainChangesAsync"/>
    [JsonObject(MemberSerialization.OptIn)]
    public class DnsChange
    {
#pragma warning disable 649 // Field 'fieldName' is never assigned to, and will always have its default value {value}
        /// <summary>
        /// This is the backing field for the <see cref="Field"/> property.
        /// </summary>
        [JsonProperty("field")]
        private string _field;

        /// <summary>
        /// This is the backing field for the <see cref="OriginalValue"/> property.
        /// </summary>
        [JsonProperty("originalValue")]
        private string _originalValue;

        /// <summary>
        /// This is the backing field for the <see cref="NewValue"/> property.
        /// </summary>
        [JsonProperty("newValue")]
        private string _newValue;
#pragma warning restore 649

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsChange"/> class during
        /// JSON deserialization.
        /// </summary>
        [JsonConstructor]
        protected DnsChange()
        {
        }

        /// <summary>
        /// Gets the name of the field which changed.
        /// </summary>
        public string Field
        {
            get
            {
                return _field;
            }
        }

        /// <summary>
        /// Gets the value of the field before the change was made.
        /// </summary>
        public string OriginalValue
        {
            get
            {
                return _originalValue;
            }
        }

        /// <summary>
        /// Gets the value of the field after the change was made.
        /// </summary>
        public string NewValue
        {
            get
            {
                return _newValue;
            }
        }
    }
}
