namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Providers.Rackspace.Objects;

    public interface ILoadBalancerService
    {
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
    }
}
