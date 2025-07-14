using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Predictor.Web;
using Predictor.Web.Models;
using System.Net.Http.Json;

namespace Predictor.Tests.Integration;

public class PredictionsTests
{
    private WebApplicationFactory<Program> factory;
    private HttpClient client;
    private Uri baseUrl;

    [SetUp]
    public void Setup()
    {
        this.factory = new WebApplicationFactory<Program>();
        this.client = this.factory.CreateClient();
        this.baseUrl = new Uri(this.client.BaseAddress!, "/api/v1/predictions");
    }

    [Test]
    public async Task Prediction_ShouldCalculateBalance()
    {
        //Arrange
        var exceptedIncome = 10m;
        var exceptedExpense = 5m;
        var exceptedBalance = 5m;
        var monthsCount = 2;

        var request = new PredictionRequest(
            PredictionMonths: monthsCount, 
            InitialBudget: 0m, 
            StartPredictionMonth: new MonthDate(1, 2025), 
            Incomes: [new PaymentItem("basic_income", exceptedIncome, new MonthDate(1, 2025), Frequency.Monthly)], 
            Expenses: [new PaymentItem("basic_expense", exceptedExpense, new MonthDate(1, 2025), Frequency.Monthly)]);

        //Act
        var response = await this.client.PostAsJsonAsync(this.baseUrl, request);
        response.EnsureSuccessStatusCode();
        var result = (await response.Content.ReadFromJsonAsync<PredictionResult>())!;

        //Assert
        result.Summary.TotalIncome.Should().Be(exceptedIncome * monthsCount);
        result.Summary.TotalExpenses.Should().Be(exceptedExpense * monthsCount);
        result.Summary.StartingBalance.Should().Be(exceptedBalance);
        result.Summary.EndingBalance.Should().Be(exceptedBalance);

        result.Months.Should().AllSatisfy(x => x.Income.Should().Be(exceptedIncome));
        result.Months.Should().AllSatisfy(x => x.Expense.Should().Be(exceptedExpense));
        result.Months.Should().AllSatisfy(x => x.Balance.Should().Be(exceptedBalance));

        result.Months.First().BudgetAfter.Should().Be(exceptedBalance);
        result.Months.Last().BudgetAfter.Should().Be(exceptedBalance * monthsCount);
    }
}