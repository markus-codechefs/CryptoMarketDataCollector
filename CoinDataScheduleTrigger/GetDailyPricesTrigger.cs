using System;
using System.IO;
using System.Threading.Tasks;
using ApiDudes.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace ApiDudes.CoinDataScheduleTrigger;
public class GetDailyPricesTrigger
{
    [FunctionName("GetDailyPricesTrigger")]
    public async Task Run(
        [TimerTrigger("15 */8 * * *")]
        TimerInfo myTimer, [CosmosDB(databaseName: "CryptoMarketDataDB", collectionName: "coindata", ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<dynamic> documentsOut,
        ILogger log)
    {
        log.LogInformation($"GetDailyPricesTrigger function executed at: {DateTime.Now}");

        try
        {
            string BASE_URL = System.Environment.GetEnvironmentVariable("BASEURL", EnvironmentVariableTarget.Process);
            string APIHEADER = System.Environment.GetEnvironmentVariable("APIHEADER", EnvironmentVariableTarget.Process);
            string APIKEY = System.Environment.GetEnvironmentVariable("APIKEY", EnvironmentVariableTarget.Process);
            string RESOURCE = System.Environment.GetEnvironmentVariable("RESOURCE", EnvironmentVariableTarget.Process);

            var client = new RestClient(BASE_URL);
            client.AddDefaultHeader(APIHEADER, APIKEY);

            var request = new RestRequest(RESOURCE, Method.GET);
            request.AddQueryParameter("start", "1");
            request.AddQueryParameter("limit", "5000");
            request.AddQueryParameter("convert", "CHF");

            //var response = client.Get(request);

            var model = JsonConvert.DeserializeObject<CoinModel>(GetJsonFile());

            if (!string.IsNullOrEmpty(GetJsonFile()) && model != null)
            {
                await documentsOut.AddAsync(model);
            }

            log.LogInformation("Success");
        }
        catch (System.Exception ex)
        {
            log.LogInformation($"Error: {ex.Message}");
        }
    }
    private string GetJsonFile()
    {
        return File.ReadAllText(@"CoinmarketcapData-20211118.json");
    }
}

