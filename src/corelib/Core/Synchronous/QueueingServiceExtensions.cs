﻿namespace net.openstack.Core.Synchronous
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using net.openstack.Core.Domain;
    using CancellationToken = System.Threading.CancellationToken;
    using IQueueingService = net.openstack.Core.Providers.IQueueingService;
    using JObject = Newtonsoft.Json.Linq.JObject;
    using JsonSerializationException = Newtonsoft.Json.JsonSerializationException;
    using WebException = System.Net.WebException;

    /// <summary>
    /// Provides extension methods to allow synchronous calls to the methods in <see cref="IQueueingService"/>.
    /// </summary>
    public static class QueueingServiceExtensions
    {
        #region Base endpoints

        /// <summary>
        /// Gets the home document describing the operations supported by the service.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <returns>A <see cref="HomeDocument"/> object describing the operations supported by the service.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Home_Document">Get Home Document (OpenStack Marconi API v1 Blueprint)</seealso>
        public static HomeDocument GetHome(this IQueueingService queueingService)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.GetHomeAsync(CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Checks the queueing service node status.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <returns>
        /// If the service is available, the operation will complete successfully. If the service
        /// is unavailable due to a storage driver failure or some other error, the operation will
        /// fail and the exception will contain the reason for the failure.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/wiki/Marconi/specs/api/v1#Check_Node_Health">Check Node Health (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void GetNodeHealth(this IQueueingService queueingService)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.GetNodeHealthAsync(CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion Base endpoints

        #region Queues

        /// <summary>
        /// Creates a queue, if it does not already exist.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns><c>true</c> if the queue was created by the call, or <c>false</c> if the queue already existed.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Create_Queue">Create Queue (OpenStack Marconi API v1 Blueprint)</seealso>
        public static bool CreateQueue(this IQueueingService queueingService, string queueName)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.CreateQueueAsync(queueName, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Gets a list of queues.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="marker">The name of the last queue in the previous list. The resulting collection of queues will start with the first queue <em>after</em> this value, when sorted using <see cref="StringComparer.Ordinal"/>. If this value is <c>null</c> or empty, the list starts at the beginning.</param>
        /// <param name="limit">The maximum number of queues to return. If this value is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="detailed"><c>true</c> to return detailed information about each queue; otherwise, <c>false</c>.</param>
        /// <returns><placeholder>placeholder</placeholder></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="limit"/> is less than or equal to 0.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#List_Queues">List Queues (OpenStack Marconi API v1 Blueprint)</seealso>
        public static IEnumerable<CloudQueue> ListQueues(this IQueueingService queueingService, string marker, int? limit, bool detailed)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.ListQueuesAsync(marker, limit, detailed, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Checks for the existence of a queue with a particular name.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns><c>true</c> if queue with the specified name exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Checking_Queue_Existence">Checking Queue Existence (OpenStack Marconi API v1 Blueprint)</seealso>
        public static bool QueueExists(this IQueueingService queueingService, string queueName)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.QueueExistsAsync(queueName, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Deletes a queue.
        /// </summary>
        /// <remarks>
        /// The queue will be deleted whether or not it is empty, even if one or more messages in the queue is currently claimed.
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Delete_Queue">Delete Queue (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void DeleteQueue(this IQueueingService queueingService, string queueName)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.DeleteQueueAsync(queueName, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion

        #region Queue metadata

        /// <summary>
        /// Sets the metadata associated with a queue.
        /// </summary>
        /// <typeparam name="T">The type of data to associate with the queue.</typeparam>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="metadata">The metadata to associate with the queue.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Set_Queue_Metadata">Set Queue Metadata (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void SetQueueMetadata<T>(this IQueueingService queueingService, string queueName, T metadata)
            where T : class
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.SetQueueMetadataAsync(queueName, metadata, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Gets the metadata associated with a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>A <see cref="JObject"/> object containing the metadata associated with the queue.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Queue_Metadata">Get Queue Metadata (OpenStack Marconi API v1 Blueprint)</seealso>
        public static JObject GetQueueMetadata(this IQueueingService queueingService, string queueName)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.GetQueueMetadataAsync(queueName, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Gets the metadata associated with a queue, as a strongly-typed object.
        /// </summary>
        /// <typeparam name="T">The type of metadata associated with the queue.</typeparam>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>A deserialized object of type <typeparamref name="T"/> representing the metadata associated with the queue.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="JsonSerializationException">If an error occurs while deserializing the metadata.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Queue_Metadata">Get Queue Metadata (OpenStack Marconi API v1 Blueprint)</seealso>
        public static T GetQueueMetadata<T>(this IQueueingService queueingService, string queueName)
            where T : class
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.GetQueueMetadataAsync<T>(queueName, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Gets statistics for a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>A <see cref="QueueStatistics"/> object containing statistics for the queue.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_Queue_Stats">Get Queue Stats (OpenStack Marconi API v1 Blueprint)</seealso>
        public static QueueStatistics GetQueueStatistics(this IQueueingService queueingService, string queueName)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.GetQueueStatisticsAsync(queueName, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion Queue metadata

        #region Messages

        /// <summary>
        /// Gets a list of messages currently in a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="marker">The ID of the last message received during a previous call to <see cref="ListMessages"/>. If this value is <c>null</c> or empty, the list starts at the beginning.</param>
        /// <param name="limit">The maximum number of messages to return. If this value is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="echo"><c>true</c> to include messages created by the current client; otherwise, <c>false</c>.</param>
        /// <param name="includeClaimed"><c>true</c> to include claimed messages; otherwise <c>false</c> to return only unclaimed messages.</param>
        /// <returns>A collection of <see cref="QueuedMessage"/> objects describing the messages in the queue.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="limit"/> is less than or equal to 0.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#List_Messages">List Messages (OpenStack Marconi API v1 Blueprint)</seealso>
        public static IEnumerable<QueuedMessage> ListMessages(this IQueueingService queueingService, string queueName, string marker, int? limit, bool echo, bool includeClaimed)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.ListMessagesAsync(queueName, marker, limit, echo, includeClaimed, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Gets detailed information about a specific queued message.
        /// </summary>
        /// <remarks>
        /// This method will return information for the specified message regardless of the
        /// <literal>Client-ID</literal> or claim associated with the message.
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messageId">The message ID. This is obtained from <see cref="QueuedMessage.Id">QueuedMessage.Id</see>.</param>
        /// <returns>A <see cref="QueuedMessage"/> object containing detailed information about the specified message.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageId"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_a_Specific_Message">Get a Specific Message (OpenStack Marconi API v1 Blueprint)</seealso>
        public static QueuedMessage GetMessage(this IQueueingService queueingService, string queueName, string messageId)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.GetMessageAsync(queueName, messageId, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Get messages from a queue.
        /// </summary>
        /// <remarks>
        /// This method will return information for the specified message regardless of the
        /// <literal>Client-ID</literal> or claim associated with the message.
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messageIds">The message IDs of messages to get.</param>
        /// <returns>A collection of <see cref="QueuedMessage"/> objects containing detailed information about the specified messages.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageIds"/> contains a <c>null</c> or empty value.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Get_a_Set_of_Messages_by_ID">Get a Set of Messages by ID (OpenStack Marconi API v1 Blueprint)</seealso>
        public static IEnumerable<QueuedMessage> GetMessages(this IQueueingService queueingService, string queueName, IEnumerable<string> messageIds)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.GetMessagesAsync(queueName, messageIds, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Posts messages to a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messages">The messages to post.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> contains a <c>null</c> value.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void PostMessages(this IQueueingService queueingService, string queueName, IEnumerable<Message> messages)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.PostMessagesAsync(queueName, messages, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Posts messages to a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messages">The messages to post.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> contains a <c>null</c> value.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void PostMessages(this IQueueingService queueingService, string queueName, params Message[] messages)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.PostMessagesAsync(queueName, CancellationToken.None, messages).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Posts messages to a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messages">The messages to post.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> contains a <c>null</c> value.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void PostMessages<T>(this IQueueingService queueingService, string queueName, IEnumerable<Message<T>> messages)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.PostMessagesAsync(queueName, messages, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Posts messages to a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messages">The messages to post.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messages"/> contains a <c>null</c> value.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Post_Message.28s.29">Post Message(s) (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void PostMessages<T>(this IQueueingService queueingService, string queueName, params Message<T>[] messages)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.PostMessagesAsync(queueName, CancellationToken.None, messages).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Deletes a message from a queue.
        /// </summary>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messageId">The ID of the message to delete. This is obtained from <see cref="QueuedMessage.Id">QueuedMessage.Id</see>.</param>
        /// <param name="claim">The claim for the message. If this value is <c>null</c>, the delete operation will fail if the message is claimed. If this value is non-<c>null</c>, the delete operation will fail if the message is not claimed by the specified claim.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageId"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageId"/> is empty.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Delete_Message">Delete Message (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void DeleteMessage(this IQueueingService queueingService, string queueName, string messageId, Claim claim)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.DeleteMessageAsync(queueName, messageId, claim, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Deletes messages from a queue.
        /// </summary>
        /// <remarks>
        /// <note type="warning">
        /// This method deletes messages from a queue whether or not they are currently claimed.
        /// </note>
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="messageIds">The IDs of messages to delete. These are obtained from <see cref="QueuedMessage.Id">QueuedMessage.Id</see>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageIds"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="queueName"/> is empty.
        /// <para>-or-</para>
        /// <para>If <paramref name="messageIds"/> contains a <c>null</c> or empty value.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Delete_a_Set_of_Messages_by_ID">Delete a Set of Messages by ID (OpenStack Marconi API v1 Blueprint)</seealso>
        public static void DeleteMessages(this IQueueingService queueingService, string queueName, IEnumerable<string> messageIds)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.DeleteMessagesAsync(queueName, messageIds, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion Messages

        #region Claims

        /// <summary>
        /// Claim messages from a queue.
        /// </summary>
        /// <remarks>
        /// When the claim is no longer required, the code should call <see cref="Claim.DisposeAsync"/>
        /// or <see cref="Claim.Dispose()"/> to ensure the following actions are taken.
        /// <list type="bullet">
        /// <item>Messages which are part of this claim which were not processed are made available to other nodes.</item>
        /// <item>The claim resource is cleaned up without waiting for the time-to-live to expire.</item>
        /// </list>
        ///
        /// <para>Messages which are not deleted before the claim is released will be eligible for
        /// reclaiming by another process.</para>
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="limit">The maximum number of messages to claim. If this value is <c>null</c>, a provider-specific default value is used.</param>
        /// <param name="timeToLive">The time to wait before the server automatically releases the claim.</param>
        /// <param name="gracePeriod">The time to wait, after the time-to-live for the claim expires, before the server allows the claimed messages to be deleted due to the individual message's time-to-live expiring.</param>
        /// <returns>A <see cref="Claim"/> object representing the claim.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="queueName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="limit"/> is less than or equal to 0.
        /// <para>-or-</para>
        /// <para>If <paramref name="timeToLive"/> is negative or <see cref="TimeSpan.Zero"/>.</para>
        /// <para>-or-</para>
        /// <para>If <paramref name="gracePeriod"/> is negative.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Claim_Messages">Claim Messages (OpenStack Marconi API v1 Blueprint)</seealso>
        public static Claim ClaimMessage(this IQueueingService queueingService, string queueName, int? limit, TimeSpan timeToLive, TimeSpan gracePeriod)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.ClaimMessageAsync(queueName, limit, timeToLive, gracePeriod, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Gets detailed information about the current state of a claim.
        /// </summary>
        /// <remarks>
        /// <note type="caller">Use <see cref="Claim.RefreshAsync"/> instead of calling this method directly.</note>
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="claim">The claim to query.</param>
        /// <returns>A <see cref="Claim"/> object representing the claim.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="claim"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Query_Claim">Query Claim (OpenStack Marconi API v1 Blueprint)</seealso>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Claim QueryClaim(this IQueueingService queueingService, string queueName, Claim claim)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                return queueingService.QueryClaimAsync(queueName, claim, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Renews a claim, by updating the time-to-live and resetting the age of the claim to zero.
        /// </summary>
        /// <remarks>
        /// <note type="caller">Use <see cref="Claim.RenewAsync"/> instead of calling this method directly.</note>
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="claim">The claim to renew.</param>
        /// <param name="timeToLive">The updated time-to-live for the claim.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="claim"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="timeToLive"/> is negative.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Update_Claim">Update Claim (OpenStack Marconi API v1 Blueprint)</seealso>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UpdateClaim(this IQueueingService queueingService, string queueName, Claim claim, TimeSpan timeToLive)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.UpdateClaimAsync(queueName, claim, timeToLive, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        /// <summary>
        /// Immediately release a claim, making any (remaining, non-deleted) messages associated
        /// with the claim available to other workers.
        /// </summary>
        /// <remarks>
        /// <note type="caller">Use <see cref="Claim.DisposeAsync"/> instead of calling this method directly.</note>
        /// </remarks>
        /// <param name="queueingService">The queueing service instance.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="claim">The claim to release.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="queueingService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="queueName"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="claim"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="queueName"/> is empty.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="https://wiki.openstack.org/w/index.php?title=Marconi/specs/api/v1#Release_Claim">Release Claim (OpenStack Marconi API v1 Blueprint)</seealso>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ReleaseClaim(this IQueueingService queueingService, string queueName, Claim claim)
        {
            if (queueingService == null)
                throw new ArgumentNullException("queueingService");

            try
            {
                queueingService.ReleaseClaimAsync(queueName, claim, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion
    }
}
