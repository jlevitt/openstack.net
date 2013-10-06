namespace Net.OpenStack.Testing.Integration.Providers.Rackspace
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using net.openstack.Providers.Rackspace;
    using net.openstack.Providers.Rackspace.Objects;
    using Newtonsoft.Json;
    using Path = System.IO.Path;

    [TestClass]
    public class UserDnsTests
    {
        /// <summary>
        /// Users created by these unit tests have a username with this prefix, allowing
        /// them to be identified and cleaned up following a failed test.
        /// </summary>
        private static readonly string TestDomainPrefix = "UnitTestDomain-";

        /// <summary>
        /// This method can be used to clean up domains created during integration testing.
        /// </summary>
        /// <remarks>
        /// The Cloud DNS integration tests generally delete domains created during the
        /// tests, but test failures may lead to unused domains gathering on the system.
        /// This method searches for all domains matching the "integration testing"
        /// pattern (i.e., domains whose name starts with <see cref="TestDomainPrefix"/>),
        /// and attempts to delete them.
        /// <para>
        /// The deletion requests are sent in parallel, so a single deletion failure will
        /// not prevent the method from cleaning up other queues that can be successfully
        /// deleted. Note that the system does not increase the
        /// <see cref="ProviderBase{TProvider}.ConnectionLimit"/>, so the underlying REST
        /// requests may be pipelined if the number of domains to delete exceeds the
        /// default system connection limit.
        /// </para>
        /// </remarks>
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        [TestCategory(TestCategories.Cleanup)]
        public void CleanupTestDomains()
        {
            const int BatchSize = 10;

            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(60))))
            {
                DnsDomain[] allDomains = ListAllDomains(provider, null, null, cancellationTokenSource.Token).Where(i => i.Name.StartsWith(TestDomainPrefix, StringComparison.OrdinalIgnoreCase)).ToArray();

                List<Task> deleteTasks = new List<Task>();
                for (int i = 0; i < allDomains.Length; i += BatchSize)
                {
                    for (int j = i; j < i + BatchSize && j < allDomains.Length; j++)
                        Console.WriteLine("Deleting domain: {0}", allDomains[j].Name);

                    deleteTasks.Add(provider.RemoveDomainsAsync(allDomains.Skip(i).Take(BatchSize).Select(domain => domain.Id), true, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token));
                }

                Task.WaitAll(deleteTasks.ToArray());
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestListLimits()
        {
            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
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
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
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
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(10))))
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

        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestListDomains()
        {
            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(30))))
            {
                IEnumerable<DnsDomain> domains = ListAllDomains(provider, null, null, cancellationTokenSource.Token);
                Assert.IsNotNull(domains);

                if (!domains.Any())
                    Assert.Inconclusive("No domains were returned by the server");

                foreach (var domain in domains)
                {
                    Console.WriteLine();
                    Console.WriteLine("Domain: {0} ({1})", domain.Name, domain.Id);
                    Console.WriteLine();
                    Console.WriteLine(await JsonConvert.SerializeObjectAsync(domain, Formatting.Indented));
                }
            }
        }

        /// <summary>
        /// This tests the behavior of the <see cref="IDnsService.CreateDomainsAsync"/> and
        /// <see cref="IDnsService.RemoveDomainsAsync"/> when performed on a single domain.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestCreateDomain()
        {
            string domainName = CreateRandomDomainName();

            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(30))))
            {
                DnsConfiguration configuration = new DnsConfiguration(
                    new DnsDomainConfiguration(
                        name: domainName,
                        timeToLive: default(TimeSpan?),
                        emailAddress: "admin@" + domainName,
                        comment: "Integration test domain",
                        records: new DnsDomainRecordConfiguration[] { },
                        subdomains: new DnsSubdomainConfiguration[] { }));

                DnsJob<DnsDomains> createResponse = await provider.CreateDomainsAsync(configuration, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob<DnsDomains> details = await provider.GetJobStatus<DnsDomains>(createResponse.Id, true, cancellationTokenSource.Token);
                IEnumerable<DnsDomain> createdDomains = Enumerable.Empty<DnsDomain>();
                if (createResponse.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(details.Error.ToString(Formatting.Indented));
                    Assert.Fail();
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(details.Response, Formatting.Indented));
                    createdDomains = details.Response.Domains;
                }

                IEnumerable<DnsDomain> domains = await provider.ListDomainsAsync(domainName, null, null, cancellationTokenSource.Token);
                Assert.IsNotNull(domains);

                if (!domains.Any())
                    Assert.Inconclusive("No domains were returned by the server");

                foreach (var domain in domains)
                {
                    Console.WriteLine();
                    Console.WriteLine("Domain: {0} ({1})", domain.Name, domain.Id);
                    Console.WriteLine();
                    Console.WriteLine(await JsonConvert.SerializeObjectAsync(domain, Formatting.Indented));
                }

                DnsJob deleteResponse = await provider.RemoveDomainsAsync(createdDomains.Select(i => i.Id), false, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob deleteDetails = await provider.GetJobStatus(deleteResponse.Id, true, cancellationTokenSource.Token);
                if (deleteDetails.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(deleteDetails.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to delete temporary domain created during the integration test.");
                }
            }
        }

        /// <summary>
        /// This tests the behavior of the <see cref="IDnsService.CloneDomainsAsync"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestCloneDomain()
        {
            string domainName = CreateRandomDomainName();
            string clonedName = CreateRandomDomainName();

            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(30))))
            {
                List<string> domainsToRemove = new List<string>();

                DnsConfiguration configuration = new DnsConfiguration(
                    new DnsDomainConfiguration(
                        name: domainName,
                        timeToLive: default(TimeSpan?),
                        emailAddress: "admin@" + domainName,
                        comment: "Integration test domain",
                        records: new DnsDomainRecordConfiguration[] { },
                        subdomains: new DnsSubdomainConfiguration[] { }));

                DnsJob<DnsDomains> createResponse = await provider.CreateDomainsAsync(configuration, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob<DnsDomains> details = await provider.GetJobStatus<DnsDomains>(createResponse.Id, true, cancellationTokenSource.Token);
                if (createResponse.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(details.Error.ToString(Formatting.Indented));
                    Assert.Fail();
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(details.Response, Formatting.Indented));
                    domainsToRemove.AddRange(details.Response.Domains.Select(i => i.Id));
                }

                DnsJob<DnsDomains> cloneResponse = await provider.CloneDomainAsync(details.Response.Domains[0].Id, clonedName, true, true, true, true, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob<DnsDomains> cloneDetails = await provider.GetJobStatus<DnsDomains>(cloneResponse.Id, true, cancellationTokenSource.Token);
                if (cloneResponse.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(cloneDetails.Error.ToString(Formatting.Indented));
                    Assert.Fail();
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(cloneDetails.Response, Formatting.Indented));
                    domainsToRemove.AddRange(cloneDetails.Response.Domains.Select(i => i.Id));
                }

                DnsJob deleteResponse = await provider.RemoveDomainsAsync(domainsToRemove, false, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob deleteDetails = await provider.GetJobStatus(deleteResponse.Id, true, cancellationTokenSource.Token);
                if (deleteDetails.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(deleteDetails.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to delete temporary domain created during the integration test.");
                }
            }
        }

        /// <summary>
        /// This tests the behavior of the <see cref="IDnsService.CloneDomainsAsync"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        [TestMethod]
        [TestCategory(TestCategories.User)]
        [TestCategory(TestCategories.Dns)]
        public async Task TestDomainExportImport()
        {
            string domainName = CreateRandomDomainName();

            IDnsService provider = CreateProvider();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TestTimeout(TimeSpan.FromSeconds(60))))
            {
                List<string> domainsToRemove = new List<string>();

                DnsConfiguration configuration = new DnsConfiguration(
                    new DnsDomainConfiguration(
                        name: domainName,
                        timeToLive: default(TimeSpan?),
                        emailAddress: "admin@" + domainName,
                        comment: "Integration test domain",
                        records: new DnsDomainRecordConfiguration[] { },
                        subdomains: new DnsSubdomainConfiguration[] { }));

                DnsJob<DnsDomains> createResponse = await provider.CreateDomainsAsync(configuration, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob<DnsDomains> details = await provider.GetJobStatus<DnsDomains>(createResponse.Id, true, cancellationTokenSource.Token);
                if (createResponse.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(details.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to create a test domain.");
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(details.Response, Formatting.Indented));
                    domainsToRemove.AddRange(details.Response.Domains.Select(i => i.Id));
                }

                DnsJob<ExportedDomain> exportedDomain = await provider.ExportDomainAsync(details.Response.Domains[0].Id, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                exportedDomain = await provider.GetJobStatus<ExportedDomain>(exportedDomain.Id, true, cancellationTokenSource.Token);
                if (exportedDomain.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(exportedDomain.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to export test domain.");
                }

                Assert.AreEqual(DnsJobStatus.Completed, exportedDomain.Status);
                Assert.IsNotNull(exportedDomain.Response);
                Assert.IsFalse(string.IsNullOrEmpty(exportedDomain.Response.Id));
                Assert.IsFalse(string.IsNullOrEmpty(exportedDomain.Response.AccountId));
                Assert.IsFalse(string.IsNullOrEmpty(exportedDomain.Response.Contents));
                Assert.IsFalse(string.IsNullOrEmpty(exportedDomain.Response.ContentType));

                DnsJob deleteResponse = await provider.RemoveDomainsAsync(domainsToRemove, false, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                DnsJob deleteDetails = await provider.GetJobStatus(deleteResponse.Id, true, cancellationTokenSource.Token);
                if (deleteDetails.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(deleteDetails.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to delete temporary domain created during the integration test.");
                }

                domainsToRemove.Clear();

                SerializedDomain serializedDomain =
                    new SerializedDomain(
                        exportedDomain.Response.Contents,
                        exportedDomain.Response.ContentType);
                DnsJob<DnsDomains> importedDomain = await provider.ImportDomainAsync(new[] { serializedDomain }, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                importedDomain = await provider.GetJobStatus<DnsDomains>(importedDomain.Id, true, cancellationTokenSource.Token);
                if (importedDomain.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(importedDomain.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to import the test domain.");
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(importedDomain.Response, Formatting.Indented));
                    domainsToRemove.AddRange(importedDomain.Response.Domains.Select(i => i.Id));
                }

                deleteResponse = await provider.RemoveDomainsAsync(domainsToRemove, false, DnsCompletionOption.RequestCompleted, cancellationTokenSource.Token);
                deleteDetails = await provider.GetJobStatus(deleteResponse.Id, true, cancellationTokenSource.Token);
                if (deleteDetails.Status == DnsJobStatus.Error)
                {
                    Console.WriteLine(deleteDetails.Error.ToString(Formatting.Indented));
                    Assert.Fail("Failed to delete temporary domain created during the integration test.");
                }
            }
        }

        /// <summary>
        /// Gets all existing domains through a series of asynchronous operations,
        /// each of which requests a subset of the available domains.
        /// </summary>
        /// <remarks>
        /// Each of the returned tasks is executed asynchronously but sequentially. This
        /// method will not send concurrent requests to the DNS service.
        /// <para>
        /// Due to the way the list end is detected, the final task will return an empty
        /// collection of <see cref="DnsDomain"/> instances.
        /// </para>
        /// </remarks>
        /// <param name="provider">The DNS service.</param>
        /// <param name="limit">The maximum number of <see cref="DnsDomain"/> objects to return from a single task. If this value is <c>null</c>, a provider-specific default is used.</param>
        /// <param name="detailed"><c>true</c> to return detailed information for each domain; otherwise, <c>false</c>.</param>
        /// <returns>
        /// A collections of <see cref="Task{TResult}"/> objects, each of which
        /// represents an asynchronous operation to gather a subset of the available
        /// domains.
        /// </returns>
        private static IEnumerable<DnsDomain> ListAllDomains(IDnsService provider, string domainName, int? limit, CancellationToken cancellationToken)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            int index = 0;
            int previousIndex;

            do
            {
                previousIndex = index;
                Task<IEnumerable<DnsDomain>> domains = provider.ListDomainsAsync(domainName, index, limit, cancellationToken);
                foreach (DnsDomain domain in domains.Result)
                {
                    index++;
                    yield return domain;
                }

                if (limit == null)
                {
                    // this service will return a 400 error if offset is not a multiple of limit,
                    // or if the limit is not specified
                    limit = index;
                }
            } while (index > previousIndex);
        }

        private TimeSpan TestTimeout(TimeSpan timeout)
        {
            if (Debugger.IsAttached)
                return TimeSpan.FromDays(1);

            return timeout;
        }

        private string CreateRandomDomainName()
        {
            return TestDomainPrefix + Path.GetRandomFileName().Replace('.', '-') + ".com";
        }

        private IDnsService CreateProvider()
        {
            return new CloudDnsProvider(Bootstrapper.Settings.TestIdentity, null, false, null, null);
        }
    }
}
