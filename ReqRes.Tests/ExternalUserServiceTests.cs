using Moq;
using ReqRes.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using ReqRes.Core.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using System;

public class ExternalUserServiceTests
{
    [Fact]
    public async Task GetUserByIdAsync_Returns_User()
    {
        var user = new User { Id = 2, FirstName = "Janet", LastName = "Weaver", Email = "janet@reqres.in" };
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new UserResponse { Data = [user] }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var client = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://reqres.in/api/") };
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var logger = new Mock<ILogger<ExternalUserService>>();

        var service = new ExternalUserService(client, memoryCache, logger.Object);

        var result = await service.GetUserByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("Janet", result?.FirstName);
    }
}
