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
    public async Task ApiShouldHandle_1000_ExampleCalculations_PerSecond_With100Users()
    {
        using var httpClient = new HttpClient();
        var url = "https://localhost:7176";
        var durationSeconds = 5;
        var minimumPerSecond = 1000;
        var maxLatency99 = 120;
        var maxLatency95 = 110;
        var maxLatency50 = 70;

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

        stats.AllOkCount.Should().BeGreaterThan(durationSeconds * minimumPerSecond);

        var latencyStats = stats.ScenarioStats[0].Ok.Latency;

        latencyStats.LatencyCount.LessOrEq800.Should().BeGreaterThanOrEqualTo(durationSeconds * minimumPerSecond);
        latencyStats.Percent99.Should().BeLessThanOrEqualTo(maxLatency99);
        latencyStats.Percent95.Should().BeLessThanOrEqualTo(maxLatency95);
        latencyStats.Percent50.Should().BeLessThanOrEqualTo(maxLatency50);

        stats.AllFailCount.Should().Be(0);
    }
}
