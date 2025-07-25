﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Predictor.Web;
using Predictor.Web.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Predictor.Tests.Performance;

[TestFixture]
public class ApiPerformanceTests
{
    private WebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;

    [SetUp]
    public void Setup()
    {
        this.factory = new WebApplicationFactory<Program>();
        this.client = this.factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        this.client?.Dispose();
        this.factory?.Dispose();
    }

    [Test]
    [Ignore("performance test")]
    public void SimpleCalculation_ShouldHandle_HighThroughput()
    {
        var simpleRequest = CreateSimpleRequest();
        var requestJson = JsonSerializer.Serialize(simpleRequest);

        var scenario = Scenario.Create("simple_calculation", async context =>
        {
            var request = Http.CreateRequest("POST", "/api/v1/predictions")
                .WithHeader("Content-Type", "application/json")
                .WithBody(new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json"));

            return await Http.Send(this.client, request);
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(3))
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 50, during: TimeSpan.FromSeconds(10))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        VerifyPerformanceStats(stats,
            minRequestsPerSecond: 500,
            maxLatencyP99: 100,
            maxLatencyP95: 80,
            maxLatencyP50: 40);
    }

    [Test]
    [Ignore("performance test")]
    public void ComplexCalculation_ShouldHandle_ReasonableLoad()
    {
        var complexRequest = CreateComplexRequest();
        var requestJson = JsonSerializer.Serialize(complexRequest);

        var scenario = Scenario.Create("complex_calculation", async context =>
        {
            var request = Http.CreateRequest("POST", "/api/v1/predictions")
                .WithHeader("Content-Type", "application/json")
                .WithBody(new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json"));

            return await Http.Send(this.client, request);
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(15))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        VerifyPerformanceStats(stats,
            minRequestsPerSecond: 100,
            maxLatencyP99: 500,
            maxLatencyP95: 400,
            maxLatencyP50: 200);
    }

    [Test]
    [Ignore("performance test")]

    public void CacheRetrieval_ShouldBe_VeryFast()
    {
        var request = CreateSimpleRequest();

        var predictionResult = this.client.PostAsJsonAsync("/api/v1/predictions", request)
            .GetAwaiter().GetResult().Content.ReadFromJsonAsync<PredictionResult>()
            .GetAwaiter().GetResult()!;

        var scenario = Scenario.Create("cache_retrieval", async context =>
        {
            var request = Http.CreateRequest("GET", $"/api/v1/predictions/{predictionResult.Id}");
            return await Http.Send(this.client, request);
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(2))
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 100, during: TimeSpan.FromSeconds(10))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        VerifyPerformanceStats(stats,
            minRequestsPerSecond: 2000,
            maxLatencyP99: 50,              
            maxLatencyP95: 30,              
            maxLatencyP50: 10);     
    }

    [Test]
    [Ignore("performance test")]
    public void AnalyticsEndpoint_ShouldBe_Fast()
    {
        var request = CreateSimpleRequest();

        var predictionResult = this.client.PostAsJsonAsync("/api/v1/predictions", request)
            .GetAwaiter().GetResult().Content.ReadFromJsonAsync<PredictionResult>()
            .GetAwaiter().GetResult()!;

        var checkGoalRequest = new CheckGoalRequest(
            PredictionId: predictionResult.Id,
            Month: new MonthDate(6, 2025),
            BalanceHigherOrEqual: 1000m,
            IncomeHigherOrEqual: 500m,
            ExpenseLowerOrEqual: 2000m
        );

        var requestJson = JsonSerializer.Serialize(checkGoalRequest);

        var scenario = Scenario.Create("analytics_check_goal", async context =>
        {
            var request = Http.CreateRequest("POST", "/api/v1/analytics/check-goal")
                .WithHeader("Content-Type", "application/json")
                .WithBody(new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json"));

            return await Http.Send(this.client, request);
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(2))
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 80, during: TimeSpan.FromSeconds(8))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

                VerifyPerformanceStats(stats,
            minRequestsPerSecond: 800,
            maxLatencyP99: 30,
            maxLatencyP95: 20,
            maxLatencyP50: 10);
    }

    private static void VerifyPerformanceStats(NodeStats stats,
        int minRequestsPerSecond, int maxLatencyP99, int maxLatencyP95, int maxLatencyP50)
    {
                _ = stats.AllFailCount.Should().Be(0, "No requests should fail");

                var duration = stats.Duration;
        var actualRps = stats.AllOkCount / duration.TotalSeconds;
        _ = actualRps.Should().BeGreaterThan(minRequestsPerSecond,
            $"Should handle at least {minRequestsPerSecond} requests per second, but got {actualRps:F1}");

                var mainScenario = stats.ScenarioStats.First();
        _ = mainScenario.Ok.Latency.Percent99.Should().BeLessThanOrEqualTo(maxLatencyP99,
            $"99th percentile latency should be ≤ {maxLatencyP99}ms");
        _ = mainScenario.Ok.Latency.Percent95.Should().BeLessThanOrEqualTo(maxLatencyP95,
            $"95th percentile latency should be ≤ {maxLatencyP95}ms");
        _ = mainScenario.Ok.Latency.Percent50.Should().BeLessThanOrEqualTo(maxLatencyP50,
            $"50th percentile (median) latency should be ≤ {maxLatencyP50}ms");
    }

    private static PredictionRequest CreateSimpleRequest() => new(
        PredictionMonths: 12,
        InitialBudget: 5000m,
        StartPredictionMonth: new MonthDate(1, 2025),
        Incomes: [
            new("Salary", 4000m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Bonus", 2000m, new MonthDate(6, 2025), Frequency.OneTime)
        ],
        Expenses: [
            new("Rent", 1200m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Food", 500m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Utilities", 200m, new MonthDate(1, 2025), Frequency.Monthly)
        ]
    );

    private static PredictionRequest CreateComplexRequest() => new(
        PredictionMonths: 120,         InitialBudget: 50000m,
        StartPredictionMonth: new MonthDate(1, 2025),
        Incomes: GenerateComplexIncomes(),
        Expenses: GenerateComplexExpenses()
    );

    private static PaymentItem[] GenerateComplexIncomes()
    {
        var incomes = new List<PaymentItem>();

        incomes.AddRange([
            new("Primary Salary", 7000m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Spouse Salary", 5500m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Annual Raise Primary", 350m, new MonthDate(1, 2026), Frequency.Monthly),
            new("Annual Raise Spouse", 275m, new MonthDate(1, 2026), Frequency.Monthly),
        ]);

        for (int year = 2025; year <= 2034; year++)
        {
            incomes.Add(new($"Dividend Income {year}", 400m + (year - 2025) * 50m,
                new MonthDate(3, year), Frequency.Quarterly));
            incomes.Add(new($"Annual Bonus {year}", 5000m + (year - 2025) * 200m,
                new MonthDate(2, year), Frequency.OneTime));
        }

        incomes.Add(new("Rental Property 1", 1800m, new MonthDate(6, 2025), Frequency.Monthly));
        incomes.Add(new("Rental Property 2", 2200m, new MonthDate(1, 2027), Frequency.Monthly));
        incomes.Add(new("Rental Property 3", 2500m, new MonthDate(6, 2029), Frequency.Monthly));

        for (int month = 1; month <= 120; month += 3)
        {
            var baseDate = new MonthDate(1, 2025);
            var monthDate = month == 1 ? baseDate : baseDate.AddMonths(month - 1);
            if (monthDate.Year <= 2034)
            {
                incomes.Add(new($"Consulting Q{(month - 1) / 3 + 1}-{monthDate.Year}",
                    1200m + month * 10m, monthDate, Frequency.OneTime));
            }
        }

        return [.. incomes];
    }

    private static PaymentItem[] GenerateComplexExpenses()
    {
        var expenses = new List<PaymentItem>();

        expenses.AddRange([
            new("Mortgage", 3200m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Property Tax", 650m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Home Insurance", 280m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Car Insurance", 220m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Health Insurance", 950m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Life Insurance", 180m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Utilities", 320m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Internet/Phone", 180m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Groceries", 900m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Transportation", 400m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Entertainment", 350m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Personal Care", 200m, new MonthDate(1, 2025), Frequency.Monthly),
        ]);

        expenses.AddRange([
            new("Car Loan 1", 520m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2028)),
            new("Car Loan 2", 480m, new MonthDate(6, 2027), Frequency.Monthly, new MonthDate(5, 2031)),
            new("Student Loan", 380m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(8, 2030)),
            new("Personal Loan", 250m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2027)),
        ]);

        expenses.AddRange([
            new("401k Contribution", 1200m, new MonthDate(1, 2025), Frequency.Monthly),
            new("IRA Contribution", 500m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Emergency Fund", 800m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2027)),
            new("Investment Account", 1500m, new MonthDate(1, 2025), Frequency.Monthly),
        ]);

        for (int year = 2025; year <= 2034; year++)
        {
            expenses.Add(new($"Vacation {year}", 4500m, new MonthDate(7, year), Frequency.OneTime));
            expenses.Add(new($"Holiday Gifts {year}", 1200m, new MonthDate(12, year), Frequency.OneTime));
            expenses.Add(new($"Tax Preparation {year}", 400m, new MonthDate(3, year), Frequency.OneTime));
            expenses.Add(new($"Home Maintenance {year}", 2500m, new MonthDate(5, year), Frequency.OneTime));
        }

        expenses.AddRange([
            new("Property Maintenance", 800m, new MonthDate(3, 2025), Frequency.Quarterly),
            new("Medical Checkups", 600m, new MonthDate(6, 2025), Frequency.SemiAnnually),
            new("Car Maintenance", 450m, new MonthDate(4, 2025), Frequency.SemiAnnually),
        ]);

        return [.. expenses];
    }
}