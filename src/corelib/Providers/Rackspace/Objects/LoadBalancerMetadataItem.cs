namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class LoadBalancerMetadataItem
    {
        /// <summary>
        /// This is the backing field for the <see cref="Id"/> property.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private string _id;

        /// <summary>
        /// This is the backing field for the <see cref="Key"/> property.
        /// </summary>
        [JsonProperty("key", DefaultValueHandling = DefaultValueHandling.Include)]
        private string _key;

        /// <summary>
        /// This is the backing field for the <see cref="Value"/> property.
        /// </summary>
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Include)]
        private string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerMetadataItem"/> class
        /// during JSON deserialization.
        /// </summary>
        [JsonConstructor]
        protected LoadBalancerMetadataItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadBalancerMetadataItem"/> class
        /// with the specified key and value.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="key"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="value"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="key"/> is empty.</exception>
        public LoadBalancerMetadataItem(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key cannot be empty");

            _key = key;
            _value = value;
        }

        /// <summary>
        /// Gets the unique ID for the metadata item.
        /// </summary>
        /// <remarks>
        /// Metadata IDs in the load balancer service are only guaranteed to be unique within the
        /// context of the item they are associated with.
        /// </remarks>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the key for this metadata item.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }

        /// <summary>
        /// Gets the value for this metadata item.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
        }
    }
}
