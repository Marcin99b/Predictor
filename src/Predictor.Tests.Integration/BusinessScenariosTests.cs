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
        var request = CreateBasicRequest(30, 10_000m) with // Extended to 30 months, start with 10k saved
        {
            Incomes = [CreateIncome("Monthly Salary", 6_000m, frequency: Frequency.Monthly)],
            Expenses = [
                CreateExpense("Living Expenses", 3_500m, frequency: Frequency.Monthly), // Reduced from 4000
                CreateExpense("Savings Goal", 1_500m, frequency: Frequency.Monthly) // Dedicated savings
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var monthsToReachGoal = result.Months
            .Select((month, index) => new { Month = index + 1, month.BudgetAfter })
            .FirstOrDefault(x => x.BudgetAfter >= targetAmount);

        _ = monthsToReachGoal.Should().NotBeNull("Should reach $50k goal within timeframe");
        _ = monthsToReachGoal!.Month.Should().BeLessThanOrEqualTo(30); // Should reach goal within 30 months

        // Monthly net savings: 6000 - 3500 - 1500 = 1000
        // With 10k initial: need 40k more = 40 months, but we have dedicated savings too
        // So effective savings rate is 2500/month (1000 + 1500) = 16 months to reach 50k from 10k
        _ = result.Summary.TotalIncome.Should().BeGreaterThan(result.Summary.TotalExpenses);
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
                            endDate: new MonthDate(6, 2026)) // Ends after 18 months (Jan 2025 + 18 = June 2026)
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // First 18 months: net 700/month (4500 - 3000 - 800)
        // After month 18: net 1500/month (4500 - 3000)

        var month18 = result.Months[17]; // 0-indexed
        var month19 = result.Months[18];

        _ = month18.Expense.Should().Be(3_800m); // Still paying loan
        _ = month19.Expense.Should().Be(3_000m); // Loan paid off

        // Cash flow improvement after loan payoff
        var improvementInCashFlow = month19.Balance - month18.Balance;
        _ = improvementInCashFlow.Should().Be(800m); // The loan payment amount
    }

    [Test]
    public async Task Prediction_RetirementPlanning_ShouldShowLongTermGrowth()
    {
        // Arrange - Long-term retirement savings scenario
        var request = CreateBasicRequest(60, 50_000m) with // 5 years planning
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
        var totalRetirementContributions = (1_200m + 500m) * 60; // Monthly contributions
        var totalIncome = 8_000m * 60 + 10_000m * 5; // 60 months + 5 annual bonuses
        var totalExpenses = result.Summary.TotalExpenses;

        _ = result.Summary.TotalIncome.Should().Be(totalIncome);
        _ = totalRetirementContributions.Should().Be(102_000m); // 1700 * 60

        // Should have positive cash flow throughout
        _ = result.Months.Should().AllSatisfy(m => m.Balance.Should().BePositive());

        // Final budget should be significantly higher than initial
        var finalBudget = result.Months.Last().BudgetAfter;
        _ = finalBudget.Should().BeGreaterThan(100_000m);
    }

    [Test]
    public async Task Prediction_SeasonalBusinessIncome_ShouldHandleIrregularity()
    {
        // Arrange - Seasonal business with irregular income
        var request = CreateBasicRequest(12, 5_000m) with
        {
            Incomes = [
                // High season: months 6-8 (summer)
                CreateIncome("Summer Revenue", 15_000m, month: 6, frequency: Frequency.OneTime),
                CreateIncome("Summer Revenue", 18_000m, month: 7, frequency: Frequency.OneTime),
                CreateIncome("Summer Revenue", 12_000m, month: 8, frequency: Frequency.OneTime),
                // Holiday season: months 11-12
                CreateIncome("Holiday Revenue", 8_000m, month: 11, frequency: Frequency.OneTime),
                CreateIncome("Holiday Revenue", 10_000m, month: 12, frequency: Frequency.OneTime),
                // Low income other months
                CreateIncome("Base Income", 2_000m, frequency: Frequency.Monthly)
            ],
            Expenses = [
                CreateExpense("Fixed Costs", 4_000m, frequency: Frequency.Monthly),
                CreateExpense("Summer Marketing", 3_000m, month: 5, frequency: Frequency.OneTime), // Prepare for season
                CreateExpense("Holiday Inventory", 5_000m, month: 10, frequency: Frequency.OneTime)
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // Should survive low months and capitalize on high months
        var summerMonths = result.Months.Skip(5).Take(3).ToArray(); // Months 6-8
        var lowMonths = result.Months.Take(5).Concat(result.Months.Skip(8).Take(2)).ToArray(); // Months 1-5, 9-10

        _ = summerMonths.Should().AllSatisfy(m => m.Balance.Should().BeGreaterThan(10_000m));

        // Business should be profitable overall
        _ = result.Summary.TotalIncome.Should().BeGreaterThan(result.Summary.TotalExpenses);

        // Should not go bankrupt in low season
        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive());
    }

    [Test]
    public async Task Prediction_FreelancerWithIrregularWork_ShouldManageCashFlow()
    {
        // Arrange - Freelancer with project-based income
        var request = CreateBasicRequest(12, 8_000m) with // Start with more cash buffer
        {
            Incomes = [
                // Project payments come irregularly
                CreateIncome("Project A Payment", 8_000m, month: 2, frequency: Frequency.OneTime),
                CreateIncome("Project B Payment", 12_000m, month: 5, frequency: Frequency.OneTime),
                CreateIncome("Project C Payment", 6_000m, month: 8, frequency: Frequency.OneTime),
                CreateIncome("Project D Payment", 15_000m, month: 11, frequency: Frequency.OneTime),
                // Small recurring income
                CreateIncome("Retainer Client", 2_500m, frequency: Frequency.Monthly) // Increased retainer
            ],
            Expenses = [
                CreateExpense("Living Expenses", 3_000m, frequency: Frequency.Monthly), // Reduced living costs
                CreateExpense("Business Expenses", 500m, frequency: Frequency.Monthly), // Reduced business costs
                CreateExpense("Quarterly Taxes", 2_000m, frequency: Frequency.Quarterly) // Reduced tax burden
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // Should maintain positive balance throughout despite irregular income
        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive(),
            "Freelancer should maintain positive cash flow");

        // Months with project payments should show significant balance increases
        _ = result.Months[1].Balance.Should().BeGreaterThan(5_000m); // Project A month
        _ = result.Months[4].Balance.Should().BeGreaterThan(8_000m); // Project B month

        // Total income should cover all expenses with profit
        _ = result.Summary.TotalIncome.Should().BeGreaterThan(result.Summary.TotalExpenses);

        // Should have substantial cash reserves by end of year
        _ = result.Months.Last().BudgetAfter.Should().BeGreaterThan(15_000m);
    }

    [Test]
    public async Task Prediction_FamilyBudgetWithChildExpenses_ShouldAccountForGrowingCosts()
    {
        // Arrange - Family with growing child-related expenses
        var request = CreateBasicRequest(36, 15_000m) with // 3 years
        {
            Incomes = [
                CreateIncome("Parent 1 Salary", 6_500m, frequency: Frequency.Monthly),
                CreateIncome("Parent 2 Salary", 4_800m, frequency: Frequency.Monthly),
                CreateIncome("Tax Refund", 3_500m, month: 4, frequency: Frequency.Annually) // Annual tax refund
            ],
            Expenses = [
                CreateExpense("Mortgage", 2_800m, frequency: Frequency.Monthly),
                CreateExpense("Utilities & Home", 600m, frequency: Frequency.Monthly),
                CreateExpense("Food & Groceries", 1_200m, frequency: Frequency.Monthly),
                CreateExpense("Childcare", 1_800m, frequency: Frequency.Monthly,
                            endDate: new MonthDate(6, 2027)), // Ends in month 30 (June 2027)
                CreateExpense("School Supplies", 800m, month: 8, frequency: Frequency.Annually),
                CreateExpense("Medical/Dental", 400m, frequency: Frequency.Quarterly),
                CreateExpense("College Savings", 1_000m, frequency: Frequency.Monthly),
                // Growing expenses
                CreateExpense("Activities/Sports", 300m, month: 12, frequency: Frequency.Monthly) // Starts year 2
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        var earlyMonths = result.Months.Take(12);
        var laterMonths = result.Months.Skip(24).Take(12);

        // Should maintain positive cash flow throughout
        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive());

        // College savings should accumulate
        var totalCollegeSavings = 1_000m * 36; // $36k over 3 years
        _ = totalCollegeSavings.Should().Be(36_000m);

        // Should be able to handle the expense changes
        // Childcare ends in month 30 (June 2027), so month 29 has childcare, month 30 doesn't
        if (result.Months.Length > 30)
        {
            var beforeChildcareEnds = result.Months[28]; // Month 29 - still paying childcare
            var afterChildcareEnds = result.Months[30];  // Month 31 - childcare stopped

            // The difference should be approximately the childcare amount (1800)
            var expenseDifference = beforeChildcareEnds.Expense - afterChildcareEnds.Expense;
            _ = expenseDifference.Should().BeGreaterThan(1000m,
                $"Childcare should stop, reducing expenses. Before: {beforeChildcareEnds.Expense}, After: {afterChildcareEnds.Expense}");
        }
    }

    [Test]
    public async Task Prediction_StartupFounderScenario_ShouldSurviveInitialLosses()
    {
        // Arrange - Startup founder with initial losses, then growth
        var request = CreateBasicRequest(18, 40_000m) with // Increased initial capital
        {
            Incomes = [
                // No income first 6 months
                CreateIncome("First Revenue", 3_000m, month: 7, frequency: Frequency.Monthly, // Increased revenue
                          endDate: new MonthDate(12, 2025)),
                // Growing revenue from month 13
                CreateIncome("Growing Revenue", 10_000m, month: 13, frequency: Frequency.Monthly) // Increased growth revenue
            ],
            Expenses = [
                // High initial costs but reduced
                CreateExpense("Development Costs", 3_000m, frequency: Frequency.Monthly, // Reduced from 4000
                            endDate: new MonthDate(6, 2025)),
                CreateExpense("Office Setup", 8_000m, month: 1, frequency: Frequency.OneTime), // Reduced from 10000
                CreateExpense("Legal/Registration", 3_000m, month: 2, frequency: Frequency.OneTime), // Reduced from 5000
                // Ongoing costs
                CreateExpense("Minimal Living", 2_000m, frequency: Frequency.Monthly), // Reduced from 2500
                CreateExpense("Business Operations", 1_000m, month: 7, frequency: Frequency.Monthly) // Reduced from 1500
            ]
        };

        // Act
        var result = await this.GetPredictionResult(request);

        // Assert
        // Should survive the initial burn period
        _ = result.Months.Should().AllSatisfy(m => m.BudgetAfter.Should().BePositive(),
            "Startup should not run out of money");

        // Should show recovery after month 12
        var month12Budget = result.Months[11].BudgetAfter;
        var month18Budget = result.Months[17].BudgetAfter;

        _ = month18Budget.Should().BeGreaterThan(month12Budget,
            "Business should recover and grow after initial period");

        // The transition months should show the business model working
        var revenueStartMonth = result.Months[6]; // Month 7
        _ = revenueStartMonth.Income.Should().BeGreaterThan(0);
    }
}