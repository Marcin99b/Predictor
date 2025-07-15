---
id: overview
slug: /
title: Predictor Overview
sidebar_position: 1
---

# Predictor

**"When will I be able to afford that house?"**

Predictor simulates your budget months into the future, accounting for your salary, expenses, and life events.

## The problem

You want to buy something expensive but don't know when you'll have enough money. Maybe it's a house down payment, a new car, or just building an emergency fund.

Budget apps show you where your money *went*, but won't tell you when you'll hit your savings goals. Spreadsheets work but become a mess when you have irregular income, loans that end, or seasonal expenses.

## What Predictor does

Predictor takes the uncertainty out of long-term planning by showing you exactly when you'll reach your goals.

- Income that changes over time (raises, contract work ending)
- Expenses that disappear (when you finish paying off that car loan)
- Irregular costs (annual insurance, quarterly taxes)
- Unexpected events (tax refunds, emergency repairs)

You'll get a clear timeline instead of guesswork.

## Quick start

```bash
git clone https://github.com/Marcin99b/Predictor.git
cd predictor/src
dotnet run --project Predictor.Web
```

Open `https://localhost:7176/swagger` and try it with the built-in example data.

*Docker support is available but optional - see Dockerfile in the project.*

## Example

Here's what a simple prediction looks like:

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

This person earns $5k/month, pays $2k rent, starts with $10k saved. They'll save $3k each month.

The API returns month-by-month projections showing exactly how their money grows over time.

## Real use cases

**House buying:** "I want a $400k house. When will I have 20% down?"

You earn $8k per month, spend $6k, and have $25k saved. Enter your numbers and Predictor shows you'll hit $80k (20% down) in exactly 27.5 months. Now you can plan accordingly instead of hoping it works out.

**Emergency fund planning:** "How long until I have 6 months of expenses saved?"

Your monthly expenses are $4,500, so you need $27k total. Currently saving $800/month with $5k already set aside. Predictor calculates you'll reach your goal in 27.5 months, accounting for any irregular income or expense changes you've planned.

**Career transition:** "If I take this lower-paying job, how will it affect my car purchase timeline?"

Compare scenarios: current job vs. new job with $1k less monthly income. See exactly how much longer you'll need to save for that $25k car, helping you make an informed decision about the career move.

**Debt freedom:** "When will I be completely debt-free?"

Enter all your loans with their payment schedules. See not just when each individual debt disappears, but how your cash flow improves month by month as payments end, and when you'll have true financial freedom.

## How it works

1. **Input your data** - Current savings, income sources, expenses, timeline
2. **Run the simulation** - Predictor calculates month-by-month projections
3. **Get your answer** - See exactly when you'll reach your goals

The API handles:

- Multiple income sources with different frequencies
- Expenses that start, stop, or change over time
- One-time events (bonuses, emergencies, major purchases)
- Complex scenarios (seasonal work, contract jobs, loan payoffs)

## Payment types

Predictor supports realistic payment scenarios:

- **One-time payments** - Tax refunds, bonuses, emergency repairs
- **Monthly** - Salary, rent, most bills
- **Quarterly** - Insurance payments, estimated taxes
- **Semi-annually** - Car maintenance, some subscriptions
- **Annually** - Vacation funds, holiday spending

You can set start and end dates for any payment. Perfect for modeling loans that get paid off, contracts that end, or new income that starts later.

## Next steps

- [Try the API](./api) - See all endpoints and examples
- [Read use cases](./use-cases) - More real-world scenarios
- [Check the roadmap](./roadmap) - What's coming next
- [Contribute](./contributing) - Help make it better
