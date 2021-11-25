using System.Collections.Generic;
using System.Threading.Tasks;
using ApiDudes.Model;

namespace ApiDudes.Services;
public interface ICosmosDbService
{
    public Task AddCoinAsync(CoinModel item);
    public Task DeleteCoinAsync(string id);
    public Task<CoinModel> GetCoinAsync(string id);
    public Task<IEnumerable<CoinModel>> GetCoinsAsync(string query);
    public Task UpdateCoinAsync(string id, CoinModel item);
}