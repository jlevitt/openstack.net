namespace Net.OpenStack.Testing.Integration.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using net.openstack.Providers.Rackspace;
    using net.openstack.Providers.Rackspace.Objects;
    using CancellationTokenSource = System.Threading.CancellationTokenSource;
    using Path = System.IO.Path;
    using CancellationToken = System.Threading.CancellationToken;
    using Newtonsoft.Json;
    using System.Diagnostics;

    [TestClass]
    public class UserLoadBalancerTests
    {
        /// <summary>
        /// The prefix to use for names of load balancers created during integration testing.
        /// </summary>
        public static readonly string TestLoadBalancerPrefix = "UnitTestLB-";

        /// <summary>
        /// This method can be used to clean up load balancers created during integration testing.
        /// </summary>
        /// <remarks>
        /// The Cloud Load Balancer integration tests generally delete load balancers created
        /// during the tests, but test failures may lead to unused load balancers gathering
        /// on the system. This method searches for all load balancers matching the
        /// "integration testing" pattern (i.e., load balancers whose name starts with
        /// <see cref="TestLoadBalancerPrefix"/>), and attempts to delete them.
        /// <para>
        /// The deletion requests are sent in parallel, so a single deletion failure will not
        /// prevent the method from cleaning up other load balancers that can be successfully
        /// deleted. Note that the system does not increase the
        /// <see cref="ProviderBase{TProvider}.ConnectionLimit"/>, so the underlying REST
        /// requests may be pipelined if the number of load balancers to delete exceeds the
        /// default system connection limit.
        /// </para>
        /// </remarks>
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        [TestCategory(TestCategories.Cleanup)]
        public async Task CleanupTestLoadBalancers()
        {
            ILoadBalancerService provider = CreateProvider();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(60)));
            string queueName = CreateRandomLoadBalancerName();

            LoadBalancer[] allLoadBalancers = ListAllLoadBalancers(provider, null, cancellationTokenSource.Token).Where(loadBalancer => loadBalancer.Name.StartsWith(TestLoadBalancerPrefix, StringComparison.OrdinalIgnoreCase)).ToArray();
            int blockSize = 10;
            for (int i = 0; i < allLoadBalancers.Length; i += blockSize)
            {
                await provider.RemoveLoadBalancerRangeAsync(
                    allLoadBalancers
                        .Skip(i)
                        .Take(blockSize)
                        .Select(loadBalancer =>
                            {
                                Console.WriteLine("Deleting load balancer: {0}", loadBalancer.Name);
                                return loadBalancer.Id;
                            })
                        .ToArray(), // included to ensure the Console.WriteLine is only executed once per load balancer
                    DnsCompletionOption.RequestCompleted,
                    cancellationTokenSource.Token,
                    null);
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListLoadBalancers()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
            {
                LoadBalancer[] loadBalancers = ListAllLoadBalancers(provider, null, cancellationTokenSource.Token).ToArray();
                if (!loadBalancers.Any())
                    Assert.Inconclusive("The account does not appear to contain any load balancers");

                foreach (LoadBalancer loadBalancer in loadBalancers)
                    Console.WriteLine("{0}: {1}", loadBalancer.Id, loadBalancer.Name);
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestGetLoadBalancer()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
            {
                foreach (LoadBalancer loadBalancer in ListAllLoadBalancers(provider, null, cancellationTokenSource.Token))
                {
                    Console.WriteLine("Basic information:");
                    Console.WriteLine(JsonConvert.SerializeObject(loadBalancer, Formatting.Indented));

                    LoadBalancer details = await provider.GetLoadBalancerAsync(loadBalancer.Id, cancellationTokenSource.Token);
                    Console.WriteLine("Detailed information:");
                    Console.WriteLine(JsonConvert.SerializeObject(details, Formatting.Indented));
                }
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestCreateLoadBalancer()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
            {
                string loadBalancerName = CreateRandomLoadBalancerName();

                LoadBalancerConfiguration configuration = new LoadBalancerConfiguration(
                    name: loadBalancerName,
                    nodes: null,
                    protocol: null,
                    virtualAddresses: null,
                    halfClosed: null,
                    accessList: null,
                    algorithm: null,
                    connectionLogging: null,
                    contentCaching: null,
                    connectionThrottle: null,
                    healthMonitor: null,
                    metadata: null,
                    port: null,
                    timeout: null,
                    sessionPersistence: null);
                LoadBalancer tempLoadBalancer = await provider.CreateLoadBalancerAsync(configuration, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);

                foreach (LoadBalancer loadBalancer in ListAllLoadBalancers(provider, null, cancellationTokenSource.Token))
                {
                    Console.WriteLine("{0}: {1}", loadBalancer.Id, loadBalancer.Name);
                }

                await provider.RemoveLoadBalancerAsync(tempLoadBalancer.Id, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestUpdateLoadBalancer()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveLoadBalancer()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestRemoveLoadBalancerRange()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(30))))
            {
                IEnumerable<LoadBalancingProtocol> protocols = await provider.ListProtocolsAsync(cancellationTokenSource.Token);
                LoadBalancingProtocol httpProtocol = protocols.First(i => i.Name.Equals("HTTP", StringComparison.OrdinalIgnoreCase));

                string loadBalancerName = CreateRandomLoadBalancerName();

                LoadBalancerConfiguration configuration = new LoadBalancerConfiguration(
                    name: loadBalancerName,
                    nodes: null,
                    protocol: httpProtocol,
                    virtualAddresses: new[] { new LoadBalancerVirtualAddress(LoadBalancerVirtualAddressType.ServiceNet) },
                    halfClosed: null,
                    accessList: null,
                    algorithm: LoadBalancingAlgorithm.RoundRobin,
                    connectionLogging: null,
                    contentCaching: null,
                    connectionThrottle: null,
                    healthMonitor: null,
                    metadata: null,
                    port: null,
                    timeout: null,
                    sessionPersistence: null);
                Task<LoadBalancer> tempLoadBalancer = provider.CreateLoadBalancerAsync(configuration, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);

                string loadBalancer2Name = CreateRandomLoadBalancerName();

                LoadBalancerConfiguration configuration2 = new LoadBalancerConfiguration(
                    name: loadBalancer2Name,
                    nodes: null,
                    protocol: httpProtocol,
                    virtualAddresses: new[] { new LoadBalancerVirtualAddress(LoadBalancerVirtualAddressType.ServiceNet) },
                    halfClosed: null,
                    accessList: null,
                    algorithm: LoadBalancingAlgorithm.RoundRobin,
                    connectionLogging: null,
                    contentCaching: null,
                    connectionThrottle: null,
                    healthMonitor: null,
                    metadata: null,
                    port: null,
                    timeout: null,
                    sessionPersistence: null);
                Task<LoadBalancer> tempLoadBalancer2 = provider.CreateLoadBalancerAsync(configuration2, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);

                await Task.Factory.ContinueWhenAll(new Task[] { tempLoadBalancer, tempLoadBalancer2 }, TaskExtrasExtensions.PropagateExceptions);
                Assert.AreEqual(LoadBalancerStatus.Active, tempLoadBalancer.Result.Status);
                Assert.AreEqual(LoadBalancerStatus.Active, tempLoadBalancer2.Result.Status);

                /* Cleanup
                 */

                await provider.RemoveLoadBalancerRangeAsync(new[] { tempLoadBalancer.Result.Id, tempLoadBalancer2.Result.Id }, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestGetErrorPage()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
            {
                LoadBalancer[] loadBalancers = ListAllLoadBalancers(provider, null, cancellationTokenSource.Token).ToArray();
                if (!loadBalancers.Any())
                    Assert.Inconclusive("The account does not appear to contain any load balancers");

                foreach (LoadBalancer loadBalancer in loadBalancers)
                {
                    Console.WriteLine("{0}: {1}", loadBalancer.Id, loadBalancer.Name);
                    Console.WriteLine("Error page:");
                    Console.WriteLine(await provider.GetErrorPageAsync(loadBalancer.Id, cancellationTokenSource.Token));
                }
            }
        }

        /// <summary>
        /// This method performs integration testing for the following methods:
        /// <list type="bullet">
        /// <item><see cref="ILoadBalancerService.GetErrorPageAsync"/></item>
        /// <item><see cref="ILoadBalancerService.SetErrorPageAsync"/></item>
        /// <item><see cref="ILoadBalancerService.RemoveErrorPageAsync"/></item>
        /// </list>
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestModifyErrorPage()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(30))))
            {
                IEnumerable<LoadBalancingProtocol> protocols = await provider.ListProtocolsAsync(cancellationTokenSource.Token);
                LoadBalancingProtocol httpProtocol = protocols.First(i => i.Name.Equals("HTTP", StringComparison.OrdinalIgnoreCase));

                string loadBalancerName = CreateRandomLoadBalancerName();

                LoadBalancerConfiguration configuration = new LoadBalancerConfiguration(
                    name: loadBalancerName,
                    nodes: null,
                    protocol: httpProtocol,
                    virtualAddresses: new[] { new LoadBalancerVirtualAddress(LoadBalancerVirtualAddressType.ServiceNet) },
                    halfClosed: null,
                    accessList: null,
                    algorithm: LoadBalancingAlgorithm.RoundRobin,
                    connectionLogging: null,
                    contentCaching: null,
                    connectionThrottle: null,
                    healthMonitor: null,
                    metadata: null,
                    port: null,
                    timeout: null,
                    sessionPersistence: null);
                LoadBalancer tempLoadBalancer = await provider.CreateLoadBalancerAsync(configuration, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);
                Assert.AreEqual(LoadBalancerStatus.Active, tempLoadBalancer.Status);

                Console.WriteLine("Error page:");
                string defaultErrorPage = await provider.GetErrorPageAsync(tempLoadBalancer.Id, cancellationTokenSource.Token);
                string customErrorPage = "Some custom error...";

                await provider.SetErrorPageAsync(tempLoadBalancer.Id, customErrorPage, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);
                LoadBalancer details = await provider.GetLoadBalancerAsync(tempLoadBalancer.Id, cancellationTokenSource.Token);
                Assert.AreEqual(LoadBalancerStatus.Active, details.Status);
                Assert.AreEqual(customErrorPage, await provider.GetErrorPageAsync(tempLoadBalancer.Id, cancellationTokenSource.Token));

                await provider.RemoveErrorPageAsync(tempLoadBalancer.Id, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);
                details = await provider.GetLoadBalancerAsync(tempLoadBalancer.Id, cancellationTokenSource.Token);
                Assert.AreEqual(LoadBalancerStatus.Active, details.Status);
                Assert.AreEqual(defaultErrorPage, await provider.GetErrorPageAsync(tempLoadBalancer.Id, cancellationTokenSource.Token));

                /* Cleanup
                 */

                await provider.RemoveLoadBalancerAsync(tempLoadBalancer.Id, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token, null);
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestGetStatistics()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
            {
                LoadBalancer[] loadBalancers = ListAllLoadBalancers(provider, null, cancellationTokenSource.Token).ToArray();
                if (!loadBalancers.Any())
                    Assert.Inconclusive("The account does not appear to contain any load balancers");

                foreach (LoadBalancer loadBalancer in loadBalancers)
                {
                    LoadBalancerStatistics statistics = await provider.GetStatisticsAsync(loadBalancer.Id, cancellationTokenSource.Token);
                    Console.WriteLine("{0}: {1}", loadBalancer.Id, loadBalancer.Name);
                    Console.WriteLine("  Predefined Statistics");
                    Console.WriteLine("    Connection Error: {0}", statistics.ConnectionError);
                    Console.WriteLine("    Connection Failure: {0}", statistics.ConnectionFailure);
                    Console.WriteLine("    Connection Timed Out: {0}", statistics.ConnectionTimedOut);
                    Console.WriteLine("    Data Timed Out: {0}", statistics.DataTimedOut);
                    Console.WriteLine("    Keep Alive Timed Out: {0}", statistics.KeepAliveTimedOut);
                    Console.WriteLine("    Max Connections: {0}", statistics.MaxConnections);
                    Console.WriteLine("  Generic Statistics");
                    foreach (var pair in statistics)
                        Console.WriteLine("    {0}: {1}", pair.Key, pair.Value);
                }
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListNodes()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestGetNode()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestAddNode()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestAddNodes()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestUpdateNode()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveNode()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveNodeRange()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListNodeServiceEvents()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListVirtualAddresses()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveVirtualAddress()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveVirtualAddressRange()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestListAllowedDomains()
        {
            ILoadBalancerService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
            {
                Console.WriteLine("Allowed domains:");
                IEnumerable<string> allowedDomains = await provider.ListAllowedDomainsAsync(cancellationTokenSource.Token);
                Assert.IsNotNull(allowedDomains);

                foreach (string domain in allowedDomains)
                {
                    Assert.IsNotNull(domain);
                    Console.WriteLine("    {0}", domain);
                }

                if (!allowedDomains.Any())
                    Assert.Inconclusive("No allowed domains were returned by the call.");
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListBillableLoadBalancers()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListAccountLevelUsage()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListHistoricalUsage()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListCurrentUsage()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListAccessList()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestCreateAccessList()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestCreateAccessListRange()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveAccessList()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveAccessListRange()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestClearAccessList()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestGetSessionPersistence()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestSetSessionPersistence()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveSessionPersistence()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestGetConnectionLogging()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestSetConnectionLogging()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListThrottles()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestUpdateThrottles()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveThrottles()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestGetContentCaching()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestSetContentCaching()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestListProtocols()
        {
            ILoadBalancerService provider = CreateProvider();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(60)));

            IEnumerable<LoadBalancingProtocol> protocols = await provider.ListProtocolsAsync(cancellationTokenSource.Token);
            if (!protocols.Any())
                Assert.Inconclusive("No load balancer protocols were returned by the server.");

            foreach (LoadBalancingProtocol protocol in protocols)
                Console.WriteLine("{0} ({1})", protocol.Name, protocol.Port);
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public async Task TestListAlgorithms()
        {
            ILoadBalancerService provider = CreateProvider();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(60)));

            IEnumerable<LoadBalancingAlgorithm> algorithms = await provider.ListAlgorithmsAsync(cancellationTokenSource.Token);
            if (!algorithms.Any())
                Assert.Inconclusive("No load balancer algorithms were returned by the server.");

            foreach (LoadBalancingAlgorithm algorithm in algorithms)
                Console.WriteLine(algorithm.Name);

            Assert.IsTrue(algorithms.Contains(LoadBalancingAlgorithm.LeastConnections));
            Assert.IsTrue(algorithms.Contains(LoadBalancingAlgorithm.Random));
            Assert.IsTrue(algorithms.Contains(LoadBalancingAlgorithm.RoundRobin));
            Assert.IsTrue(algorithms.Contains(LoadBalancingAlgorithm.WeightedLeastConnections));
            Assert.IsTrue(algorithms.Contains(LoadBalancingAlgorithm.WeightedRoundRobin));
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListLoadBalancerMetadata()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestGetLoadBalancerMetadataItem()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestAddLoadBalancerMetadata()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestUpdateLoadBalancerMetadataItem()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveLoadBalancerMetadataItem()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestListNodeMetadata()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestGetNodeMetadataItem()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestAddNodeMetadata()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestUpdateNodeMetadataItem()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.LoadBalancers)]
        public void TestRemoveNodeMetadataItem()
        {
            Assert.Inconclusive("Not yet implemented.");
        }

        /// <summary>
        /// Gets all existing load balancers through a series of asynchronous operations,
        /// each of which requests a subset of the available load balancers.
        /// </summary>
        /// <remarks>
        /// Each of the returned tasks is executed asynchronously but sequentially. This
        /// method will not send concurrent requests to the load balancers service.
        /// <para>
        /// Due to the way the list end is detected, the final task will return an empty
        /// collection of <see cref="LoadBalancer"/> instances.
        /// </para>
        /// </remarks>
        /// <param name="provider">The load balancer service.</param>
        /// <param name="limit">The maximum number of <see cref="LoadBalancer"/> to return from a single task. If this value is <c>null</c>, a provider-specific default is used.</param>
        /// <returns>
        /// A collections of <see cref="Task{TResult}"/> objects, each of which
        /// represents an asynchronous operation to gather a subset of the available
        /// load balancers.
        /// </returns>
        private static IEnumerable<LoadBalancer> ListAllLoadBalancers(ILoadBalancerService provider, int? limit, CancellationToken cancellationToken)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            LoadBalancer lastLoadBalancer = null;

            do
            {
                string marker = lastLoadBalancer != null ? lastLoadBalancer.Id : null;
                IEnumerable<LoadBalancer> loadBalancers = provider.ListLoadBalancersAsync(marker, limit, cancellationToken).Result;
                lastLoadBalancer = null;
                foreach (LoadBalancer loadBalancer in loadBalancers)
                {
                    yield return loadBalancer;
                    lastLoadBalancer = loadBalancer;
                }
            } while (false && lastLoadBalancer != null);
        }

        private TimeSpan TestTimeout(TimeSpan timeout)
        {
            if (Debugger.IsAttached)
                return TimeSpan.FromDays(1);

            return timeout;
        }

        /// <summary>
        /// Creates a random load balancer name with the proper prefix for integration testing.
        /// </summary>
        /// <returns>A unique, randomly-generated load balancer name.</returns>
        private string CreateRandomLoadBalancerName()
        {
            return TestLoadBalancerPrefix + Path.GetRandomFileName();
        }

        /// <summary>
        /// Creates an instance of <see cref="ILoadBalancerService"/> for testing using
        /// the <see cref="OpenstackNetSetings.TestIdentity"/>.
        /// </summary>
        /// <returns>An instance of <see cref="ILoadBalancerService"/> for integration testing.</returns>
        private ILoadBalancerService CreateProvider()
        {
            var provider = new CloudLoadBalancerProvider(Bootstrapper.Settings.TestIdentity, null, null, null);
            provider.BeforeAsyncWebRequest +=
                (sender, e) =>
                {
                    Console.WriteLine("{0} (Request) {1} {2}", DateTime.Now, e.Request.Method, e.Request.RequestUri);
                };
            provider.AfterAsyncWebResponse +=
                (sender, e) =>
                {
                    Console.WriteLine("{0} (Result {1}) {2}", DateTime.Now, e.Response.StatusCode, e.Response.ResponseUri);
                };

            provider.ConnectionLimit = 3;

            return provider;
        }
    }
}
