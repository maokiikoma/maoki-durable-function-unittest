using MaokiDurableFunction;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class MyOrchestratorTest
    {
        private Mock<IDurableOrchestrationContext> _durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

        [Fact]
        public async Task Test1()
        {
            // arrange
            _durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<string>("Function1_Hello", "Tokyo")).ReturnsAsync("Hello Tokyo!");
            _durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<string>("Function1_Hello", "Seattle")).ReturnsAsync("Hello Seattle!");
            _durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<string>("Function1_Hello", "London")).ReturnsAsync("Hello London!");

            // act
            var result = await Function1.RunOrchestrator(_durableOrchestrationContextMock.Object);

            // assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Hello Tokyo!", result[0]);
            Assert.Equal("Hello Seattle!", result[1]);
            Assert.Equal("Hello London!", result[2]);
        }
    }
}
