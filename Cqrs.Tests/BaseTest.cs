using Orleans.TestingHost;

public abstract class BaseTest : IAsyncLifetime
{
    TestCluster TestCluster { get; }
    Lazy<IServiceProvider> _serviceProviderLazy;
    protected IServiceProvider ServiceProvider => _serviceProviderLazy.Value;
    protected IClusterClient ClusterClient => TestCluster.Client;

    public BaseTest()
    {
        var builder = new TestClusterBuilder(1);
        TestCluster = builder
            .AddSiloBuilderConfigurator<TestHostConfigurator>()
            .Build();

        _serviceProviderLazy = new(() => 
            TestCluster.Silos.Cast<InProcessSiloHandle>().Single().SiloHost.Services);
    }

    public async Task InitializeAsync()
    {
        await TestCluster.DeployAsync();
    }

    public async Task DisposeAsync()
    {
        await TestCluster.StopAllSilosAsync();
        await TestCluster.DisposeAsync();
    }
}
