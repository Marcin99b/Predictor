using FluentAssertions;
using Predictor.Web.Models;

namespace Predictor.Tests.Integration;

public class SummaryStatisticsTests : BasePredictionTest
{
    [Test]
    public async Task Prediction_ShouldCalculateCorrectSummaryStatistics()
    {
        // Arrange - Month 1: -2000, Month 2: +7000, Month 3: +2000
        var request = CreateBasicRequest(3, 2000m) with
        {
            Incomes = [
                CreateIncome("Regular Salary", 3000m, "USD", frequency: Frequency.Monthly),
                CreateIncome("Big Bonus", 5000m, "USD", month: 2, frequency: Frequency.OneTime)
            ],
            Expenses = [
                CreateExpense("Big Purchase", 4000m, "USD", frequency: Frequency.OneTime),
                CreateExpense("Regular Expense", 1000m, "USD", frequency: Frequency.Monthly)
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(14000m);
        _ = result.Summary.TotalExpenses.Should().Be(7000m);
        _ = result.Summary.LowestBalance.Should().Be(-2000m);
        _ = result.Summary.HighestBalance.Should().Be(7000m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(1, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(2, 2025));
    }

    [Test]
    public async Task Prediction_WithConstantBalance_ShouldHaveSameLowestAndHighest()
    {
        // Arrange
        var request = CreateBasicRequest(3) with
        {
            Incomes = [CreateIncome("Income", 1000m, "USD", frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Expense", 1000m, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.LowestBalance.Should().Be(0m);
        _ = result.Summary.HighestBalance.Should().Be(0m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(1, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(1, 2025));
    }

    [Test]
    public async Task Prediction_WithIncreasingBalance_ShouldHaveCorrectMinMax()
    {
        // Arrange
        var request = CreateBasicRequest(4, 1000m) with
        {
            Incomes = [CreateIncome("Growing Income", 2000m, "USD", frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Fixed Expense", 1000m, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.LowestBalance.Should().Be(1000m);
        _ = result.Summary.HighestBalance.Should().Be(1000m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(1, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(1, 2025));
    }

    [Test]
    public async Task Prediction_WithDecreasingBalance_ShouldHaveCorrectMinMax()
    {
        // Arrange
        var request = CreateBasicRequest(3, 5000m) with
        {
            Incomes = [],
            Expenses = [CreateExpense("Heavy Expense", 1500m, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert

        _ = result.Summary.LowestBalance.Should().Be(-1500m);
        _ = result.Summary.HighestBalance.Should().Be(-1500m);
        _ = result.Summary.StartingBalance.Should().Be(-1500m);
        _ = result.Summary.EndingBalance.Should().Be(-1500m);
    }

    [Test]
    public async Task Prediction_WithFluctuatingBalance_ShouldFindCorrectExtremes()
    {
        // Arrange - Create fluctuating pattern: +2000, -3000, +4000, -1000
        var request = CreateBasicRequest(4) with
        {
            Incomes = [
                CreateIncome("Regular Income", 1000m, "USD", frequency: Frequency.Monthly),
                CreateIncome("Bonus Month 1", 1000m, "USD", month: 1, frequency: Frequency.OneTime),
                CreateIncome("Big Bonus Month 3", 3000m, "USD", month: 3, frequency: Frequency.OneTime)
            ],
            Expenses = [
                CreateExpense("Big Expense Month 2", 4000m, "USD", month: 2, frequency: Frequency.OneTime),
                CreateExpense("Regular Expense", 100m, "USD", frequency: Frequency.Monthly)             ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert

        _ = result.Summary.LowestBalance.Should().Be(-3100m);
        _ = result.Summary.HighestBalance.Should().Be(3900m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(2, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(3, 2025));
    }

    [Test]
    public async Task Prediction_WithOnlyOneMonth_ShouldHaveSameStartAndEnd()
    {
        // Arrange
        var request = CreateBasicRequest(1, 5000m) with
        {
            Incomes = [CreateIncome("Single Income", 2000m, "USD")],
            Expenses = [CreateExpense("Single Expense", 800m, "USD")]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.StartingBalance.Should().Be(1200m);
        _ = result.Summary.EndingBalance.Should().Be(1200m);
        _ = result.Summary.LowestBalance.Should().Be(1200m);
        _ = result.Summary.HighestBalance.Should().Be(1200m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(1, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(1, 2025));
        _ = result.Summary.TotalIncome.Should().Be(2000m);
        _ = result.Summary.TotalExpenses.Should().Be(800m);
    }

    [Test]
    public async Task Prediction_WithNoIncomesOrExpenses_ShouldHaveZeroTotals()
    {
        // Arrange
        var request = CreateBasicRequest(3, 1000m);

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(0m);
        _ = result.Summary.TotalExpenses.Should().Be(0m);
        _ = result.Summary.StartingBalance.Should().Be(0m);
        _ = result.Summary.EndingBalance.Should().Be(0m);
        _ = result.Summary.LowestBalance.Should().Be(0m);
        _ = result.Summary.HighestBalance.Should().Be(0m);
    }

    [Test]
    public async Task Prediction_WithMultipleMonthsHavingSameBalance_ShouldReturnFirstDate()
    {
        // Arrange - All months will have the same balance
        var request = CreateBasicRequest(5) with
        {
            Incomes = [CreateIncome("Steady Income", 1500m, "USD", frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Steady Expense", 1500m, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert - When multiple months have the same balance, should return the first occurrence
        _ = result.Summary.LowestBalance.Should().Be(0m);
        _ = result.Summary.HighestBalance.Should().Be(0m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(1, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(1, 2025));
    }

    [Test]
    public async Task Prediction_WithLargeNumbers_ShouldCalculateCorrectly()
    {
        // Arrange
        var request = CreateBasicRequest(2, 1_000_000m) with
        {
            Incomes = [CreateIncome("High Income", 500_000m, "USD", frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("High Expense", 300_000m, "USD", frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(1_000_000m);
        _ = result.Summary.TotalExpenses.Should().Be(600_000m);
        _ = result.Summary.StartingBalance.Should().Be(200_000m);
        _ = result.Summary.EndingBalance.Should().Be(200_000m);
        _ = result.Months[0].BudgetAfter.Should().Be(1_200_000m);
        _ = result.Months[1].BudgetAfter.Should().Be(1_400_000m);
    }

    [Test]
    public async Task Prediction_WithDecimalPrecision_ShouldMaintainAccuracy()
    {
        // Arrange
        var request = CreateBasicRequest(1, 100.50m) with
        {
            Incomes = [CreateIncome("Precise Income", 1234.67m, "USD")],
            Expenses = [CreateExpense("Precise Expense", 567.89m, "USD")]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(1234.67m);
        _ = result.Summary.TotalExpenses.Should().Be(567.89m);
        _ = result.Summary.StartingBalance.Should().Be(666.78m);
        _ = result.Summary.EndingBalance.Should().Be(666.78m);
        _ = result.Months[0].BudgetAfter.Should().Be(767.28m);
    }
}