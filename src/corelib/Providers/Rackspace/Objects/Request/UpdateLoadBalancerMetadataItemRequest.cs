namespace net.openstack.Providers.Rackspace.Objects.Request
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class UpdateLoadBalancerMetadataItemRequest
    {
        [JsonProperty("meta")]
        private UpdateMetadataItemRequestBody _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLoadBalancerMetadataItemRequest"/> class
        /// with the specified metadata value.
        /// </summary>
        /// <param name="value">The updated metadata value.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <c>null</c>.</exception>
        public UpdateLoadBalancerMetadataItemRequest(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _body = new UpdateMetadataItemRequestBody(value);
        }

        [JsonObject(MemberSerialization.OptIn)]
        protected class UpdateMetadataItemRequestBody
        {
            [JsonProperty("value")]
            private string _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="UpdateMetadataItemRequestBody"/> class
            /// with the specified metadata value.
            /// </summary>
            /// <param name="value">The updated metadata value.</param>
            /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <c>null</c>.</exception>
            public UpdateMetadataItemRequestBody(string value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _value = value;
            }
        }
    }
}
