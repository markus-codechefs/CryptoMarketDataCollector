using Xunit;
using System.IO;
using Newtonsoft.Json;
using ApiDudes.Model;

namespace Tests;

public class SchedulerTriggerTests
{
    [Fact]
    public void SimplePassingTest()
    {
        var json = GetJsonFile();

        var result = JsonConvert.DeserializeObject<CoinModel>(json);     

        Assert.NotNull(json);
        Assert.NotNull(result);
        Assert.Equal("BTC", result.Data[0].Symbol);
    }

    private string GetJsonFile()
    {
        return File.ReadAllText(@"testdata.json");
    }
}