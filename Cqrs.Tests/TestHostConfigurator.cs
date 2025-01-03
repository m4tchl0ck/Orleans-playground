using Cqrs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Orleans.TestingHost;

public class TestHostConfigurator : IHostConfigurator
{
    public void Configure(IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.InitializeCqrs();

            var somentingMock = new Mock<ISomething>();
            services.AddSingleton(somentingMock);
            services.AddSingleton(sp => sp.GetRequiredService<Mock<ISomething>>().Object);
        });
    }
}