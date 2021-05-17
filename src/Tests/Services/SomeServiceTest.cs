using Infrastructure.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class SomeServiceTest
    {
        private Mock<HttpMessageHandler> _httpHandler;

        [Fact]
        public async Task Get_FailedFetchingData_ThrowsException()
        {
            var service = CreateService();

            SetupHttpResponse(null, HttpStatusCode.ServiceUnavailable);

            await Assert.ThrowsAnyAsync<Exception>(() => { return service.GetSomething(); });

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task Get_SuccessFetchingData_StringList()
        {
            var service = CreateService();
            var response = new List<string>() { "one", "two", "three" };

            SetupHttpResponse(response);

            var result = await service.GetSomething();

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }

        private void SetupHttpResponse(object response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string responseJson = response is string ? (string)response : JsonSerializer.Serialize(response);

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseJson),
            };

            _httpHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(httpResponse);
        }

        private SomeService CreateService()
        {
            _httpHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpHandler.Object)
            {
                BaseAddress = new Uri("https://tempuri.org")
            };

            return new SomeService(httpClient);
        }
    }
}
