namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Providers.Rackspace.Objects;

    public interface ILoadBalancerService
    {
        #region Sessions

        /// <summary>
        /// Gets the session persistence configuration for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a <see cref="SessionPersistence"/>
        /// object describing the session persistence configuration for the load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Manage_Session_Persistence-d1e3733.html">Manage Session Persistence (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<SessionPersistence> GetSessionPersistenceAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the session persistence configuration for a load balancer.
        /// </summary>
        /// <remarks>
        /// You can only set one of the session persistence modes on a load balancer, and it can only support one
        /// protocol, so if you set <see cref="SessionPersistenceType.HttpCookie"/> mode for an HTTP load balancer,
        /// then it will support session persistence for HTTP requests only. Likewise, if you set
        /// <see cref="SessionPersistenceType.SourceAddress"/> mode for an HTTPS load balancer, then it will support
        /// session persistence for HTTPS requests only.
        ///
        /// <para>
        /// If you want to support session persistence for both HTTP and HTTPS requests concurrently, then you have 2 choices:
        /// </para>
        ///
        /// <list type="bullet">
        /// <item>Use two load balancers, one configured for session persistence for HTTP requests and the other
        /// configured for session persistence for HTTPS requests. That way, the load balancers together will support
        /// session persistence for both HTTP and HTTPS requests concurrently, with each load balancer supporting one
        /// of the protocols.</item>
        /// <item>Use one load balancer, configure it for session persistence for HTTP requests, and then enable SSL
        /// termination for that load balancer (refer to Section 4.17, "SSL Termination" for details). The load
        /// balancer will then support session persistence for both HTTP and HTTPS requests concurrently.</item>
        /// </list>
        /// </remarks>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="sessionPersistence">The session persistence configuration.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="sessionPersistence"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Manage_Session_Persistence-d1e3733.html">Manage Session Persistence (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task SetSessionPersistenceAsync(string loadBalancerId, SessionPersistence sessionPersistence, CancellationToken cancellationToken);

        /// <summary>
        /// Removes the session persistence configuration for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Manage_Session_Persistence-d1e3733.html">Manage Session Persistence (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveSessionPersistenceAsync(string loadBalancerId, CancellationToken cancellationToken);

        #endregion Sessions

        #region Connections

        /// <summary>
        /// Gets whether or not connection logging is enabled for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will be <c>true</c> if content
        /// caching is enabled for the load balancer; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Log_Connections-d1e3924.html">Log Connections (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<bool> GetConnectionLoggingAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Enables or disables connection logging for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="enabled"><c>true</c> to enable connection logging on the load balancer; otherwise, <c>false</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Log_Connections-d1e3924.html">Log Connections (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task SetConnectionLoggingAsync(string loadBalancerId, bool enabled, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the connection throttling configuration for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a <see cref="ConnectionThrottles"/>
        /// object describing the connection throttling configuration in effect on the load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Throttle_Connections-d1e4057.html">Throttle Connections (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<ConnectionThrottles> ListThrottlesAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the connection throttling configuration for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="throttleConfiguration">A <see cref="ConnectionThrottles"/> object describing the throttling configuration to apply for the load balancer.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="throttleConfiguration"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Throttle_Connections-d1e4057.html">Throttle Connections (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task UpdateThrottlesAsync(string loadBalancerId, ConnectionThrottles throttleConfiguration, CancellationToken cancellationToken);

        /// <summary>
        /// Removes the connection throttling configuration for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Throttle_Connections-d1e4057.html">Throttle Connections (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveThrottlesAsync(string loadBalancerId, CancellationToken cancellationToken);

        #endregion Connections

        #region Content Caching

        /// <summary>
        /// Gets whether or not content caching is enabled for a load balancer.
        /// </summary>
        /// <remarks>
        /// When content caching is enabled, recently-accessed files are stored on the load balancer
        /// for easy retrieval by web clients. Content caching improves the performance of high
        /// traffic web sites by temporarily storing data that was recently accessed. While it's
        /// cached, requests for that data will be served by the load balancer, which in turn reduces
        /// load off the back end nodes. The result is improved response times for those requests and
        /// less load on the web server.
        ///
        /// <para>
        /// For more information about content caching, refer to the following Knowledge Center
        /// article:
        /// <see href="http://www.rackspace.com/knowledge_center/content/content-caching-cloud-load-balancers">Content Caching for Cloud Load Balancers</see>.
        /// </para>
        /// </remarks>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will be <c>true</c> if content
        /// caching is enabled for the load balancer; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/ContentCaching-d1e3358.html">Content Caching (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<bool> GetContentCachingAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Enables or disables content caching for a load balancer.
        /// </summary>
        /// <remarks>
        /// When content caching is enabled, recently-accessed files are stored on the load balancer
        /// for easy retrieval by web clients. Content caching improves the performance of high
        /// traffic web sites by temporarily storing data that was recently accessed. While it's
        /// cached, requests for that data will be served by the load balancer, which in turn reduces
        /// load off the back end nodes. The result is improved response times for those requests and
        /// less load on the web server.
        ///
        /// <para>
        /// For more information about content caching, refer to the following Knowledge Center
        /// article:
        /// <see href="http://www.rackspace.com/knowledge_center/content/content-caching-cloud-load-balancers">Content Caching for Cloud Load Balancers</see>.
        /// </para>
        /// </remarks>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="enabled"><c>true</c> to enable content caching on the load balancer; otherwise, <c>false</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/ContentCaching-d1e3358.html">Content Caching (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task SetContentCachingAsync(string loadBalancerId, bool enabled, CancellationToken cancellationToken);

        #endregion Content Caching

        #region Protocols

        /// <summary>
        /// Gets a collection of supported load balancing protocols.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a collection of <see cref="LoadBalancingProtocol"/> objects describing the load balancing
        /// protocols supported by this service.
        /// </returns>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancing_Protocols-d1e4269.html">List Load Balancing Protocols (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancingProtocol>> ListProtocolsAsync(CancellationToken cancellationToken);

        #endregion Protocols

        #region Algorithms

        /// <summary>
        /// Gets a collection of supported load balancing algorithms.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a collection of <see cref="LoadBalancingAlgorithm"/> objects describing the load balancing
        /// algorithms supported by this service.
        /// </returns>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancing_Algorithms-d1e4459.html">List Load Balancing Algorithms (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancingAlgorithm>> ListAlgorithmsAsync(CancellationToken cancellationToken);

        #endregion Algorithms

        #region Metadata

        /// <summary>
        /// Gets the metadata associated with a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a collection of <see cref="LoadBalancerMetadataItem"/> objects describing the metadata
        /// associated with a load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Metadata-d1e2218.html">List Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancerMetadataItem>> ListLoadBalancerMetadataAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a specific metadata item associated with a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="metadataId">The metadata item ID. This is obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a <see cref="LoadBalancerMetadataItem"/> object describing the metadata item.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Metadata-d1e2218.html">List Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<LoadBalancerMetadataItem> GetLoadBalancerMetadataItemAsync(string loadBalancerId, string metadataId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the metadata associated with a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a collection of <see cref="LoadBalancerMetadataItem"/> objects describing the metadata
        /// associated with the load balancer node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Metadata-d1e2218.html">List Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancerMetadataItem>> ListNodeMetadataAsync(string loadBalancerId, string nodeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a specific metadata item associated with a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="metadataId">The metadata item ID. This is obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a <see cref="LoadBalancerMetadataItem"/> object describing the metadata item.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is empty.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Metadata-d1e2218.html">List Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<LoadBalancerMetadataItem> GetNodeMetadataItemAsync(string loadBalancerId, string nodeId, string metadataId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the metadata associated with a load balancer.
        /// </summary>
        /// <remarks>
        /// <note type="warning">
        /// The behavior is unspecified if <paramref name="metadata"/> contains a pair whose key matches the name of an existing metadata item associated with the load balancer.
        /// </note>
        /// </remarks>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="metadata">A collection of metadata items to associate with the load balancer.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a collection of <see cref="LoadBalancerMetadataItem"/> objects describing the updated
        /// metadata associated with the load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadata"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadata"/> contains a pair whose <see cref="KeyValuePair{TKey, TValue}.Key"/> is <c>null</c> or empty, or whose <see cref="KeyValuePair{TKey, TValue}.Value"/> is is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Add_Metadata-d1e2379.html">Add Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancerMetadataItem>> AddLoadBalancerMetadataAsync(string loadBalancerId, IEnumerable<KeyValuePair<string, string>> metadata, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the metadata associated with a load balancer node.
        /// </summary>
        /// <remarks>
        /// <note type="warning">
        /// The behavior is unspecified if <paramref name="metadata"/> contains a pair whose key matches the name of an existing metadata item associated with the node.
        /// </note>
        /// </remarks>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="metadata">A collection of metadata items to associate with the node.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes successfully, the <see cref="Task{TResult}.Result"/> property will contain a
        /// a collection of <see cref="LoadBalancerMetadataItem"/> objects describing the updated
        /// metadata associated with the load balancer node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadata"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is empty.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadata"/> contains a pair whose <see cref="KeyValuePair{TKey, TValue}.Key"/> is <c>null</c> or empty, or whose <see cref="KeyValuePair{TKey, TValue}.Value"/> is is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Add_Metadata-d1e2379.html">Add Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancerMetadataItem>> AddNodeMetadataAsync(string loadBalancerId, string nodeId, IEnumerable<KeyValuePair<string, string>> metadata, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the value for a metadata item associated with a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="metadataId">The metadata item ID. This is obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="value">The new value for the metadata item.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="value"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Modify_Metadata-d1e2503.html">Modify Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task UpdateLoadBalancerMetadataItemAsync(string loadBalancerId, string metadataId, string value, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the value for a metadata item associated with a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="metadataId">The metadata item ID. This is obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="value">The new value for the metadata item.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="value"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is empty.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Modify_Metadata-d1e2503.html">Modify Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task UpdateNodeMetadataItemAsync(string loadBalancerId, string nodeId, string metadataId, string value, CancellationToken cancellationToken);

        /// <summary>
        /// Removes one or more metadata items associated with a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="metadataIds">The metadata item IDs. These are obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> contains any <c>null</c> or empty values.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Metadata-d1e2675.html">Remove Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveLoadBalancerMetadataItemAsync(string loadBalancerId, IEnumerable<string> metadataIds, CancellationToken cancellationToken);

        /// <summary>
        /// Removes one or more metadata items associated with a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="metadataIds">The metadata item IDs. These are obtained from <see cref="LoadBalancerMetadataItem.Id">LoadBalancerMetadataItem.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeId"/> is empty.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="metadataIds"/> contains any <c>null</c> or empty values.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Metadata-d1e2675.html">Remove Metadata (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveNodeMetadataItemAsync(string loadBalancerId, string nodeId, IEnumerable<string> metadataIds, CancellationToken cancellationToken);

        #endregion Metadata
    }
}
