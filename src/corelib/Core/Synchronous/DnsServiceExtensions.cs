﻿namespace net.openstack.Core.Synchronous
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using net.openstack.Providers.Rackspace;
    using net.openstack.Providers.Rackspace.Objects;
    using CancellationToken = System.Threading.CancellationToken;

    /// <summary>
    /// Provides extension methods to allow synchronous calls to the methods in <see cref="IDnsService"/>.
    /// </summary>
    /// <preliminary/>
    public static class DnsServiceExtensions
    {
        #region Limits

        /// <summary>
        /// Get information about the provider-specific limits of this service.
        /// </summary>
        /// <param name="service">The DNS service instance.</param>
        /// <returns>A <see cref="DnsServiceLimits"/> object containing detailed information about the limits for the service provider.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="service"/> is <c>null</c>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_All_Limits.html">List All Limits (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        public static DnsServiceLimits ListLimits(this IDnsService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            try
            {
                return service.ListLimitsAsync(CancellationToken.None).Result;
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
        /// Get information about the types of provider-specific limits in place for this service.
        /// </summary>
        /// <param name="service">The DNS service instance.</param>
        /// <returns>A collection of <see cref="LimitType"/> objects containing the limit types supported by the service.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="service"/> is <c>null</c>.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Limit_Types.html">List Limit Types (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        public static IEnumerable<LimitType> ListLimitTypes(this IDnsService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            try
            {
                return service.ListLimitTypesAsync(CancellationToken.None).Result;
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
        /// Get information about the provider-specific limits of this service for a particular <see cref="LimitType"/>.
        /// </summary>
        /// <param name="service">The DNS service instance.</param>
        /// <param name="type">The limit type.</param>
        /// <returns>A <see cref="DnsServiceLimits"/> object containing detailed information about the limits of the specified <paramref name="type"/> for the service provider.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="service"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="type"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/List_Specific_Limit.html">List Specific Limit (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        public static DnsServiceLimits ListLimits(this IDnsService service, LimitType type)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            try
            {
                return service.ListLimitsAsync(type, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion Limits

        #region Jobs

        /// <summary>
        /// Gets information about an asynchronous task being executed by the DNS service.
        /// </summary>
        /// <param name="service">The DNS service instance.</param>
        /// <param name="job">The <see cref="DnsJob"/> to query.</param>
        /// <param name="showDetails"><c>true</c> to include detailed information about the job; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="DnsJob"/> object containing the updated job information.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="service"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="job"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/sync_asynch_responses.html">Synchronous and Asynchronous Responses (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        public static DnsJob GetJobStatus(this IDnsService service, DnsJob job, bool showDetails)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            try
            {
                return service.GetJobStatusAsync(job, showDetails, CancellationToken.None).Result;
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
        /// Gets information about an asynchronous task with a strongly-typed result being executed by the DNS service.
        /// </summary>
        /// <param name="service">The DNS service instance.</param>
        /// <param name="job">The <see cref="DnsJob{TResponse}"/> to query.</param>
        /// <param name="showDetails"><c>true</c> to include detailed information about the job; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="DnsJob{TResult}"/> object containing the updated job information.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="service"/> is <c>null</c>.
        /// <para>-or-</para>
        /// <para>If <paramref name="job"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="JsonSerializationException">If an error occurs while deserializing the response object.</exception>
        /// <exception cref="WebException">If the REST request does not return successfully.</exception>
        /// <seealso href="http://docs.rackspace.com/cdns/api/v1.0/cdns-devguide/content/sync_asynch_responses.html">Synchronous and Asynchronous Responses (Rackspace Cloud DNS Developer Guide - API v1.0)</seealso>
        public static DnsJob<TResponse> GetJobStatus<TResponse>(this IDnsService service, DnsJob<TResponse> job, bool showDetails)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            try
            {
                return service.GetJobStatusAsync(job, showDetails, CancellationToken.None).Result;
            }
            catch (AggregateException ex)
            {
                ReadOnlyCollection<Exception> innerExceptions = ex.Flatten().InnerExceptions;
                if (innerExceptions.Count == 1)
                    throw innerExceptions[0];

                throw;
            }
        }

        #endregion Jobs
    }
}
