using Xunit;
using System.IO;
using ApiDudes.CoinDataScheduleTrigger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests;

public class SchedulerTriggerTests
{
    [Fact]
    public void SimplePassingTest()
    {
        var json = GetJsonFile();

        var result = JsonConvert.DeserializeObject<CoinModel>(json);     

        Assert.NotNull(json);
    }

    private string GetJsonFile()
    {
        return File.ReadAllText(@"testdata.json");
    }
}