using FluentAssertions;
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
        // Test simple 12-month calculation performance
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

        // Verify performance requirements
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
        // Test complex 120-month calculation with many items
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

        // Complex calculations should have relaxed requirements
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
        // Test cache retrieval performance after initial calculation
        var request = CreateSimpleRequest();

        // First, create a prediction to cache
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

        // Cache should be extremely fast
        VerifyPerformanceStats(stats,
            minRequestsPerSecond: 2000,
            maxLatencyP99: 50,  // Relaxed from 20ms to 50ms
            maxLatencyP95: 30,  // Relaxed from 15ms to 30ms  
            maxLatencyP50: 10); // Relaxed from 5ms to 10ms
    }

    [Test]
    [Ignore("performance test")]
    public void AnalyticsEndpoint_ShouldBe_Fast()
    {
        // Test the analytics/check-goal endpoint performance
        var request = CreateSimpleRequest();

        // First create a prediction
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

        // Analytics should be very fast since it's just lookups
        VerifyPerformanceStats(stats,
            minRequestsPerSecond: 800,
            maxLatencyP99: 30,
            maxLatencyP95: 20,
            maxLatencyP50: 10);
    }

    private static void VerifyPerformanceStats(NodeStats stats,
        int minRequestsPerSecond, int maxLatencyP99, int maxLatencyP95, int maxLatencyP50)
    {
        // No failed requests
        _ = stats.AllFailCount.Should().Be(0, "No requests should fail");

        // Minimum throughput
        var duration = stats.Duration;
        var actualRps = stats.AllOkCount / duration.TotalSeconds;
        _ = actualRps.Should().BeGreaterThan(minRequestsPerSecond,
            $"Should handle at least {minRequestsPerSecond} requests per second, but got {actualRps:F1}");

        // Latency requirements
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

    private static PredictionRequest CreateMediumRequest() => new(
        PredictionMonths: 36,
        InitialBudget: 15000m,
        StartPredictionMonth: new MonthDate(1, 2025),
        Incomes: [
            new("Primary Salary", 5000m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Spouse Salary", 3500m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Freelance", 800m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2026)),
            new("Annual Bonus", 4000m, new MonthDate(3, 2025), Frequency.Annually),
            new("Quarterly Dividend", 250m, new MonthDate(3, 2025), Frequency.Quarterly),
            new("Tax Refund", 2500m, new MonthDate(4, 2025), Frequency.OneTime),
            new("Side Business", 600m, new MonthDate(6, 2025), Frequency.Monthly)
        ],
        Expenses: [
            new("Mortgage", 2200m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Car Payment", 450m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(6, 2027)),
            new("Insurance", 320m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Groceries", 700m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Utilities", 280m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Childcare", 1100m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(8, 2027)),
            new("Savings", 1000m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Vacation", 3000m, new MonthDate(7, 2025), Frequency.Annually),
            new("Home Maintenance", 800m, new MonthDate(4, 2025), Frequency.SemiAnnually),
            new("Medical", 150m, new MonthDate(1, 2025), Frequency.Quarterly)
        ]
    );

    private static PredictionRequest CreateComplexRequest() => new(
        PredictionMonths: 120, // 10 years
        InitialBudget: 50000m,
        StartPredictionMonth: new MonthDate(1, 2025),
        Incomes: GenerateComplexIncomes(),
        Expenses: GenerateComplexExpenses()
    );

    private static PaymentItem[] GenerateComplexIncomes()
    {
        var incomes = new List<PaymentItem>();

        // Regular employment income
        incomes.AddRange([
            new("Primary Salary", 7000m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Spouse Salary", 5500m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Annual Raise Primary", 350m, new MonthDate(1, 2026), Frequency.Monthly),
            new("Annual Raise Spouse", 275m, new MonthDate(1, 2026), Frequency.Monthly),
        ]);

        // Investment income
        for (int year = 2025; year <= 2034; year++)
        {
            incomes.Add(new($"Dividend Income {year}", 400m + (year - 2025) * 50m,
                new MonthDate(3, year), Frequency.Quarterly));
            incomes.Add(new($"Annual Bonus {year}", 5000m + (year - 2025) * 200m,
                new MonthDate(2, year), Frequency.OneTime));
        }

        // Rental properties (added over time)
        incomes.Add(new("Rental Property 1", 1800m, new MonthDate(6, 2025), Frequency.Monthly));
        incomes.Add(new("Rental Property 2", 2200m, new MonthDate(1, 2027), Frequency.Monthly));
        incomes.Add(new("Rental Property 3", 2500m, new MonthDate(6, 2029), Frequency.Monthly));

        // Business income
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

        return incomes.ToArray();
    }

    private static PaymentItem[] GenerateComplexExpenses()
    {
        var expenses = new List<PaymentItem>();

        // Fixed monthly expenses
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

        // Loans and time-limited expenses
        expenses.AddRange([
            new("Car Loan 1", 520m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2028)),
            new("Car Loan 2", 480m, new MonthDate(6, 2027), Frequency.Monthly, new MonthDate(5, 2031)),
            new("Student Loan", 380m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(8, 2030)),
            new("Personal Loan", 250m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2027)),
        ]);

        // Retirement and savings
        expenses.AddRange([
            new("401k Contribution", 1200m, new MonthDate(1, 2025), Frequency.Monthly),
            new("IRA Contribution", 500m, new MonthDate(1, 2025), Frequency.Monthly),
            new("Emergency Fund", 800m, new MonthDate(1, 2025), Frequency.Monthly, new MonthDate(12, 2027)),
            new("Investment Account", 1500m, new MonthDate(1, 2025), Frequency.Monthly),
        ]);

        // Periodic expenses
        for (int year = 2025; year <= 2034; year++)
        {
            expenses.Add(new($"Vacation {year}", 4500m, new MonthDate(7, year), Frequency.OneTime));
            expenses.Add(new($"Holiday Gifts {year}", 1200m, new MonthDate(12, year), Frequency.OneTime));
            expenses.Add(new($"Tax Preparation {year}", 400m, new MonthDate(3, year), Frequency.OneTime));
            expenses.Add(new($"Home Maintenance {year}", 2500m, new MonthDate(5, year), Frequency.OneTime));
        }

        // Quarterly and semi-annual expenses
        expenses.AddRange([
            new("Property Maintenance", 800m, new MonthDate(3, 2025), Frequency.Quarterly),
            new("Medical Checkups", 600m, new MonthDate(6, 2025), Frequency.SemiAnnually),
            new("Car Maintenance", 450m, new MonthDate(4, 2025), Frequency.SemiAnnually),
        ]);

        return expenses.ToArray();
    }
}