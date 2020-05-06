using MaokiDurableFunction;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class MyHttpStarterTest
    {
        private Mock<IDurableOrchestrationClient> _durableOrchestrationClientMock = new Mock<IDurableOrchestrationClient>();
        private Mock<ILogger> _loggerMock = new Mock<ILogger>();

        [Fact]
        public async Task Test1()
        {
            // arrange
            var functionName = "Function1";
            var instanceId = "testid";

            _durableOrchestrationClientMock.
                Setup(x => x.StartNewAsync(functionName, It.IsAny<string>(), It.IsAny<object>())).
                ReturnsAsync(instanceId);

            _durableOrchestrationClientMock
                .Setup(x => x.CreateCheckStatusResponse(It.IsAny<HttpRequestMessage>(), instanceId, false))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty),
                    Headers =
                    {
                        RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(10))
                    }
                });

            // act
            var result = await HttpStart.Run(
                new HttpRequestMessage()
                {
                    Content = new StringContent("{}", Encoding.UTF8, "application/json"),
                    RequestUri = new Uri("http://localhost:7071/orchestrators/E1_HelloSequence"),
                },
                _durableOrchestrationClientMock.Object,
                _loggerMock.Object);

            // assert
            Assert.NotNull(result.Headers.RetryAfter);
            Assert.Equal(TimeSpan.FromSeconds(10), result.Headers.RetryAfter.Delta);
        }
    }
}
