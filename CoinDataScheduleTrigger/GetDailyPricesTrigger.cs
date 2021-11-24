using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RestSharp;
using Newtonsoft.Json;

namespace ApiDudes.CoinDataScheduleTrigger
{
    public class GetDailyPricesTrigger
    {
        [FunctionName("GetDailyPricesTrigger")]
        public void Run([TimerTrigger("15 */8 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"GetDailyPricesTrigger function executed at: {DateTime.Now}");

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

            var response = client.Get(request);

            var model = JsonConvert.DeserializeObject<CoinModel>(response.Content);
        }
    }    
}

