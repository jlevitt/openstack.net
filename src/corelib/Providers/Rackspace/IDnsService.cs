namespace net.openstack.Providers.Rackspace
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using net.openstack.Providers.Rackspace.Objects;

    public interface IDnsService
    {
        #region Limits

        Task<DnsServiceLimits> ListLimitsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<LimitType>> ListLimitTypesAsync(CancellationToken cancellationToken);

        Task<DnsServiceLimits> ListLimitsAsync(LimitType type, CancellationToken cancellationToken);

        #endregion
    }
}
