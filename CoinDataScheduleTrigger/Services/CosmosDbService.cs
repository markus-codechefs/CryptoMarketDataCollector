using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDudes.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace ApiDudes.Services;
public class CosmosDbService : ICosmosDbService
{
    private Container container;

    public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
    {
        this.container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddCoinAsync(CoinModel coin)
    {
        await this.container.CreateItemAsync<CoinModel>(coin, new PartitionKey(coin.Status.Timestamp.ToString()));
    }

    public async Task DeleteCoinAsync(string id)
    {
        await this.container.DeleteItemAsync<CoinModel>(id, new PartitionKey(id));
    }

    public async Task<CoinModel> GetCoinAsync(string id)
    {
        try
        {
            ItemResponse<CoinModel> response = await this.container.ReadItemAsync<CoinModel>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<CoinModel>> GetCoinsAsync(string queryString)
    {
        var query = this.container.GetItemQueryIterator<CoinModel>(new QueryDefinition(queryString));
        List<CoinModel> results = new List<CoinModel>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task UpdateCoinAsync(string id, CoinModel item)
    {
        await this.container.UpsertItemAsync<CoinModel>(item, new PartitionKey(id));
    }
}
