using FluentAssertions;
using Predictor.Web.Models;

namespace Predictor.Tests.Integration;

public class PaymentFrequencyTests : BasePredictionTest
{
    [TestCase(Frequency.Monthly, 4, new[] { 0, 1, 2, 3 })] // Every month
    [TestCase(Frequency.Quarterly, 7, new[] { 0, 3, 6 })] // Months 1, 4, 7
    [TestCase(Frequency.SemiAnnually, 13, new[] { 0, 6, 12 })] // Months 1, 7, 13
    [TestCase(Frequency.Annually, 25, new[] { 0, 12, 24 })] // Months 1, 13, 25
    public async Task Prediction_WithRecurringFrequency_ShouldOccurAtCorrectIntervals(
        Frequency frequency, int totalMonths, int[] expectedMonthIndexes)
    {
        // Arrange
        var request = CreateBasicRequest(totalMonths) with
        {
            Incomes = [CreateIncome("Recurring Income", 1000m, frequency: frequency)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        for (var i = 0; i < totalMonths; i++)
        {
            var expectedIncome = expectedMonthIndexes.Contains(i) ? 1000m : 0m;
            _ = result.Months[i].Income.Should().Be(expectedIncome, $"Month {i + 1} should have income {expectedIncome}");
        }

        var expectedTotal = expectedMonthIndexes.Length * 1000m;
        _ = result.Summary.TotalIncome.Should().Be(expectedTotal);
    }

    [TestCase(3, new[] { 0, 0, 1000 })] // One-time in month 3
    [TestCase(1, new[] { 1000 })] // One-time in month 1
    [TestCase(5, new[] { 0, 0, 0, 0, 1000 })] // One-time in month 5
    public async Task Prediction_WithOneTimeFrequency_ShouldOccurOnlyOnce(
        int targetMonth, decimal[] expectedIncomes)
    {
        // Arrange
        var request = CreateBasicRequest(expectedIncomes.Length) with
        {
            Incomes = [CreateIncome("One-time Bonus", 1000m, targetMonth, frequency: Frequency.OneTime)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months.Select(m => m.Income).Should().Equal(expectedIncomes);
        _ = result.Summary.TotalIncome.Should().Be(1000m);
    }

    [Test]
    public async Task Prediction_WithEndDate_ShouldStopAfterEndDate()
    {
        // Arrange
        var endDate = new MonthDate(3, 2025);
        var request = CreateBasicRequest(6) with
        {
            Incomes = [CreateIncome("Contract", 1000m, frequency: Frequency.Monthly, endDate: endDate)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var expectedIncomes = new[] { 1000m, 1000m, 1000m, 0m, 0m, 0m };
        _ = result.Months.Select(m => m.Income).Should().Equal(expectedIncomes);
        _ = result.Summary.TotalIncome.Should().Be(3000m);
    }

    [Test]
    public async Task Prediction_WithEndDateBeforeStart_ShouldNotOccur()
    {
        // Arrange
        var endDate = new MonthDate(12, 2024); // Before start date
        var request = CreateBasicRequest(3) with
        {
            Incomes = [CreateIncome("Expired Contract", 1000m, frequency: Frequency.Monthly, endDate: endDate)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months.Should().AllSatisfy(m => m.Income.Should().Be(0m));
        _ = result.Summary.TotalIncome.Should().Be(0m);
    }

    [Test]
    public async Task Prediction_WithLateStartDate_ShouldStartFromCorrectMonth()
    {
        // Arrange
        var request = CreateBasicRequest(5) with
        {
            Incomes = [CreateIncome("Late Start", 1000m, month: 3, frequency: Frequency.Monthly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var expectedIncomes = new[] { 0m, 0m, 1000m, 1000m, 1000m };
        _ = result.Months.Select(m => m.Income).Should().Equal(expectedIncomes);
        _ = result.Summary.TotalIncome.Should().Be(3000m);
    }

    [Test]
    public async Task Prediction_WithQuarterlyStartingInMiddle_ShouldCalculateCorrectly()
    {
        // Arrange - Start quarterly payment in month 2, should occur in months 2, 5, 8
        var request = CreateBasicRequest(9) with
        {
            Incomes = [CreateIncome("Quarterly Mid-Start", 3000m, month: 2, frequency: Frequency.Quarterly)]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months[0].Income.Should().Be(0m);    // Month 1
        _ = result.Months[1].Income.Should().Be(3000m); // Month 2
        _ = result.Months[2].Income.Should().Be(0m);    // Month 3
        _ = result.Months[3].Income.Should().Be(0m);    // Month 4
        _ = result.Months[4].Income.Should().Be(3000m); // Month 5
        _ = result.Months[5].Income.Should().Be(0m);    // Month 6
        _ = result.Months[6].Income.Should().Be(0m);    // Month 7
        _ = result.Months[7].Income.Should().Be(3000m); // Month 8
        _ = result.Months[8].Income.Should().Be(0m);    // Month 9

        _ = result.Summary.TotalIncome.Should().Be(9000m);
    }

    [Test]
    public async Task Prediction_WithMixedFrequencies_ShouldCalculateAllCorrectly()
    {
        // Arrange
        var request = CreateBasicRequest(12) with
        {
            Incomes = [
                CreateIncome("Monthly Salary", 3000m, frequency: Frequency.Monthly),
                CreateIncome("Quarterly Bonus", 2000m, frequency: Frequency.Quarterly),
                CreateIncome("Annual Bonus", 10000m, frequency: Frequency.Annually),
                CreateIncome("One-time Gift", 5000m, month: 6, frequency: Frequency.OneTime)
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // Monthly: 12 * 3000 = 36000
        // Quarterly: 4 * 2000 = 8000 (months 1, 4, 7, 10)
        // Annual: 1 * 10000 = 10000 (month 1)
        // One-time: 1 * 5000 = 5000 (month 6)
        // Total: 59000

        _ = result.Summary.TotalIncome.Should().Be(59000m);

        // Check specific months
        _ = result.Months[0].Income.Should().Be(15000m); // Month 1: 3000 + 2000 + 10000
        _ = result.Months[5].Income.Should().Be(8000m);  // Month 6: 3000 + 5000
        _ = result.Months[11].Income.Should().Be(3000m); // Month 12: 3000 only
    }
}