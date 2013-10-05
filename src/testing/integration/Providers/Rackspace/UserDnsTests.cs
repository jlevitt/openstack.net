namespace Net.OpenStack.Testing.Integration.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using net.openstack.Providers.Rackspace;
    using net.openstack.Providers.Rackspace.Objects;
    using Newtonsoft.Json;

    [TestClass]
    public class UserDnsTests
    {
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestListLimits()
        {
            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
            {
                DnsServiceLimits limits = await provider.ListLimitsAsync(cancellationTokenSource.Token);
                Assert.IsNotNull(limits);
                Assert.IsNotNull(limits.RateLimits);
                Assert.IsNotNull(limits.AbsoluteLimits);

                Console.WriteLine(await JsonConvert.SerializeObjectAsync(limits, Formatting.Indented));
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestListLimitTypes()
        {
            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
            {
                IEnumerable<LimitType> limitTypes = await provider.ListLimitTypesAsync(cancellationTokenSource.Token);
                Assert.IsNotNull(limitTypes);

                if (!limitTypes.Any())
                    Assert.Inconclusive("No limit types were returned by the server");

                foreach (var limitType in limitTypes)
                    Console.WriteLine(limitType.Name);

                Assert.IsTrue(limitTypes.Contains(LimitType.Rate));
                Assert.IsTrue(limitTypes.Contains(LimitType.Domain));
                Assert.IsTrue(limitTypes.Contains(LimitType.DomainRecord));
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestListSpecificLimit()
        {
            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
            {
                IEnumerable<LimitType> limitTypes = await provider.ListLimitTypesAsync(cancellationTokenSource.Token);
                Assert.IsNotNull(limitTypes);

                if (!limitTypes.Any())
                    Assert.Inconclusive("No limit types were returned by the server");

                foreach (var limitType in limitTypes)
                {
                    Console.WriteLine();
                    Console.WriteLine("Limit Type: {0}", limitType);
                    Console.WriteLine();
                    DnsServiceLimits limits = await provider.ListLimitsAsync(limitType, cancellationTokenSource.Token);
                    Console.WriteLine(await JsonConvert.SerializeObjectAsync(limits, Formatting.Indented));
                }
            }
        }

        private IDnsService CreateProvider()
        {
            return new CloudDnsProvider(Bootstrapper.Settings.TestIdentity, null, false, null, null);
        }
    }
}
