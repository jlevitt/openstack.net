namespace net.openstack.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using net.openstack.Core.Domain;
    using Newtonsoft.Json.Linq;
    using CancellationToken = System.Threading.CancellationToken;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1&oldid=30943">OpenStack Marconi API v1 Blueprint</seealso>
    public interface IQueueingService
    {
        #region Base endpoints

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Home_Document">Get Home Document (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<HomeDocument> GetHomeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Checks the queueing service node status.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the asynchronous operation. If the service
        /// is available, the task will complete successfully. If the service is unavailable due
        /// to a storage driver failure or some other error, the will fail and the
        /// <see cref="Task.Exception"/> property will contain the reason for the failure.
        /// </returns>
        /// <seealso href="https://wiki.openstack.org/wiki/Marconi/specs/api/v1#Check_Node_Health">Check Node Health (OpenStack Marconi API v1 Blueprint)</seealso>
        Task GetNodeHealthAsync(CancellationToken cancellationToken);

        #endregion Base endpoints

        #region Queues

        /// <summary>
        /// Creates a queue.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Create_Queue">Create Queue (OpenStack Marconi API v1 Blueprint)</seealso>
        Task CreateQueueAsync(string queueName, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#List_Queues">List Queues (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<IEnumerable<CloudQueue>> ListQueuesAsync(string marker, int? limit, bool detailed, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Checking_Queue_Existence">Checking Queue Existence (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<bool> QueueExistsAsync(string queueName, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Delete_Queue">Delete Queue (OpenStack Marconi API v1 Blueprint)</seealso>
        Task DeleteQueueAsync(string queueName, CancellationToken cancellationToken);

        #endregion

        #region Queue metadata

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Set_Queue_Metadata">Set Queue Metadata (OpenStack Marconi API v1 Blueprint)</seealso>
        Task SetQueueMetadataAsync<T>(string queueName, T metadata, CancellationToken cancellationToken)
            where T : class;

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Queue_Metadata">Get Queue Metadata (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<JObject> GetQueueMetadataAsync(string queueName, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Queue_Metadata">Get Queue Metadata (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<T> GetQueueMetadataAsync<T>(string queueName, CancellationToken cancellationToken)
            where T : class;

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Queue_Stats">Get Queue Stats (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<QueueStatistics> GetQueueStatisticsAsync(string queueName, CancellationToken cancellationToken);

        #endregion Queue metadata

        #region Messages

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="marker"></param>
        /// <param name="limit"></param>
        /// <param name="echo"><c>true</c> to include messages created by the current client; otherwise, <c>false</c>.</param>
        /// <param name="includeClaimed"><c>true</c> to include claimed messages; otherwise <c>false</c> to return only unclaimed messages.</param>
        /// <returns></returns>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#List_Messages">List Messages (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<IEnumerable<QueuedMessage>> ListMessagesAsync(string queueName, string marker, int? limit, bool echo, bool includeClaimed, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_a_Specific_Message">Get a Specific Message (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<QueuedMessage> GetMessageAsync(string queueName, string messageId, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_a_Set_of_Messages_by_ID">Get a Set of Messages by ID (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<IEnumerable<QueuedMessage>> GetMessagesAsync(string queueName, IEnumerable<string> messageIds, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        Task PostMessagesAsync(string queueName, IEnumerable<Message> messages, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        Task PostMessagesAsync(string queueName, CancellationToken cancellationToken, params Message[] messages);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        Task PostMessagesAsync<T>(string queueName, IEnumerable<Message<T>> messages, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        Task PostMessagesAsync<T>(string queueName, CancellationToken cancellationToken, params Message<T>[] messages);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Delete_Message">Delete Message (OpenStack Marconi API v1 Blueprint)</seealso>
        Task DeleteMessageAsync(string queueName, string messageId, Claim claim, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Delete_a_Set_of_Messages_by_ID">Delete a Set of Messages by ID (OpenStack Marconi API v1 Blueprint)</seealso>
        Task DeleteMessagesAsync(string queueName, IEnumerable<string> messageIds, CancellationToken cancellationToken);

        #endregion Messages

        #region Claims

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Claim_Messages">Claim Messages (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<Claim> ClaimMessageAsync(string queueName, int? limit, TimeSpan timeToLive, TimeSpan gracePeriod, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Query_Claim">Query Claim (OpenStack Marconi API v1 Blueprint)</seealso>
        Task<Claim> QueryClaimAsync(string queueName, Claim claim, CancellationToken cancellationToken);

        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Update_Claim">Update Claim (OpenStack Marconi API v1 Blueprint)</seealso>
        Task UpdateClaimAsync(string queueName, Claim claim, TimeSpan timeToLive, CancellationToken cancellationToken);

        /// <summary>
        /// Immediately release a claim, making any (remaining, non-deleted) messages associated
        /// with the claim available to other workers.
        /// </summary>
        /// <param name="queueName">The name of the queue.</param>
        /// <param name="claim">The claim to release.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="claim"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Release_Claim">Release Claim (OpenStack Marconi API v1 Blueprint)</seealso>
        Task ReleaseClaimAsync(string queueName, Claim claim, CancellationToken cancellationToken);

        #endregion
    }
}
