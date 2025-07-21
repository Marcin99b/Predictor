using FluentAssertions;
using Predictor.Web.Models;

namespace Predictor.Tests.Integration;

public class BudgetCalculationTests : BasePredictionTest
{
    public static IEnumerable<object[]> BudgetAccumulationTestCases()
    {
        yield return new object[] { 1000m, 3000m, 2000m, 3 };
        yield return new object[] { 5000m, 1000m, 1500m, 2 };
        yield return new object[] { 0m, 5000m, 2000m, 2 };
        yield return new object[] { 2500m, 4200m, 3100m, 4 };
    }

    [TestCaseSource(nameof(BudgetAccumulationTestCases))]
    public async Task Prediction_ShouldAccumulateBudgetCorrectly(
        decimal initialBudget, decimal income, decimal expense, int months)
    {
        // Arrange
        var request = CreateBasicRequest(months, initialBudget) with
        {
            Incomes = [CreateIncome("Income", income, "USD", frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Expense", expense, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var monthlyBalance = income - expense;
        for (var i = 0; i < months; i++)
        {
            var expectedBudget = initialBudget + monthlyBalance * (i + 1);
            _ = result.Months[i].BudgetAfter.Should().Be(expectedBudget,
                $"Month {i + 1} should have budget {expectedBudget}");
        }

        _ = result.Summary.TotalIncome.Should().Be(income * months);
        _ = result.Summary.TotalExpenses.Should().Be(expense * months);
    }

    [Test]
    public async Task Prediction_WithMultipleSources_ShouldSumCorrectly()
    {
        // Arrange
        var request = CreateBasicRequest(1) with
        {
            Incomes = [
                CreateIncome("Salary", 3000m, "USD"),
                CreateIncome("Bonus", 2000m, "USD")
            ],
            Expenses = [
                CreateExpense("Rent", 1200m, "USD"),
                CreateExpense("Food", 800m, "USD")
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months[0].Income.Should().Be(5000m);
        _ = result.Months[0].Expense.Should().Be(2000m);
        _ = result.Months[0].Balance.Should().Be(3000m);
    }

    [Test]
    public async Task Prediction_WithOnlyIncomes_ShouldIncreaseBalance()
    {
        // Arrange
        var request = CreateBasicRequest(2, 1000m) with
        {
            Incomes = [CreateIncome("Salary", 2000m, "USD", frequency: Frequency.Monthly)],
            Expenses = []
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months[0].BudgetAfter.Should().Be(3000m);
        _ = result.Months[1].BudgetAfter.Should().Be(5000m);
        _ = result.Summary.TotalIncome.Should().Be(4000m);
        _ = result.Summary.TotalExpenses.Should().Be(0m);
    }

    [Test]
    public async Task Prediction_WithOnlyExpenses_ShouldDecreaseBalance()
    {
        // Arrange
        var request = CreateBasicRequest(2, 5000m) with
        {
            Incomes = [],
            Expenses = [CreateExpense("Rent", 1500m, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months[0].BudgetAfter.Should().Be(3500m);
        _ = result.Months[1].BudgetAfter.Should().Be(2000m);
        _ = result.Summary.TotalIncome.Should().Be(0m);
        _ = result.Summary.TotalExpenses.Should().Be(3000m);
    }

    [Test]
    public async Task Prediction_WithZeroInitialBudget_ShouldCalculateCorrectly()
    {
        // Arrange
        var request = CreateBasicRequest(1, 0m) with
        {
            Incomes = [CreateIncome("Income", 1000m, "USD")],
            Expenses = [CreateExpense("Expense", 300m, "USD")]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months[0].Balance.Should().Be(700m);
        _ = result.Months[0].BudgetAfter.Should().Be(700m);
    }
}