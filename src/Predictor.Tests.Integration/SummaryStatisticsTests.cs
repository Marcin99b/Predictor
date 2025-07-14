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
                CreateIncome("Regular Salary", 3000m, frequency: Frequency.Monthly),
                CreateIncome("Big Bonus", 5000m, month: 2, frequency: Frequency.OneTime)
            ],
            Expenses = [
                CreateExpense("Big Purchase", 4000m, frequency: Frequency.OneTime),
                CreateExpense("Regular Expense", 1000m, frequency: Frequency.Monthly)
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(14000m); // 3*3000 + 5000
        _ = result.Summary.TotalExpenses.Should().Be(7000m); // 4000 + 3*1000
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
            Incomes = [CreateIncome("Income", 1000m, frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Expense", 1000m, frequency: Frequency.Monthly)]
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
            Incomes = [CreateIncome("Growing Income", 2000m, frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Fixed Expense", 1000m, frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.LowestBalance.Should().Be(1000m); // Always positive and growing
        _ = result.Summary.HighestBalance.Should().Be(1000m); // Same balance each month
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
            Expenses = [CreateExpense("Heavy Expense", 1500m, frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // Month 1: Balance -1500, Budget 3500
        // Month 2: Balance -1500, Budget 2000
        // Month 3: Balance -1500, Budget 500

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
                CreateIncome("Regular Income", 1000m, frequency: Frequency.Monthly),
                CreateIncome("Bonus Month 1", 1000m, month: 1, frequency: Frequency.OneTime),
                CreateIncome("Big Bonus Month 3", 3000m, month: 3, frequency: Frequency.OneTime)
            ],
            Expenses = [
                CreateExpense("Big Expense Month 2", 4000m, month: 2, frequency: Frequency.OneTime),
                CreateExpense("Regular Expense", 0m, frequency: Frequency.Monthly) // No regular expenses
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // Month 1: Income 2000, Expense 0, Balance +2000
        // Month 2: Income 1000, Expense 4000, Balance -3000 (LOWEST)
        // Month 3: Income 4000, Expense 0, Balance +4000 (HIGHEST)
        // Month 4: Income 1000, Expense 0, Balance +1000

        _ = result.Summary.LowestBalance.Should().Be(-3000m);
        _ = result.Summary.HighestBalance.Should().Be(4000m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(2, 2025));
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(3, 2025));
    }

    [Test]
    public async Task Prediction_WithOnlyOneMonth_ShouldHaveSameStartAndEnd()
    {
        // Arrange
        var request = CreateBasicRequest(1, 5000m) with
        {
            Incomes = [CreateIncome("Single Income", 2000m)],
            Expenses = [CreateExpense("Single Expense", 800m)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.StartingBalance.Should().Be(1200m); // 2000 - 800
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
            Incomes = [CreateIncome("Steady Income", 1500m, frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("Steady Expense", 1500m, frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert - When multiple months have the same balance, should return the first occurrence
        _ = result.Summary.LowestBalance.Should().Be(0m);
        _ = result.Summary.HighestBalance.Should().Be(0m);
        _ = result.Summary.LowestBalanceDate.Should().Be(new MonthDate(1, 2025)); // First month
        _ = result.Summary.HighestBalanceDate.Should().Be(new MonthDate(1, 2025)); // First month
    }

    [Test]
    public async Task Prediction_WithLargeNumbers_ShouldCalculateCorrectly()
    {
        // Arrange
        var request = CreateBasicRequest(2, 1_000_000m) with
        {
            Incomes = [CreateIncome("High Income", 500_000m, frequency: Frequency.Monthly)],
            Expenses = [CreateExpense("High Expense", 300_000m, frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(1_000_000m);
        _ = result.Summary.TotalExpenses.Should().Be(600_000m);
        _ = result.Summary.StartingBalance.Should().Be(200_000m); // 500k - 300k
        _ = result.Summary.EndingBalance.Should().Be(200_000m);
        _ = result.Months[0].BudgetAfter.Should().Be(1_200_000m); // 1M + 200k
        _ = result.Months[1].BudgetAfter.Should().Be(1_400_000m); // 1.2M + 200k
    }

    [Test]
    public async Task Prediction_WithDecimalPrecision_ShouldMaintainAccuracy()
    {
        // Arrange
        var request = CreateBasicRequest(1, 100.50m) with
        {
            Incomes = [CreateIncome("Precise Income", 1234.67m)],
            Expenses = [CreateExpense("Precise Expense", 567.89m)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Summary.TotalIncome.Should().Be(1234.67m);
        _ = result.Summary.TotalExpenses.Should().Be(567.89m);
        _ = result.Summary.StartingBalance.Should().Be(666.78m); // 1234.67 - 567.89
        _ = result.Summary.EndingBalance.Should().Be(666.78m);
        _ = result.Months[0].BudgetAfter.Should().Be(767.28m); // 100.50 + 666.78
    }
}