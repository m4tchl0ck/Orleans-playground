using Cqrs;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class GrainComman2Tests : BaseTest
{
    [Fact]
    public async Task ShouldPass_WhenCalled()
    {
        // Arrange
        var cmd = new GrainCommand2("Name", 42);
        var bus = new GrainBus(ClusterClient);

        // Act
        await bus.Send(cmd);

        // Assert
        var somethingMock = ServiceProvider.GetRequiredService<Mock<ISomething>>();
        somethingMock.Verify(x => x.Do(cmd.Name, cmd.Age), Times.Once);
    }
}
