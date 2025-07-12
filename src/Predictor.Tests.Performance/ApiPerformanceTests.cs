using FluentAssertions;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using NUnit.Framework.Internal;
using System.Diagnostics;
using System.Text;

namespace Predictor.Tests.Performance;

[TestFixture]
public class ApiPerformanceTests
{
    [SetUp]
    public void Setup()
    {
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    [Test]
    [Ignore("performance test")]
    public async Task ApiShouldHandle_100_ExampleCalculations_PerSecond_With100Users()
    {
        using var httpClient = new HttpClient();
        var url = "https://localhost:7176";
        var durationSeconds = 5;
        var minimumPerSecond = 100;

        var exampleData = await (await httpClient.GetAsync(url + "/example-data"))
            .Content
            .ReadAsStringAsync();

        var scenario = Scenario.Create("calc_requests", async context =>
        {
            var request =
                Http.CreateRequest("POST", url + "/calc")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new StringContent(exampleData, Encoding.UTF8, "application/json"));

            var response = await Http.Send(httpClient, request);

            return response;
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(
            Simulation.KeepConstant(100, TimeSpan.FromSeconds(durationSeconds))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        Trace.WriteLine($"AllRequestCount: {stats.AllRequestCount} AllFailCount: {stats.AllFailCount}");

        stats.AllFailCount.Should().Be(0);
        stats.AllRequestCount.Should().BeGreaterThan(durationSeconds * minimumPerSecond);
        /* 
        MinMs = 1.22
        MeanMs = 73.0
        MaxMs = 3576.19
        Percent50 = 68.35
        Percent75 = 84.29
        Percent95 = 105.02
        Percent99 = 119.68
        StdDev = 186.94
        LatencyCount = {
            LessOrEq800 = 5963
            More800Less1200 = 6
            MoreOrEq1200 = 37 
        }
        */
        var latencyStats = stats.ScenarioStats[0].Ok.Latency;
        latencyStats.Percent99.Should().BeLessThanOrEqualTo(120);
        latencyStats.Percent95.Should().BeLessThanOrEqualTo(110);
        latencyStats.Percent50.Should().BeLessThanOrEqualTo(70);

    }
}
