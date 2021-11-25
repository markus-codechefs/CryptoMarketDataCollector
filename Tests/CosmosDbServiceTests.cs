using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiDudes.Model;
using ApiDudes.Services;
using Moq;
using Xunit;

namespace Tests;

public class CosmosDbServiceTests
{
    [Fact]
    public void MockCosmosDbService()
    {
        var cosmosDbService = new Mock<ICosmosDbService>();

        Expression<Func<ICosmosDbService, Task>> addCoinAsync = x =>
            x.AddCoinAsync(It.IsAny<CoinModel>());

        cosmosDbService
            .Setup(addCoinAsync)
            .Returns((CoinModel item) =>
            {
                return default;
            });

        cosmosDbService.Object.AddCoinAsync(new CoinModel());

        cosmosDbService.Verify(addCoinAsync, Times.Once);
    }
}