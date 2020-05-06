using MaokiDurableFunction;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class MyActivityTest
    {
        private Mock<IDurableActivityContext> _durableActivityContextMock = new Mock<IDurableActivityContext>();
        private Mock<ILogger> _loggerMock = new Mock<ILogger>();

        [Fact]
        public void Function1HelloTest()
        {
            _durableActivityContextMock.Setup(x => x.GetInput<string>()).Returns("John");
            var result = Function1.SayHello(_durableActivityContextMock.Object, _loggerMock.Object);
            Assert.Equal("Hello John!", result);
        }

        [Fact]
        public void Function1Hello2Test()
        {
            var result = Function1.SayHello("John", _loggerMock.Object);
            Assert.Equal("Hello John!", result);
        }
    }
}
