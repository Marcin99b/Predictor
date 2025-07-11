namespace Predictor.Web;

public static class ExampleData
{
    private static readonly CalculateInput calculateInputExample = new(
        35_000,
        Incomes: [
            new("Primary Salary", 4_200, true, MonthDate.Now),
            new("Side Consulting", 800, true, MonthDate.Now),
            new("Investment Dividends", 150, true, MonthDate.Now),
            new("Rental Property", 1_200, true, MonthDate.Now),
            new("Freelance Project", 2_500, false, MonthDate.Now.AddMonths(1)),
            new("Tax Refund", 1_800, false, MonthDate.Now.AddMonths(3)),
            new("Bonus", 3_000, false, MonthDate.Now.AddMonths(6)),
            new("Part-time Teaching", 600, true, MonthDate.Now.AddMonths(2)),
            new("Stock Options Vest", 5_000, false, MonthDate.Now.AddMonths(12)),
            new("Holiday Bonus", 2_000, false, MonthDate.Now.AddMonths(11)),
        ],
        Outcomes: [
            // Fixed Monthly Expenses
            new("Mortgage", 1_850, true, MonthDate.Now),
            new("Property Tax", 420, true, MonthDate.Now),
            new("Home Insurance", 180, true, MonthDate.Now),
            new("Car Payment", 450, true, MonthDate.Now),
            new("Car Insurance", 160, true, MonthDate.Now),
            new("Health Insurance", 380, true, MonthDate.Now),
            new("Dental Insurance", 45, true, MonthDate.Now),
            new("Life Insurance", 85, true, MonthDate.Now),
            new("Phone Bill", 95, true, MonthDate.Now),
            new("Internet", 75, true, MonthDate.Now),
            new("Streaming Services", 65, true, MonthDate.Now),
            new("Gym Membership", 50, true, MonthDate.Now),
            new("Student Loan", 320, true, MonthDate.Now),
            new("Credit Card Minimum", 180, true, MonthDate.Now),

            // Variable Monthly Expenses
            new("Groceries", 450, true, MonthDate.Now),
            new("Gasoline", 200, true, MonthDate.Now),
            new("Utilities", 150, true, MonthDate.Now),
            new("Dining Out", 300, true, MonthDate.Now),
            new("Entertainment", 200, true, MonthDate.Now),
            new("Personal Care", 100, true, MonthDate.Now),
            new("Household Items", 80, true, MonthDate.Now),
            new("Pet Expenses", 120, true, MonthDate.Now),
            new("Charity", 200, true, MonthDate.Now),
            new("Savings", 500, true, MonthDate.Now),
            new("Emergency Fund", 300, true, MonthDate.Now),
            new("401k Contribution", 600, true, MonthDate.Now),
            new("IRA Contribution", 500, true, MonthDate.Now),

            // One-time/Irregular Expenses
            new("Winter Clothes", 400, false, MonthDate.Now.AddMonths(1)),
            new("Car Maintenance", 350, false, MonthDate.Now.AddMonths(2)),
            new("Vacation Fund", 2_500, false, MonthDate.Now.AddMonths(3)),
            new("Home Repairs", 1_200, false, MonthDate.Now.AddMonths(4)),
            new("Medical Checkup", 300, false, MonthDate.Now.AddMonths(5)),
            new("Summer Clothes", 300, false, MonthDate.Now.AddMonths(6)),
            new("Back to School", 600, false, MonthDate.Now.AddMonths(7)),
            new("Holiday Gifts", 800, false, MonthDate.Now.AddMonths(8)),
            new("Car Registration", 150, false, MonthDate.Now.AddMonths(9)),
            new("Professional Development", 500, false, MonthDate.Now.AddMonths(10)),
            new("Home Appliance Replacement", 1_500, false, MonthDate.Now.AddMonths(11)),
            new("Annual Insurance Premium", 600, false, MonthDate.Now.AddMonths(12)),
            new("Vacation Trip", 4_000, false, MonthDate.Now.AddMonths(13)),
            new("Furniture Upgrade", 2_200, false, MonthDate.Now.AddMonths(14)),
            new("Wedding Gift", 250, false, MonthDate.Now.AddMonths(15)),
            new("Computer Upgrade", 1_800, false, MonthDate.Now.AddMonths(16)),
            new("Dental Work", 800, false, MonthDate.Now.AddMonths(17)),
            new("Home Security System", 600, false, MonthDate.Now.AddMonths(18)),
            new("Kitchen Renovation", 8_000, false, MonthDate.Now.AddMonths(20)),
            new("New Car Down Payment", 5_000, false, MonthDate.Now.AddMonths(24))]);

    public static CalculateInput CalculateInputExample => calculateInputExample;
}
