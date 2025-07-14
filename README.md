# Predictor

![License](https://img.shields.io/badge/license-MIT-blue.svg)
[![codecov](https://codecov.io/gh/Marcin99b/Predictor/graph/badge.svg?token=iMzRpPQ5r9)](https://codecov.io/gh/Marcin99b/Predictor)

**"When will I be able to afford that house?"**

Instead of guessing, get a real answer. Predictor simulates your budget months into the future, accounting for your salary, expenses, and life events.

## Table of Contents

- [The Problem](#the-problem)
- [What Predictor Does](#what-predictor-does)
- [Quick Start](#quick-start)
- [Example](#example)
- [API](#api)
  - [POST /api/v1/predictions](#post-apiv1predictions)
  - [PUT /api/v1/predictions/{id}](#put-apiv1predictionsid)
  - [GET /api/v1/predictions/example](#get-apiv1predictionsexample)
  - [GET /api/v1/predictions/{id}](#get-apiv1predictionsid)
  - [GET /api/v1/predictions/{id}/summary](#get-apiv1predictionsidsummary)
  - [GET /api/v1/predictions/{id}/months](#get-apiv1predictionsidmonths)
  - [POST /api/v1/analytics/check-goal](#post-apiv1analyticscheck-goal)
- [Use Cases](#use-cases)
- [Development Roadmap](#development-roadmap)
- [Contributing](#contributing)
- [Tech Stack](#tech-stack)
- [Running Tests](#running-tests)
- [License](#license)

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

```bash
git clone https://github.com/Marcin99b/Predictor.git
cd predictor/src
dotnet run --project Predictor.Web
```

Open `https://localhost:7176/swagger` and try it with the built-in example data.

*Docker support is available but optional - see Dockerfile in the project.*

## Example

Start with a simple scenario:

```json
{
  "predictionMonths": 12,
  "initialBudget": 10000,
  "startPredictionMonth": { "month": 7, "year": 2025 },
  "incomes": [
    {
      "name": "Salary",
      "value": 5000,
      "startDate": { "month": 7, "year": 2025 },
      "frequency": 2
    }
  ],
  "expenses": [
    {
      "name": "Rent",
      "value": 2000,
      "startDate": { "month": 7, "year": 2025 },
      "frequency": 2
    }
  ]
}
```

This shows someone earning $5k/month, paying $2k rent, starting with $10k saved. The API returns month-by-month projections showing they'll save $3k each month.

**Try it yourself:**

```bash
# Get example data
curl -X GET "https://localhost:7176/api/v1/predictions/example"

# Run prediction
curl -X POST "https://localhost:7176/api/v1/predictions" -H "Content-Type: application/json" -d @example-data.json
```

## API

### `POST /api/v1/predictions`

Send your financial data, get month-by-month predictions.

**Input:**

```json
{
  "predictionMonths": 12,
  "initialBudget": 10000,
  "startPredictionMonth": { "month": 7, "year": 2025 },
  "incomes": [
    {
      "name": "Salary",
      "value": 5000,
      "startDate": { "month": 7, "year": 2025 },
      "frequency": 2
    }
  ],
  "expenses": [
    {
      "name": "Rent",
      "value": 2000,
      "startDate": { "month": 7, "year": 2025 },
      "frequency": 2
    }
  ]
}
```

**Output:**

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "summary": {
    "startingBalance": 3000,
    "endingBalance": 46000,
    "totalIncome": 60000,
    "totalExpenses": 24000,
    "lowestBalance": 3000,
    "lowestBalanceDate": { "month": 7, "year": 2025 },
    "highestBalance": 46000,
    "highestBalanceDate": { "month": 6, "year": 2026 }
  },
  "months": [
    {
      "monthDate": { "month": 7, "year": 2025 },
      "budgetAfter": 13000,
      "balance": 3000,
      "income": 5000,
      "expense": 2000
    }
  ]
}
```

### `PUT /api/v1/predictions/{id}`

Update an existing prediction with new data.

### `GET /api/v1/predictions/example`

Returns sample data you can modify and use for testing.

### `GET /api/v1/predictions/{id}`

Retrieve a complete prediction result by ID.

### `GET /api/v1/predictions/{id}/summary`

Get only the summary data for a prediction.

### `GET /api/v1/predictions/{id}/months`

Get only the month-by-month breakdown for a prediction.

### `POST /api/v1/analytics/check-goal`

Check if a specific financial goal is met in a given month.

**Input:**

```json
{
  "predictionId": "123e4567-e89b-12d3-a456-426614174000",
  "month": { "month": 12, "year": 2025 },
  "balanceHigherOrEqual": 50000,
  "incomeHigherOrEqual": 5000,
  "expenseLowerOrEqual": 3000
}
```

**Output:** `true` or `false`

## Use Cases

**House buying:** "I want a $400k house. When will I have 20% down?"

You are earning $8k per month, spending $6k, and have $25k saved. Enter your numbers and Predictor shows you will hit $80k (20% down) in exactly 27.5 months. Now you can plan accordingly instead of hoping it works out.

**Emergency fund planning:** "How long until I have 6 months of expenses saved?"

Your monthly expenses are $4,500, so you need $27k total. Currently saving $800/month with $5k already set aside. Predictor calculates you'll reach your goal in 27.5 months, accounting for any irregular income or expense changes you've planned.

**Career transition:** "If I take this lower-paying job, how will it affect my car purchase timeline?"

Compare scenarios: current job vs. new job with $1k less monthly income. See exactly how much longer you will need to save for that $25k car, helping you make an informed decision about the career move.

**Debt freedom:** "When will I be completely debt-free?"

Enter all your loans with their payment schedules. See not just when each individual debt disappears, but how your cash flow improves month by month as payments end, and when you'll have true financial freedom.

## Development Roadmap

Here's where I want to take this project:

### Core Features

- [x] Basic budget calculation engine
- [x] Flexible recurring payments (monthly, quarterly, semi-annually, annually)
- [x] Time-limited payments (loans that end, contracts)
- [x] One-time income and expenses
- [x] Input validation with FluentValidation
- [x] Prediction caching and retrieval
- [x] Goal checking analytics
- [ ] Inflation adjustments
- [ ] Financial goal tracking ("alert me when I can afford X")
- [ ] Scenario comparison ("what if I get a raise vs. what if I move?")
- [ ] Multi-currency support
- [ ] Risk analysis and confidence intervals

### Performance & Scale  

- [x] Performance testing setup
- [ ] Caching layer improvements
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

1. Browse [Issues](https://github.com/Marcin99b/Predictor/issues) - look for `good-first-issue`
2. Fork the repo and create a branch: `git checkout -b feature/123-feature-name` (or `bug/123-bug-name` for bugs)
3. Make your changes and test locally
4. Submit a PR and link the issue by adding `Closes #123` in the description or using the GitHub UI

Every contribution helps, from fixing typos to optimizing algorithms. Jump in!

## Tech Stack

- .NET 8 + ASP.NET Core
- FluentValidation for input validation
- MediatR for request handling
- Swagger/OpenAPI for documentation
- NBomber for performance testing
- NUnit for unit testing
- Docker (optional)
- Memory caching for prediction storage
- Planned: Rust for performance-critical calculations

## Running Tests

```bash
# Performance tests  
cd src/Predictor.Tests.Performance
dotnet test
```

Performance tests validate that the API can handle high load scenarios. They're useful for ensuring calculations remain fast as complexity grows.

**Note:** Performance tests are marked with `[Ignore]` by default. Remove the ignore attribute to run them against a running instance of the API.

## License

MIT - see [LICENSE](LICENSE) file.
