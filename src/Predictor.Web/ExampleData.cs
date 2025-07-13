using Predictor.Web.Models;

namespace Predictor.Web;

public static class ExampleData
{
    public static PredictionRequest CalculateInputExample { get; } = new(
        PredictionMonths: 36,
        InitialBudget: 48_750,
        StartPredictionMonth: MonthDate.Now,
        Incomes: [
            // Infinite recurring income (no EndDate)
            new("Primary Salary", 5_400, MonthDate.Now, Frequency.Monthly),
            new("Spouse Salary", 4_100, MonthDate.Now, Frequency.Monthly),
            new("Rental Property A", 1_500, MonthDate.Now, Frequency.Monthly),
            new("Rental Property B", 1_100, MonthDate.Now.AddMonths(3), Frequency.Monthly),
            new("Investment Dividends", 320, MonthDate.Now.AddMonths(1), Frequency.Quarterly),
            new("Side Business", 850, MonthDate.Now.AddMonths(2), Frequency.Monthly),

            // Finite recurring income (with EndDate)
            new("Contract Work", 2_200, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(18)),
            new("Consulting Retainer", 1_800, MonthDate.Now.AddMonths(1), Frequency.Monthly, MonthDate.Now.AddMonths(24)),
            new("Freelance Project", 1_200, MonthDate.Now.AddMonths(2), Frequency.Quarterly, MonthDate.Now.AddMonths(14)),
            new("Teaching Position", 900, MonthDate.Now.AddMonths(1), Frequency.Monthly, MonthDate.Now.AddMonths(10)),
            new("Seasonal Work", 1_400, MonthDate.Now.AddMonths(4), Frequency.Monthly, MonthDate.Now.AddMonths(9)),
            new("Part-time Job", 650, MonthDate.Now.AddMonths(6), Frequency.Monthly, MonthDate.Now.AddMonths(15)),
            new("Quarterly Bonus", 2_500, MonthDate.Now.AddMonths(3), Frequency.Quarterly, MonthDate.Now.AddMonths(21)),
            new("Investment Payout", 800, MonthDate.Now.AddMonths(2), Frequency.SemiAnnually, MonthDate.Now.AddMonths(26)),
            new("Royalty Income", 450, MonthDate.Now.AddMonths(1), Frequency.Quarterly, MonthDate.Now.AddMonths(25)),
            new("Rental Income C", 900, MonthDate.Now.AddMonths(12), Frequency.Monthly, MonthDate.Now.AddMonths(36)),

            // One-time income
            new("Tax Refund", 3_200, MonthDate.Now.AddMonths(4)),
            new("Insurance Settlement", 8_500, MonthDate.Now.AddMonths(7)),
            new("Inheritance", 22_000, MonthDate.Now.AddMonths(11)),
            new("Asset Sale", 15_000, MonthDate.Now.AddMonths(16)),
            new("Lottery Winnings", 5_000, MonthDate.Now.AddMonths(19)),
            new("Legal Settlement", 12_000, MonthDate.Now.AddMonths(28)),
            new("Business Sale", 45_000, MonthDate.Now.AddMonths(32)),
            new("Stock Options", 18_000, MonthDate.Now.AddMonths(35)),
        ],
        Expenses: [
            // Infinite recurring expenses (no EndDate)
            new("Primary Mortgage", 2_300, MonthDate.Now, Frequency.Monthly),
            new("Property Tax", 520, MonthDate.Now, Frequency.Monthly),
            new("Home Insurance", 280, MonthDate.Now, Frequency.Monthly),
            new("Car Insurance", 190, MonthDate.Now, Frequency.Monthly),
            new("Health Insurance", 750, MonthDate.Now, Frequency.Monthly),
            new("Life Insurance", 140, MonthDate.Now, Frequency.Monthly),
            new("Phone Bills", 160, MonthDate.Now, Frequency.Monthly),
            new("Internet", 95, MonthDate.Now, Frequency.Monthly),
            new("Utilities", 220, MonthDate.Now, Frequency.Monthly),
            new("Groceries", 720, MonthDate.Now, Frequency.Monthly),
            new("Gasoline", 320, MonthDate.Now, Frequency.Monthly),
            new("Dining Out", 450, MonthDate.Now, Frequency.Monthly),
            new("Entertainment", 280, MonthDate.Now, Frequency.Monthly),
            new("Personal Care", 180, MonthDate.Now, Frequency.Monthly),
            new("Pet Expenses", 220, MonthDate.Now, Frequency.Monthly),
            new("Charity", 350, MonthDate.Now, Frequency.Monthly),
            new("Savings", 800, MonthDate.Now, Frequency.Monthly),
            new("401k", 900, MonthDate.Now, Frequency.Monthly),
            new("IRA", 650, MonthDate.Now, Frequency.Monthly),
            new("Investment Account", 1_200, MonthDate.Now, Frequency.Monthly),

            // Finite recurring expenses (with EndDate)
            new("Student Loan 1", 480, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(48)),
            new("Student Loan 2", 320, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(36)),
            new("Car Payment 1", 580, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(42)),
            new("Car Payment 2", 420, MonthDate.Now.AddMonths(6), Frequency.Monthly, MonthDate.Now.AddMonths(54)),
            new("Personal Loan", 380, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(24)),
            new("Credit Card Payment", 450, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(18)),
            new("Gym Membership", 85, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(12)),
            new("Streaming Services", 120, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(24)),
            new("Child Care", 1_200, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(60)),
            new("Tutoring", 300, MonthDate.Now.AddMonths(2), Frequency.Monthly, MonthDate.Now.AddMonths(22)),
            new("Music Lessons", 180, MonthDate.Now.AddMonths(1), Frequency.Monthly, MonthDate.Now.AddMonths(18)),
            new("Sports Club", 150, MonthDate.Now.AddMonths(3), Frequency.Monthly, MonthDate.Now.AddMonths(15)),
            new("Rental Property A Mortgage", 950, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(180)),
            new("Rental Property B Mortgage", 720, MonthDate.Now.AddMonths(3), Frequency.Monthly, MonthDate.Now.AddMonths(240)),
            new("Business Loan", 850, MonthDate.Now.AddMonths(2), Frequency.Monthly, MonthDate.Now.AddMonths(84)),
            new("Equipment Lease", 280, MonthDate.Now.AddMonths(1), Frequency.Monthly, MonthDate.Now.AddMonths(36)),
            new("Software Subscription", 95, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(12)),
            new("Professional Membership", 75, MonthDate.Now, Frequency.Monthly, MonthDate.Now.AddMonths(24)),

            // Quarterly expenses with EndDate
            new("Property Management", 180, MonthDate.Now.AddMonths(1), Frequency.Quarterly, MonthDate.Now.AddMonths(36)),
            new("Lawn Service", 220, MonthDate.Now.AddMonths(2), Frequency.Quarterly, MonthDate.Now.AddMonths(30)),
            new("Pest Control", 140, MonthDate.Now, Frequency.Quarterly, MonthDate.Now.AddMonths(24)),
            new("HVAC Maintenance", 300, MonthDate.Now.AddMonths(3), Frequency.Quarterly, MonthDate.Now.AddMonths(48)),
            new("Pool Maintenance", 180, MonthDate.Now.AddMonths(1), Frequency.Quarterly, MonthDate.Now.AddMonths(21)),

            // Semi-annual expenses with EndDate
            new("Car Maintenance", 650, MonthDate.Now.AddMonths(3), Frequency.SemiAnnually, MonthDate.Now.AddMonths(42)),
            new("Home Maintenance", 1_200, MonthDate.Now.AddMonths(2), Frequency.SemiAnnually, MonthDate.Now.AddMonths(60)),
            new("Medical Checkups", 800, MonthDate.Now.AddMonths(4), Frequency.SemiAnnually, MonthDate.Now.AddMonths(48)),
            new("Dental Work", 450, MonthDate.Now.AddMonths(1), Frequency.SemiAnnually, MonthDate.Now.AddMonths(36)),
            new("Eye Exams", 280, MonthDate.Now.AddMonths(5), Frequency.SemiAnnually, MonthDate.Now.AddMonths(30)),
            new("Veterinary Care", 350, MonthDate.Now.AddMonths(2), Frequency.SemiAnnually, MonthDate.Now.AddMonths(54)),

            // Annual expenses with EndDate
            new("Vacation Fund", 4_500, MonthDate.Now.AddMonths(6), Frequency.Annually, MonthDate.Now.AddMonths(60)),
            new("Holiday Gifts", 1_500, MonthDate.Now.AddMonths(11), Frequency.Annually, MonthDate.Now.AddMonths(48)),
            new("Tax Preparation", 450, MonthDate.Now.AddMonths(3), Frequency.Annually, MonthDate.Now.AddMonths(36)),
            new("Insurance Premium", 950, MonthDate.Now.AddMonths(8), Frequency.Annually, MonthDate.Now.AddMonths(72)),
            new("Professional Development", 800, MonthDate.Now.AddMonths(4), Frequency.Annually, MonthDate.Now.AddMonths(60)),
            new("Charity Donation", 1_200, MonthDate.Now.AddMonths(10), Frequency.Annually, MonthDate.Now.AddMonths(84)),
            new("Home Warranty", 600, MonthDate.Now.AddMonths(7), Frequency.Annually, MonthDate.Now.AddMonths(36)),

            // One-time expenses
            new("Emergency Car Repair", 1_800, MonthDate.Now.AddMonths(2)),
            new("Appliance Replacement", 2_400, MonthDate.Now.AddMonths(5)),
            new("Wedding Gift", 750, MonthDate.Now.AddMonths(8)),
            new("Computer Upgrade", 3_200, MonthDate.Now.AddMonths(10)),
            new("Roof Repair", 8_500, MonthDate.Now.AddMonths(12)),
            new("Kitchen Renovation", 22_000, MonthDate.Now.AddMonths(15)),
            new("Bathroom Remodel", 12_000, MonthDate.Now.AddMonths(18)),
            new("Flooring Replacement", 9_500, MonthDate.Now.AddMonths(21)),
            new("New Car Down Payment", 12_000, MonthDate.Now.AddMonths(24)),
            new("College Tuition", 18_000, MonthDate.Now.AddMonths(27)),
            new("Medical Emergency", 6_500, MonthDate.Now.AddMonths(30)),
            new("Legal Fees", 4_200, MonthDate.Now.AddMonths(33)),
            new("Business Investment", 35_000, MonthDate.Now.AddMonths(36)),
            new("Home Addition", 45_000, MonthDate.Now.AddMonths(39)),
            new("Luxury Purchase", 18_000, MonthDate.Now.AddMonths(42)),
            new("Investment Property Down Payment", 25_000, MonthDate.Now.AddMonths(45)),
            new("Retirement Catch-up", 30_000, MonthDate.Now.AddMonths(48))
        ]
    );
}