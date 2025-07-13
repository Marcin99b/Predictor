# Predictor

![License](https://img.shields.io/badge/license-MIT-blue.svg)

**"When will I be able to afford that house?"**

Instead of guessing, get a real answer. Predictor simulates your budget months into the future, accounting for your salary, expenses, and life events.

## The Problem

You want to buy something expensive but don't know when you'll have enough money. Maybe it's a house down payment, a new car, or just building an emergency fund.

Budget apps show you where your money *went*, but won't tell you when you'll hit your savings goals. Spreadsheets work but become a mess when you have irregular income, loans that end, or seasonal expenses.

## What Predictor Does

Stop guessing about your financial future. Predictor takes the uncertainty out of long-term planning by showing you exactly when you'll reach your goals.

Whether you're saving for something specific or just want to see where your money will be in a year, Predictor handles the complexity:

- Income that changes over time (raises, contract work ending)
- Expenses that disappear (when you finish paying off that car loan)
- Irregular costs (annual insurance, quarterly taxes)
- Unexpected events (tax refunds, emergency repairs)

You'll get a clear timeline instead of guesswork.

## Quick Start

git clone <https://github.com/Marcin99b/Predictor.git>
cd predictor/src
dotnet run --project Predictor.Web

Open `https://localhost:7176/swagger` and try it with the built-in example data.

*Docker support is available but optional - see Dockerfile in the project.*

## Example

Start with a simple scenario:

{
  "initialBudget": 10000,
  "startCalculationMonth": { "month": 7, "year": 2025 },
  "incomes": [
    {
      "name": "Salary",
      "value": 5000,
      "startDate": { "month": 7, "year": 2025 },
      "recurringConfig": { "monthInterval": 1 }
    }
  ],
  "outcomes": [
    {
      "name": "Rent",
      "value": 2000,
      "startDate": { "month": 7, "year": 2025 },
      "recurringConfig": { "monthInterval": 1 }
    }
  ]
}

This shows someone earning $5k/month, paying $2k rent, starting with $10k saved. The API returns month-by-month projections showing they'll save $3k each month.

**Try it yourself:**

# Get more complex example

curl -X GET "<https://localhost:7176/example-data>"

# Run prediction

curl -X POST "<https://localhost:7176/calc>" -H "Content-Type: application/json" -d @example-data.json

## API

### `POST /calc`

Send your financial data, get month-by-month predictions.

**Input:**
{
  "initialBudget": 48750,
  "startCalculationMonth": { "month": 7, "year": 2025 },
  "incomes": [
    {
      "name": "Salary",
      "value": 5400,
      "startDate": { "month": 7, "year": 2025 },
      "recurringConfig": { "monthInterval": 1 }
    }
  ],
  "outcomes": [
    {
      "name": "Rent",
      "value": 2300,
      "startDate": { "month": 7, "year": 2025 },
      "recurringConfig": { "monthInterval": 1 }
    }
  ]
}

**Output:**
{
  "months": [
    {
      "monthDate": { "month": 7, "year": 2025 },
      "budgetAfter": 51850,
      "balance": 3100,
      "income": 5400,
      "outcome": 2300
    }
  ]
}

### `GET /example-data`

Returns sample data you can modify and use for testing.

## Use Cases

**House buying:** "I want a 400k house. When will I have 20% down?"

You are earning 8k per month, spending 6k, and have 25k saved. Enter your numbers and Predictor shows you will hit 80k (20% down) in exactly 27.5 months. Now you can plan accordingly instead of hoping it works out.

**Emergency fund planning:** "How long until I have 6 months of expenses saved?"

Your monthly expenses are $4,500, so you need $27k total. Currently saving $800/month with $5k already set aside. Predictor calculates you'll reach your goal in 27.5 months, accounting for any irregular income or expense changes you've planned.

**Career transition:** "If I take this lower-paying job, how will it affect my car purchase timeline?"

Compare scenarios: current job vs. new job with 1k less monthly income. See exactly how much longer you will need to save for that 25k car, helping you make an informed decision about the career move.

**Debt freedom:** "When will I be completely debt-free?"

Enter all your loans with their payment schedules. See not just when each individual debt disappears, but how your cash flow improves month by month as payments end, and when you'll have true financial freedom.

## Development Roadmap

Here's where I want to take this project:

### Core Features

- [x] Basic budget calculation engine
- [x] Flexible recurring payments (configurable intervals - every N months)
- [x] Time-limited payments (loans that end, contracts)
- [x] One-time income and expenses
- [x] Input validation with FluentValidation
- [ ] Inflation adjustments
- [ ] Financial goal tracking ("alert me when I can afford X")
- [ ] Scenario comparison ("what if I get a raise vs. what if I move?")
- [ ] Multi-currency support
- [ ] Risk analysis and confidence intervals

### Performance & Scale  

- [x] Performance testing setup
- [ ] Caching layer
- [ ] Background processing for complex calculations
- [ ] Performance benchmarking
- [ ] Rate limiting

### Data & Intelligence

- [ ] Historical trend analysis  
- [ ] External data integration (exchange rates, inflation)
- [ ] Smart spending optimization suggestions
- [ ] Export to popular formats (CSV, PDF reports)

### Developer Experience

- [x] REST API with Swagger documentation
- [x] Health check endpoints
- [ ] Enhanced API documentation with more examples
- [ ] SDK/client libraries
- [ ] Integration guides

## Contributing

Want to help make financial planning easier for everyone? Contributions are welcome whether you're new to programming or a seasoned developer.

**Getting started is easy:**

1. Browse [Issues](https://github.com/Marcin99b/Predictor/issues) - look for `good-first-issue` if you're new to open source
2. Fork the repo and create a branch: `git checkout -b feature/123-feature-name` (or `bug/123-bug-name` for bugs)
3. Make your changes and test locally
4. Submit a PR and link the issue by adding `Closes #123` in the description or using the GitHub UI

Every contribution helps, from fixing typos to optimizing algorithms. Jump in!

## Tech Stack

- .NET 8 + ASP.NET Core
- FluentValidation for input validation
- Swagger/OpenAPI for documentation
- NBomber for performance testing
- NUnit for unit testing
- Docker (optional)
- Planned: Rust for performance-critical calculations

## Running Tests

# Performance tests  

cd src/Predictor.Tests.Performance
dotnet test

Performance tests validate that the API can handle high load scenarios. They're useful for ensuring calculations remain fast as complexity grows.

## License

MIT - see [LICENSE](LICENSE) file.