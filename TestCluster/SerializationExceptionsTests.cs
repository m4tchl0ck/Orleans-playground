using FluentAssertions;
using Orleans.TestingHost;

public class SerializationExceptionsTests : IAsyncLifetime
{
    TestCluster TestCluster { get; }

    public SerializationExceptionsTests()
    {
        var builder = new TestClusterBuilder(1);
        TestCluster = builder.Build();
    }

    [Fact]
    public async Task ShouldThrowValidExceptionTypeTest()
    {
        // Arrange
        var grain = TestCluster.GrainFactory.GetGrain<IThrowingGrain>(Guid.NewGuid().ToString());

        // Act
        var act = async () => await grain.ThrowCustomException();

        // Assert
        await act.Should().ThrowAsync<CustomException>();
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