# Predictor

![License](https://img.shields.io/badge/license-MIT-blue.svg)
[![codecov](https://codecov.io/gh/Marcin99b/Predictor/graph/badge.svg?token=iMzRpPQ5r9)](https://codecov.io/gh/Marcin99b/Predictor)

**"When will I be able to afford that house?"**

Predictor gives you a real answer. Simulate your budget months into the future, accounting for salary, expenses, and life events. See exactly when you'll reach your financial goals.

## Why I built this

Budget apps show you where your money went. Spreadsheets are a pain. You want to know *when* you'll have enough money for something important, not just track what you already spent.

I got tired of guessing whether I could afford things, so I built this to get actual answers.

## Documentation

- Full docs: [https://marcin99b.github.io/Predictor/](https://marcin99b.github.io/Predictor/)
- This README
- `/swagger` when running the API

## Quick start

```bash
git clone https://github.com/Marcin99b/Predictor.git
cd predictor/src
dotnet run --project Predictor.Web
```

Open [`https://localhost:7176/swagger`](https://localhost:7176/swagger) to try the API.

Or use Docker:

```bash
docker build -f src/Predictor.Web/Dockerfile -t predictor .
docker run -p 8080:8080 predictor
```

## What it does

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

## Real examples

**House buying:** "I want a $400k house. When will I have 20% down?"

You earn $8k per month, spend $6k, and have $25k saved. Enter your numbers and Predictor shows you'll hit $80k (20% down) in exactly 27.5 months. Now you can plan accordingly instead of hoping it works out.

**Emergency fund:** "How long until I have 6 months of expenses saved?"

Your monthly expenses are $4,500, so you need $27k total. Currently saving $800/month with $5k already set aside. Predictor calculates you'll reach your goal in 27.5 months, accounting for any irregular income or expense changes you've planned.

**Career transition:** "If I take this lower-paying job, how will it affect my car purchase timeline?"

Compare scenarios: current job vs. new job with $1k less monthly income. See exactly how much longer you'll need to save for that $25k car.

**Debt freedom:** "When will I be completely debt-free?"

Enter all your loans with their payment schedules. See not just when each individual debt disappears, but how your cash flow improves month by month as payments end.

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

### Other endpoints

- `PUT /api/v1/predictions/{id}` - Update an existing prediction
- `GET /api/v1/predictions/example` - Get sample data to try
- `GET /api/v1/predictions/{id}` - Get a prediction by ID
- `GET /api/v1/predictions/{id}/summary` - Just the summary
- `GET /api/v1/predictions/{id}/months` - Just the month breakdown
- `POST /api/v1/analytics/check-goal` - Check if a goal is met

Full docs at `/swagger` when running.

## Payment frequencies

- **OneTime (1)** - Happens once (bonus, tax refund)
- **Monthly (2)** - Every month (salary, rent)
- **Quarterly (3)** - Every 3 months (insurance)
- **SemiAnnually (4)** - Every 6 months (car maintenance)
- **Annually (5)** - Every year (vacation fund)

You can set `endDate` to make payments stop (loan payoffs, contracts ending).

## Features

What's working now:

- Basic budget calculation engine
- Flexible recurring payments (monthly, quarterly, etc.)
- Time-limited payments (loans that end, contracts)
- One-time income and expenses
- Input validation
- Prediction caching and retrieval
- Goal checking analytics

What's planned:

- Inflation adjustments
- Scenario comparison ("what if I get a raise vs. move cities?")
- Multi-currency support
- Risk analysis
- Export to CSV/PDF
- Web UI (it's just an API right now)

## Development

Requirements:

- .NET 8 or Docker

```bash
git clone https://github.com/Marcin99b/Predictor.git
cd predictor/src
dotnet restore
dotnet build
dotnet test
dotnet run --project Predictor.Web
```

The API will be at `https://localhost:7176`. Swagger docs at `/swagger`.

### Testing

```bash
# All tests
dotnet test

# Just integration tests
dotnet test src/Predictor.Tests.Integration

# Performance tests (need API running)
dotnet test src/Predictor.Tests.Performance
```

Performance tests are marked `[Ignore]` by default. Remove the attribute to run them.

## Contributing

Want to help? Here's how:

1. Fork it
2. Make a branch: `git checkout -b feature/cool-thing`
3. Make your changes
4. Test it: `dotnet test`
5. Submit a PR

Look for `good-first-issue` labels if you're new. Or just fix something that bugs you.

Things that would be helpful:

- Bug fixes
- Better documentation
- UI/frontend (currently just an API)
- Performance improvements
- New features from the roadmap

## Tech stack

- .NET 8 + ASP.NET Core
- FluentValidation for input checking
- MediatR for request handling
- Swagger for API docs
- NBomber for performance testing
- NUnit for tests
- Docker support
- Memory caching (will add real database later)

## Help

- [GitHub Issues](https://github.com/Marcin99b/Predictor/issues) for bugs
- [GitHub Discussions](https://github.com/Marcin99b/Predictor/discussions) for questions
- Check `/swagger` for API details

## License

MIT - use it however you want.
