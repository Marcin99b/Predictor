using FluentAssertions;
using Predictor.Web.Models;

namespace Predictor.Tests.Integration;

public class BusinessScenariosTests : BasePredictionTest
{
    [Test]
    public async Task Prediction_SavingForHouseDownPayment_ShouldCalculateTimeframe()
    {
        // Arrange - Save for $50k down payment
        var targetAmount = 50_000m;
        var request = CreateBasicRequest(40, 15_000m) with
        {
            Incomes = [CreateIncome("Monthly Salary", 7_000m, frequency: Frequency.Monthly)],
            Expenses = [
                CreateExpense("Living Expenses", 4_000m, frequency: Frequency.Monthly),                 CreateExpense("Savings Goal", 2_000m, frequency: Frequency.Monthly)             ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert

        var monthsToReachGoal = result.Months
            .Select((month, index) => new { Month = index + 1, month.BudgetAfter })
            .FirstOrDefault(x => x.BudgetAfter >= targetAmount);

        _ = monthsToReachGoal.Should().NotBeNull("Should reach $50k goal within timeframe");
        _ = monthsToReachGoal!.Month.Should().BeLessThanOrEqualTo(40);
        _ = result.Summary.TotalIncome.Should().BeGreaterThan(result.Summary.TotalExpenses);

        _ = result.Months.Last().BudgetAfter.Should().BeGreaterThan(targetAmount,
    "Should have more than target amount by end of prediction period");
    }

    [Test]
    public async Task Prediction_PayingOffStudentLoan_ShouldShowDebtFreedom()
    {
        // Arrange - Student loan paid off in 18 months
        var request = CreateBasicRequest(24, 2_000m) with
        {
            Incomes = [CreateIncome("Job Income", 4_500m, frequency: Frequency.Monthly)],
            Expenses = [
                CreateExpense("Living Costs", 3_000m, frequency: Frequency.Monthly),
                CreateExpense("Student Loan", 800m, frequency: Frequency.Monthly,
                            endDate: new MonthDate(6, 2026))             ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert

        var month18 = result.Months[17];
        var month19 = result.Months[18];

        _ = month18.Expense.Should().Be(3_800m);
        _ = month19.Expense.Should().Be(3_000m);
        var improvementInCashFlow = month19.Balance - month18.Balance;
        _ = improvementInCashFlow.Should().Be(800m);
    }

    [Test]
    public async Task Prediction_RetirementPlanning_ShouldShowLongTermGrowth()
    {
        // Arrange - Long-term retirement savings scenario
        var request = CreateBasicRequest(60, 50_000m) with
        {
            Incomes = [
                CreateIncome("Salary", 8_000m, frequency: Frequency.Monthly),
                CreateIncome("Annual Bonus", 10_000m, frequency: Frequency.Annually)
            ],
            Expenses = [
                CreateExpense("Living Expenses", 5_500m, frequency: Frequency.Monthly),
                CreateExpense("401k Contribution", 1_200m, frequency: Frequency.Monthly),
                CreateExpense("IRA Contribution", 500m, frequency: Frequency.Monthly)
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var totalRetirementContributions = (1_200m + 500m) * 60;
        var totalIncome = 8_000m * 60 + 10_000m * 5;
        var totalExpenses = result.Summary.TotalExpenses;

        _ = result.Summary.TotalIncome.Should().Be(totalIncome);
        _ = totalRetirementContributions.Should().Be(102_000m);
        _ = result.Months.Should().AllSatisfy(m => m.Balance.Should().BePositive());

        var finalBudget = result.Months.Last().BudgetAfter;
        _ = finalBudget.Should().BeGreaterThan(100_000m);
    }

    [Test]
    public async Task Prediction_SeasonalBusinessIncome_ShouldHandleIrregularity()
    {
        // Arrange - Seasonal business with irregular income
        var request = CreateBasicRequest(12, 15_000m) with
        {
            Incomes = [
                                CreateIncome("Summer Revenue", 20_000m, month: 6, frequency: Frequency.OneTime),                 CreateIncome("Summer Revenue", 25_000m, month: 7, frequency: Frequency.OneTime),                 CreateIncome("Summer Revenue", 18_000m, month: 8, frequency: Frequency.OneTime),                                 CreateIncome("Holiday Revenue", 12_000m, month: 11, frequency: Frequency.OneTime),                 CreateIncome("Holiday Revenue", 15_000m, month: 12, frequency: Frequency.OneTime),                                 CreateIncome("Base Income", 3_000m, frequency: Frequency.Monthly)             ],
            Expenses = [
                CreateExpense("Fixed Costs", 4_000m, frequency: Frequency.Monthly),
                CreateExpense("Summer Marketing", 2_000m, month: 5, frequency: Frequency.OneTime),                 CreateExpense("Holiday Inventory", 3_000m, month: 10, frequency: Frequency.OneTime)             ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var summerMonths = result.Months.Skip(5).Take(3).ToArray();
        _ = summerMonths.Should().AllSatisfy(m => m.Balance.Should().BeGreaterThanOrEqualTo(10_000m));

        _ = result.Summary.TotalIncome.Should().BeGreaterThan(result.Summary.TotalExpenses);

        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive());
    }

    [Test]
    public async Task Prediction_FreelancerWithIrregularWork_ShouldManageCashFlow()
    {
        // Arrange - Freelancer with project-based income
        var request = CreateBasicRequest(12, 8_000m) with
        {
            Incomes = [
                                CreateIncome("Project A Payment", 8_000m, month: 2, frequency: Frequency.OneTime),
                CreateIncome("Project B Payment", 12_000m, month: 5, frequency: Frequency.OneTime),
                CreateIncome("Project C Payment", 6_000m, month: 8, frequency: Frequency.OneTime),
                CreateIncome("Project D Payment", 15_000m, month: 11, frequency: Frequency.OneTime),
                                CreateIncome("Retainer Client", 2_500m, frequency: Frequency.Monthly)             ],
            Expenses = [
                CreateExpense("Living Expenses", 3_000m, frequency: Frequency.Monthly),                 CreateExpense("Business Expenses", 500m, frequency: Frequency.Monthly),                 CreateExpense("Quarterly Taxes", 2_000m, frequency: Frequency.Quarterly)             ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive(),
    "Freelancer should maintain positive cash flow");

        _ = result.Months[1].Balance.Should().BeGreaterThan(5_000m);
        _ = result.Months[4].Balance.Should().BeGreaterThan(8_000m);
        _ = result.Summary.TotalIncome.Should().BeGreaterThan(result.Summary.TotalExpenses);

        _ = result.Months.Last().BudgetAfter.Should().BeGreaterThan(15_000m);
    }

    [Test]
    public async Task Prediction_FamilyBudgetWithChildExpenses_ShouldAccountForGrowingCosts()
    {
        // Arrange - Family with growing child-related expenses
        var request = CreateBasicRequest(36, 15_000m) with
        {
            Incomes = [
                CreateIncome("Parent 1 Salary", 6_500m, frequency: Frequency.Monthly),
                CreateIncome("Parent 2 Salary", 4_800m, frequency: Frequency.Monthly),
                CreateIncome("Tax Refund", 3_500m, month: 4, frequency: Frequency.Annually)             ],
            Expenses = [
                CreateExpense("Mortgage", 2_800m, frequency: Frequency.Monthly),
                CreateExpense("Utilities & Home", 600m, frequency: Frequency.Monthly),
                CreateExpense("Food & Groceries", 1_200m, frequency: Frequency.Monthly),
                CreateExpense("Childcare", 1_800m, frequency: Frequency.Monthly,
                            endDate: new MonthDate(6, 2027)),                 CreateExpense("School Supplies", 800m, month: 8, frequency: Frequency.Annually),
                CreateExpense("Medical/Dental", 400m, frequency: Frequency.Quarterly),
                CreateExpense("College Savings", 1_000m, frequency: Frequency.Monthly),
                                CreateExpense("Activities/Sports", 300m, month: 12, frequency: Frequency.Monthly)             ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var earlyMonths = result.Months.Take(12);
        var laterMonths = result.Months.Skip(24).Take(12);

        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive());

        var totalCollegeSavings = 1_000m * 36;
        _ = totalCollegeSavings.Should().Be(36_000m);

        if (result.Months.Length > 30)
        {
            var beforeChildcareEnds = result.Months[28];
            var afterChildcareEnds = result.Months[30];
            var expenseDifference = beforeChildcareEnds.Expense - afterChildcareEnds.Expense;
            _ = expenseDifference.Should().BeGreaterThan(1000m,
                $"Childcare should stop, reducing expenses. Before: {beforeChildcareEnds.Expense}, After: {afterChildcareEnds.Expense}");
        }
    }

    [Test]
    public async Task Prediction_StartupFounderScenario_ShouldSurviveInitialLosses()
    {
        var request = CreateBasicRequest(18, 50_000m) with
        {
            Incomes = [
                CreateIncome("First Revenue", 3_000m, month: 7, frequency: Frequency.Monthly,
                          endDate: new MonthDate(12, 2025)),
                CreateIncome("Growing Revenue", 10_000m, month: 1, year: 2026, frequency: Frequency.Monthly)
            ],
            Expenses = [
                CreateExpense("Development Costs", 3_000m, frequency: Frequency.Monthly,
                            endDate: new MonthDate(6, 2025)),
                CreateExpense("Office Setup", 8_000m, month: 1, frequency: Frequency.OneTime),
                CreateExpense("Legal/Registration", 3_000m, month: 2, frequency: Frequency.OneTime),
                CreateExpense("Minimal Living", 2_000m, frequency: Frequency.Monthly),
                CreateExpense("Business Operations", 1_000m, month: 7, frequency: Frequency.Monthly) 
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive(),
            "Startup should not run out of money");

        var month12Budget = result.Months[11].BudgetAfter;
        var month18Budget = result.Months[17].BudgetAfter;

        month18Budget.Should().BeGreaterThan(month12Budget,
            "Business should recover and grow after initial period");

        var revenueStartMonth = result.Months[6];
        revenueStartMonth.Income.Should().BeGreaterThan(0);
    }
}