namespace net.openstack.Providers.Rackspace.Objects
{
    using System;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class SessionPersistence
    {
        [JsonProperty("sessionPersistence")]
        private SessionPersistenceBody _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPersistence"/> class during
        /// JSON deserialization.
        /// </summary>
        [JsonConstructor]
        protected SessionPersistence()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPersistence"/> class using
        /// the specified persistence type.
        /// </summary>
        /// <param name="persistenceType">The session persistence mode to use.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="persistenceType"/> is <c>null</c>.</exception>
        public SessionPersistence(SessionPersistenceType persistenceType)
        {
            if (persistenceType == null)
                throw new ArgumentNullException("persistenceType");

            _body = new SessionPersistenceBody(persistenceType);
        }

        public SessionPersistenceType PersistenceType
        {
            get
            {
                if (_body == null)
                    return null;

                return _body.PersistenceType;
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        protected class SessionPersistenceBody
        {
            /// <summary>
            /// This is the backing field for the <see cref="SessionPersistenceType"/> property.
            /// </summary>
            [JsonProperty("persistenceType")]
            private SessionPersistenceType _persistenceType;

            /// <summary>
            /// Initializes a new instance of the <see cref="SessionPersistenceBody"/> class during
            /// JSON deserialization.
            /// </summary>
            [JsonConstructor]
            protected SessionPersistenceBody()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SessionPersistenceBody"/> class
            /// using the specified persistence type.
            /// </summary>
            /// <param name="persistenceType">The session persistence mode to use.</param>
            /// <exception cref="ArgumentNullException">If <paramref name="persistenceType"/> is <c>null</c>.</exception>
            protected internal SessionPersistenceBody(SessionPersistenceType persistenceType)
            {
                if (persistenceType == null)
                    throw new ArgumentNullException("persistenceType");

                _persistenceType = persistenceType;
            }

            public SessionPersistenceType PersistenceType
            {
                get
                {
                    return _persistenceType;
                }
            }
        }
    }
}
