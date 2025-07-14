using System.Net;
using FluentAssertions;
using Predictor.Web.Models;

namespace Predictor.Tests.Integration;

public class ValidationTests : BasePredictionTest
{
    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(-1, HttpStatusCode.BadRequest)]
    [TestCase(121, HttpStatusCode.BadRequest)] // More than 10 years
    [TestCase(1, HttpStatusCode.OK)]
    [TestCase(120, HttpStatusCode.OK)] // Exactly 10 years
    public async Task Prediction_WithPredictionMonths_ShouldValidateCorrectly(int months, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest(months);

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(-1000, HttpStatusCode.BadRequest)]
    [TestCase(-0.01, HttpStatusCode.BadRequest)]
    [TestCase(0, HttpStatusCode.OK)]
    [TestCase(1000, HttpStatusCode.OK)]
    public async Task Prediction_WithInitialBudget_ShouldValidateCorrectly(decimal budget, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest(initialBudget: budget);

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(0, HttpStatusCode.BadRequest)] // Invalid month
    [TestCase(13, HttpStatusCode.BadRequest)] // Invalid month
    [TestCase(-1, HttpStatusCode.BadRequest)] // Invalid month
    [TestCase(1, HttpStatusCode.OK)]
    [TestCase(12, HttpStatusCode.OK)]
    public async Task Prediction_WithStartMonthDate_ShouldValidateCorrectly(int month, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            StartPredictionMonth = new MonthDate(month, 2025)
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(1899, HttpStatusCode.BadRequest)] // Too early year
    [TestCase(3000, HttpStatusCode.BadRequest)] // Too late year
    [TestCase(1900, HttpStatusCode.OK)]
    [TestCase(2999, HttpStatusCode.OK)]
    [TestCase(2025, HttpStatusCode.OK)]
    public async Task Prediction_WithStartYearDate_ShouldValidateCorrectly(int year, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            StartPredictionMonth = new MonthDate(1, year)
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase("", HttpStatusCode.BadRequest)] // Empty name
    [TestCase("ab", HttpStatusCode.BadRequest)] // Too short (less than 3)
    [TestCase("abc", HttpStatusCode.OK)] // Minimum valid length
    [TestCase("Valid Payment Name", HttpStatusCode.OK)]
    public async Task Prediction_WithIncomeItemName_ShouldValidateCorrectly(string name, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome(name, 1000m)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase("", HttpStatusCode.BadRequest)] // Empty name
    [TestCase("ab", HttpStatusCode.BadRequest)] // Too short
    [TestCase("abc", HttpStatusCode.OK)] // Minimum valid length
    public async Task Prediction_WithExpenseItemName_ShouldValidateCorrectly(string name, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Expenses = [CreateExpense(name, 1000m)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(-1000, HttpStatusCode.BadRequest)]
    [TestCase(-0.01, HttpStatusCode.BadRequest)]
    [TestCase(0.01, HttpStatusCode.OK)]
    [TestCase(1000, HttpStatusCode.OK)]
    public async Task Prediction_WithIncomeItemValue_ShouldValidateCorrectly(decimal value, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Valid Name", value)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(-1000, HttpStatusCode.BadRequest)]
    [TestCase(0.01, HttpStatusCode.OK)]
    [TestCase(1000, HttpStatusCode.OK)]
    public async Task Prediction_WithExpenseItemValue_ShouldValidateCorrectly(decimal value, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Expenses = [CreateExpense("Valid Name", value)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(0, HttpStatusCode.BadRequest)] // Invalid month in payment item
    [TestCase(13, HttpStatusCode.BadRequest)] // Invalid month in payment item
    [TestCase(1, HttpStatusCode.OK)]
    [TestCase(12, HttpStatusCode.OK)]
    public async Task Prediction_WithPaymentItemStartDate_ShouldValidateCorrectly(int month, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Valid Name", 1000m, month)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(1899, HttpStatusCode.BadRequest)] // Invalid year in payment item
    [TestCase(3000, HttpStatusCode.BadRequest)] // Invalid year in payment item
    [TestCase(1900, HttpStatusCode.OK)]
    [TestCase(2999, HttpStatusCode.OK)]
    public async Task Prediction_WithPaymentItemStartYear_ShouldValidateCorrectly(int year, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Valid Name", 1000m, year: year)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [Test]
    public async Task Prediction_WithValidEndDate_ShouldReturnOk()
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Contract", 1000m, endDate: new MonthDate(12, 2025))]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.OK);
    }

    [TestCase(0, HttpStatusCode.BadRequest)] // Invalid month in end date
    [TestCase(13, HttpStatusCode.BadRequest)] // Invalid month in end date
    public async Task Prediction_WithInvalidEndDate_ShouldReturnBadRequest(int month, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Contract", 1000m, endDate: new MonthDate(month, 2025))]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [Test]
    public async Task Prediction_WithEmptyIncomesAndExpenses_ShouldReturnOk()
    {
        // Arrange
        var request = CreateBasicRequest();

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Prediction_WithVeryLongPaymentName_ShouldReturnBadRequest()
    {
        // Arrange
        var longName = new string('a', 101); // More than 100 characters
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome(longName, 1000m)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Prediction_WithMaxValidPaymentName_ShouldReturnOk()
    {
        // Arrange
        var maxName = new string('a', 100); // Exactly 100 characters
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome(maxName, 1000m)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.OK);
    }
}