namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Core;
    using net.openstack.Providers.Rackspace.Objects;

    public interface ILoadBalancerService
    {
        #region Load Balancers

        /// <summary>
        /// Gets a collection of current load balancers.
        /// </summary>
        /// <param name="markerId">The <see cref="LoadBalancer.Id"/> of the last item in the previous list. Used for <see href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Paginated_Collections-d1e786.html">pagination</see>. If the value is <c>null</c>, the list starts at the beginning.</param>
        /// <param name="limit">Indicates the maximum number of items to return. Used for <see href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Paginated_Collections-d1e786.html">pagination</see>. If the value is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a collection of
        /// <see cref="LoadBalancer"/> objects describing the current load balancers.
        /// </returns>
        /// <exception cref="ArgumentException">If <paramref name="markerId"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="limit"/> is less than or equal to 0.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancers-d1e1367.html">List Load Balancers (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<LoadBalancer>> ListLoadBalancersAsync(string markerId, int? limit, CancellationToken cancellationToken);

        /// <summary>
        /// Gets detailed information about a specific load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a <see cref="LoadBalancer"/>
        /// object containing detailed information about the specified load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancer_Details-d1e1522.html">List Load Balancer Details (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<LoadBalancer> GetLoadBalancerAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new load balancer.
        /// </summary>
        /// <param name="configuration">The configuration for the new load balancer.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="DnsCompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the task completes successfully,
        /// the <see cref="Task{TResult}.Result"/> property will return a <see cref="LoadBalancer"/> object
        /// describing the new load balancer. If <paramref name="completionOption"/> is
        /// <see cref="DnsCompletionOption.RequestCompleted"/>, the task will not be considered complete until
        /// the load balancer transitions out of the <see cref="LoadBalancerStatus.Build"/> state.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerConfiguration"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="completionOption"/> is not a valid <see cref="DnsCompletionOption"/>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancer_Details-d1e1522.html">List Load Balancer Details (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<LoadBalancer> CreateLoadBalancerAsync(LoadBalancerConfiguration configuration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        Task UpdateLoadBalancerAsync();

        /// <summary>
        /// Removes a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Load_Balancer-d1e2093.html">Remove Load Balancer (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveLoadBalancerAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        /// <summary>
        /// Removes one or more load balancers.
        /// </summary>
        /// <param name="loadBalancerIds">The IDs of load balancers to remove. These is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerIds"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerIds"/> contains any <c>null</c> or empty values.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Load_Balancer-d1e2093.html">Remove Load Balancer (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveLoadBalancerRangeAsync(IEnumerable<string> loadBalancerIds, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer[]> progress);

        #endregion Load Balancers

        #region Error Page

        /// <summary>
        /// Gets the HTML content of the page which is shown to an end user who is attempting to access a load balancer node that is offline or unavailable.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain the HTML content
        /// of the error page which is shown to an end user who is attempting to access a load balancer
        /// node that is offline or unavailable.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Errorpage-d1e2218.html">Error Page Operations (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<string> GetErrorPageAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the HTML content of the custom error page which is shown to an end user who is attempting to access a load balancer node that is offline or unavailable.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="content">The HTML content of the error page which is shown to an end user who is attempting to access a load balancer node that is offline or unavailable.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="DnsCompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="content"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="content"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Errorpage-d1e2218.html">Error Page Operations (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task SetErrorPageAsync(string loadBalancerId, string content, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        /// <summary>
        /// Removes the custom error page which is shown to an end user who is attempting to access a load balancer node that is offline or unavailable.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="completionOption">Specifies when the <see cref="Task"/> representing the asynchronous server operation should be considered complete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="progress">An optional callback object to receive progress notifications, if <paramref name="completionOption"/> is <see cref="DnsCompletionOption.RequestCompleted"/>. If this is <c>null</c>, no progress notifications are sent.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Errorpage-d1e2218.html">Error Page Operations (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveErrorPageAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        #endregion Error Page

        #region Load Balancer Statistics

        /// <summary>
        /// Get detailed statistics for a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a
        /// <see cref="LoadBalancerStatistics"/> object containing the detailed statistics for the
        /// load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Load_Balancer_Stats-d1e1524.html">List Load Balancer Stats (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<LoadBalancerStatistics> GetStatisticsAsync(string loadBalancerId, CancellationToken cancellationToken);

        #endregion Load Balancer Statistics

        #region Nodes

        /// <summary>
        /// List the load balancer nodes associated with a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a collection of
        /// <see cref="Node"/> objects describing the load balancer nodes associated with the specified
        /// load balancer.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Nodes-d1e2218.html">List Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<Node>> ListNodesAsync(string loadBalancerId, CancellationToken cancellationToken);

        /// <summary>
        /// Get detailed information about a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node ID. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a <see cref="Node"/>
        /// object describing the specified load balancer node.
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
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/List_Nodes-d1e2218.html">List Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<Node> GetNodeAsync(string loadBalancerId, string nodeId, CancellationToken cancellationToken);

        /// <summary>
        /// Add a node to a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeConfiguration">A <see cref="NodeConfiguration"/> object describing the load balancer node to add.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a <see cref="Node"/>
        /// object describing the new load balancer node.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeConfiguration"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="loadBalancerId"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Add_Nodes-d1e2379.html">Add Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<Node> AddNodeAsync(string loadBalancerId, NodeConfiguration nodeConfiguration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node> progress);

        /// <summary>
        /// Add one or more nodes to a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeConfiguration">A collection of <see cref="NodeConfiguration"/> objects describing the load balancer nodes to add.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a collection of
        /// <see cref="Node"/> objects describing the new load balancer nodes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeConfiguration"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeConfiguration"/> contains any <c>null</c> values.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Add_Nodes-d1e2379.html">Add Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<Node>> AddNodeRangeAsync(string loadBalancerId, IEnumerable<NodeConfiguration> nodeConfiguration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node[]> progress);

        /// <summary>
        /// Update the configuration of a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node IDs. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="condition">The new condition for the node, which determines its role within the load balancer. If this value is <c>null</c>, the existing value for the node is not changed.</param>
        /// <param name="type">The type of the node. If this value is <c>null</c>, the existing value for the node is not changed.</param>
        /// <param name="weight">The weight of the node (for weighted load balancer algorithms). If this value is <c>null</c>, the existing value for the node is not changed.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
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
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="weight"/> is less than 0.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Modify_Nodes-d1e2503.html">Modify Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task UpdateNodeAsync(string loadBalancerId, string nodeId, NodeCondition condition, NodeType type, int? weight, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node> progress);

        /// <summary>
        /// Remove a nodes from a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeId">The load balancer node IDs. This is obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
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
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Nodes-d1e2675.html">Remove Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveNodeAsync(string loadBalancerId, string nodeId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node> progress);

        /// <summary>
        /// Remove one or more nodes from a load balancer.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="nodeIds">The load balancer node IDs of nodes to remove. These are obtained from <see cref="Node.Id">Node.Id</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="loadBalancerId"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="nodeIds"/> contains any <c>null</c> or empty values.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Remove_Nodes-d1e2675.html">Remove Nodes (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task RemoveNodeRangeAsync(string loadBalancerId, IEnumerable<string> nodeIds, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<Node[]> progress);

        /// <summary>
        /// List the service events for a load balancer node.
        /// </summary>
        /// <param name="loadBalancerId">The load balancer ID. This is obtained from <see cref="LoadBalancer.Id">LoadBalancer.Id</see>.</param>
        /// <param name="marker">The <see cref="NodeServiceEvent.Id"/> of the last item in the previous list. Used for <see href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Paginated_Collections-d1e786.html">pagination</see>. If the value is <c>null</c>, the list starts at the beginning.</param>
        /// <param name="limit">Indicates the maximum number of items to return. Used for <see href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Paginated_Collections-d1e786.html">pagination</see>. If the value is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a collection of
        /// <see cref="NodeService"/> objects describing the service events for the load balancer node.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="loadBalancerId"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="loadBalancerId"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="marker"/> is empty.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="limit"/> is less than or equal to 0.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Node-Events-d1e264.html">View Node Service Events (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<NodeServiceEvent>> ListNodeServiceEvents(string loadBalancerId, string marker, int? limit, CancellationToken cancellationToken);

        #endregion Nodes

        #region Virtual IPs

        Task ListVirtualAddressesAsync(string loadBalancerId, CancellationToken cancellationToken);

        //Task AddVirtualAddressAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        Task RemoveVirtualAddressAsync(string loadBalancerId, string virtualAddressId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        Task RemoveVirtualAddressRangeAsync(string loadBalancerId, IEnumerable<string> virtualAddressId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

        #endregion Virtual IPs

        #region Allowed Domains

        /// <summary>
        /// Gets the domain name restrictions in place for adding load balancer nodes.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. When the operation
        /// completes, the <see cref="Task{TResult}.Result"/> property will contain a collection of
        /// strings containing the allowed domain names used for adding load balancer nodes.
        /// </returns>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/loadbalancers/api/v1.0/clb-devguide/content/Node-Events-d1e264.html">View Node Service Events (Rackspace Cloud Load Balancers Developer Guide - API v1.0)</seealso>
        Task<IEnumerable<string>> ListAllowedDomainsAsync(CancellationToken cancellationToken);

        #endregion Allowed Domains

        #region Usage Reports

        Task<IEnumerable<LoadBalancer>> ListBillableLoadBalancersAsync(DateTimeOffset startTime, DateTimeOffset endTime, int? offset, int? limit, CancellationToken cancellationToken);

        Task<IEnumerable<LoadBalancerUsage>> ListAccountLevelUsageAsync(DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken);

        Task<IEnumerable<LoadBalancerUsage>> ListHistoricalUsageAsync(string loadBalancerId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken);

        Task<IEnumerable<LoadBalancerUsage>> ListCurrentUsageAsync(string loadBalancerId, CancellationToken cancellationToken);

        #endregion Usage Reports

        #region Access Lists

        Task<IEnumerable<NetworkItem>> ListAccessListAsync(string loadBalancerId, CancellationToken cancellationToken);

        Task CreateAccessListAsync(string loadBalancerId, NetworkItem networkItem, CancellationToken cancellationToken);

        Task CreateAccessListAsync(string loadBalancerId, IEnumerable<NetworkItem> networkItems, CancellationToken cancellationToken);

        Task RemoveAccessListAsync(string loadBalancerId, string networkItemId, CancellationToken cancellationToken);

        Task RemoveAccessListAsync(string loadBalancerId, IEnumerable<string> networkItemIds, CancellationToken cancellationToken);

        Task ClearAccessListAsync(string loadBalancerId, CancellationToken cancellationToken);

        #endregion Access Lists

        #region Monitors

        Task GetHealthMonitorAsync();

        Task SetHealthMonitorAsync(string loadBalancerId, HealthMonitor monitor, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<HealthMonitor> progress);

        Task RemoveHealthMonitorAsync();

        #endregion Monitors

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
        Task SetSessionPersistenceAsync(string loadBalancerId, SessionPersistence sessionPersistence, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

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
        Task RemoveSessionPersistenceAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

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
        Task SetConnectionLoggingAsync(string loadBalancerId, bool enabled, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

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
        Task UpdateThrottlesAsync(string loadBalancerId, ConnectionThrottles throttleConfiguration, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

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
        Task RemoveThrottlesAsync(string loadBalancerId, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

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
        Task SetContentCachingAsync(string loadBalancerId, bool enabled, DnsCompletionOption completionOption, CancellationToken cancellationToken, IProgress<LoadBalancer> progress);

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

        #region SSL Termination

        #endregion SSL Termination

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
