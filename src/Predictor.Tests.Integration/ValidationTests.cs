using FluentAssertions;
using Predictor.Web.Models;
using System.Net;

namespace Predictor.Tests.Integration;

public class ValidationTests : BasePredictionTest
{
    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(-1, HttpStatusCode.BadRequest)]
    [TestCase(121, HttpStatusCode.BadRequest)]
    [TestCase(1, HttpStatusCode.OK)]
    [TestCase(120, HttpStatusCode.OK)]
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

    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(13, HttpStatusCode.BadRequest)]
    [TestCase(-1, HttpStatusCode.BadRequest)]
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

    [TestCase(1899, HttpStatusCode.BadRequest)]
    [TestCase(3000, HttpStatusCode.BadRequest)]
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

    [TestCase("", HttpStatusCode.BadRequest)]
    [TestCase("ab", HttpStatusCode.BadRequest)]
    [TestCase("abc", HttpStatusCode.OK)]
    [TestCase("Valid Payment Name", HttpStatusCode.OK)]
    public async Task Prediction_WithIncomeItemName_ShouldValidateCorrectly(string name, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome(name, 1000m, "USD")]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase("", HttpStatusCode.BadRequest)]
    [TestCase("ab", HttpStatusCode.BadRequest)]
    [TestCase("abc", HttpStatusCode.OK)]
    public async Task Prediction_WithExpenseItemName_ShouldValidateCorrectly(string name, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Expenses = [CreateExpense(name, 1000m, "USD")]
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
            Incomes = [CreateIncome("Valid Name", value, "USD")]
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
            Expenses = [CreateExpense("Valid Name", value, "USD")]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(13, HttpStatusCode.BadRequest)]
    [TestCase(1, HttpStatusCode.OK)]
    [TestCase(12, HttpStatusCode.OK)]
    public async Task Prediction_WithPaymentItemStartDate_ShouldValidateCorrectly(int month, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Valid Name", 1000m, "USD", month)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }

    [TestCase(1899, HttpStatusCode.BadRequest)]
    [TestCase(3000, HttpStatusCode.BadRequest)]
    [TestCase(1900, HttpStatusCode.OK)]
    [TestCase(2999, HttpStatusCode.OK)]
    public async Task Prediction_WithPaymentItemStartYear_ShouldValidateCorrectly(int year, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Valid Name", 1000m, "USD", year: year)]
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
            Incomes = [CreateIncome("Contract", 1000m, "USD", endDate: new MonthDate(12, 2025))]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.OK);
    }

    [TestCase(0, HttpStatusCode.BadRequest)]
    [TestCase(13, HttpStatusCode.BadRequest)]
    public async Task Prediction_WithInvalidEndDate_ShouldReturnBadRequest(int month, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Contract", 1000m, "USD", endDate: new MonthDate(month, 2025))]
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
        var longName = new string('a', 101);
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome(longName, 1000m, "USD")]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Prediction_WithMaxValidPaymentName_ShouldReturnOk()
    {
        // Arrange
        var maxName = new string('a', 100);
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome(maxName, 1000m, "USD")]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(HttpStatusCode.OK);
    }

    [TestCase("", HttpStatusCode.BadRequest)]
    [TestCase("  ", HttpStatusCode.BadRequest)]
    [TestCase("XYZ", HttpStatusCode.BadRequest)] // Not a real ISO code
    [TestCase("usd", HttpStatusCode.OK)]         // Lowercase, still valid
    [TestCase("USD", HttpStatusCode.OK)]         // Valid
    [TestCase("EUR", HttpStatusCode.OK)]         // Valid
    [TestCase("GBP", HttpStatusCode.OK)]         // Valid
    [TestCase("US D", HttpStatusCode.BadRequest)] // Invalid with space
    public async Task Prediction_WithCurrencyCode_ShouldValidateCorrectly(string currency, HttpStatusCode expectedStatus)
    {
        // Arrange
        var request = CreateBasicRequest() with
        {
            Incomes = [CreateIncome("Salary", 1000m, currency)]
        };

        // Act & Assert
        var status = await this.GetResponseStatusCode(request);
        _ = status.Should().Be(expectedStatus);
    }
}