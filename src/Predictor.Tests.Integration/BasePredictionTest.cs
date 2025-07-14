using Microsoft.AspNetCore.Mvc.Testing;
using Predictor.Web;
using Predictor.Web.Models;
using System.Net;
using System.Net.Http.Json;

namespace Predictor.Tests.Integration;

public abstract class BasePredictionTest
{
    protected WebApplicationFactory<Program> factory = null!;
    protected HttpClient client = null!;
    protected Uri baseUrl = null!;

    [SetUp]
    public void Setup()
    {
        this.factory = new WebApplicationFactory<Program>();
        this.client = this.factory.CreateClient();
        this.baseUrl = new Uri(this.client.BaseAddress!, "/api/v1/predictions");
    }

    [TearDown]
    public void TearDown()
    {
        this.client?.Dispose();
        this.factory?.Dispose();
    }

    protected async Task<PredictionResult> GetPredictionResult(PredictionRequest request)
    {
        var response = await this.client.PostAsJsonAsync(this.baseUrl, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PredictionResult>())!;
    }

    protected async Task<HttpStatusCode> GetResponseStatusCode(PredictionRequest request)
    {
        var response = await this.client.PostAsJsonAsync(this.baseUrl, request);
        return response.StatusCode;
    }

    protected static PredictionRequest CreateBasicRequest(int months = 3, decimal initialBudget = 0m) => new(
        PredictionMonths: months,
        InitialBudget: initialBudget,
        StartPredictionMonth: new MonthDate(1, 2025),
        Incomes: [],
        Expenses: []);

    protected static PaymentItem CreateIncome(string name, decimal value, int month = 1, int year = 2025,
        Frequency frequency = Frequency.OneTime, MonthDate? endDate = null) =>
        new(name, value, new MonthDate(month, year), frequency, endDate);

    protected static PaymentItem CreateExpense(string name, decimal value, int month = 1, int year = 2025,
        Frequency frequency = Frequency.OneTime, MonthDate? endDate = null) =>
        new(name, value, new MonthDate(month, year), frequency, endDate);
}