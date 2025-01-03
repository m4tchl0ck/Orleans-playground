using Cqrs;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Orleans.TestingHost;

public class CommandExecutionTests : IAsyncLifetime
{
    TestCluster TestCluster { get; }
    Lazy<IServiceProvider> _serviceProviderLazy;
    IServiceProvider ServiceProvider => _serviceProviderLazy.Value;

    public CommandExecutionTests()
    {
        var builder = new TestClusterBuilder(1);
        TestCluster = builder
            .AddSiloBuilderConfigurator<TestHostConfigurator>()
            .Build();

        _serviceProviderLazy = new(() => 
            TestCluster.Silos.Cast<InProcessSiloHandle>().Single().SiloHost.Services);
    }

    [Fact]
    public async Task SimpleBus_Should_Pass()
    {
        // Arrange
        var cmd = new SimpleCommand1("Name", 42);
        var bus = new SimpleBus(ServiceProvider);
        // Act

        await bus.Send(cmd);
        // Assert
        var somethingMock = ServiceProvider.GetRequiredService<Mock<ISomething>>();
        somethingMock.Verify(x => x.Do(cmd.Name, cmd.Age), Times.Once);
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
