---
id: api
slug: /api
sidebar_position: 2
title: API Reference
---

# API Reference

## Base URL

`https://localhost:7176/api/v1`

All endpoints return JSON. Check out the interactive docs at `/swagger` while the app is running.

## Main endpoints

### Create a prediction

**`POST /predictions`**

Send your financial data, get month-by-month predictions.

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

**Response:**

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

**What the response means:**

- `balance` - How much you made/lost that month (income - expenses)
- `budgetAfter` - Total money you have after that month
- `lowestBalance` - Your worst month (watch for negatives!)
- `totalIncome` - Everything you'll make during the prediction period

### Update a prediction

**`PUT /predictions/{id}`**

Change an existing prediction. Same request format as creating one.

### Get example data

**`GET /predictions/example`**

Returns a complex example with lots of different income/expense types. Good for testing.

### Get a prediction

**`GET /predictions/{id}`** - Full prediction  
**`GET /predictions/{id}/summary`** - Just the summary  
**`GET /predictions/{id}/months`** - Just the month-by-month data

## Payment items

Each income or expense looks like this:

```json
{
  "name": "Salary",
  "value": 5000,
  "startDate": { "month": 7, "year": 2025 },
  "frequency": 2,
  "endDate": { "month": 12, "year": 2027 }
}
```

- `name` - What you want to call it (3-100 characters)
- `value` - How much money (must be positive)
- `startDate` - When it begins
- `frequency` - How often it happens (see below)
- `endDate` - When it stops (optional)

## Frequency codes

- `1` (OneTime) - Happens once
- `2` (Monthly) - Every month
- `3` (Quarterly) - Every 3 months
- `4` (SemiAnnually) - Every 6 months
- `5` (Annually) - Every 12 months

## Analytics

### Check if you hit a goal

**`POST /analytics/check-goal`**

```json
{
  "predictionId": "123e4567-e89b-12d3-a456-426614174000",
  "month": { "month": 12, "year": 2025 },
  "balanceHigherOrEqual": 50000,
  "incomeHigherOrEqual": 5000,
  "expenseLowerOrEqual": 3000
}
```

Returns `true` or `false` depending on whether you hit your targets in that month.

You can check any combination of:

- `balanceHigherOrEqual` - Did you save at least this much?
- `incomeHigherOrEqual` - Did you make at least this much?
- `expenseLowerOrEqual` - Did you spend less than this?

## Common mistakes

**Getting 400 errors?**

- Check that your dates are valid (month 1-12, year 1900-2999)
- Make sure payment values are positive
- Payment names need to be at least 3 characters

**Prediction looks wrong?**

- Double-check your frequency codes
- Make sure your start/end dates make sense
- Remember that `balance` is monthly cash flow, `budgetAfter` is your total money

## Examples

### Simple scenario

Monthly salary and rent:

```bash
curl -X POST "https://localhost:7176/api/v1/predictions" \
  -H "Content-Type: application/json" \
  -d '{
    "predictionMonths": 6,
    "initialBudget": 5000,
    "startPredictionMonth": { "month": 1, "year": 2025 },
    "incomes": [
      {
        "name": "Job",
        "value": 4000,
        "startDate": { "month": 1, "year": 2025 },
        "frequency": 2
      }
    ],
    "expenses": [
      {
        "name": "Rent",
        "value": 1500,
        "startDate": { "month": 1, "year": 2025 },
        "frequency": 2
      }
    ]
  }'
```

### Loan that gets paid off

```bash
curl -X POST "https://localhost:7176/api/v1/predictions" \
  -H "Content-Type: application/json" \
  -d '{
    "predictionMonths": 24,
    "initialBudget": 10000,
    "startPredictionMonth": { "month": 1, "year": 2025 },
    "incomes": [
      {
        "name": "Salary",
        "value": 5000,
        "startDate": { "month": 1, "year": 2025 },
        "frequency": 2
      }
    ],
    "expenses": [
      {
        "name": "Car Payment",
        "value": 400,
        "startDate": { "month": 1, "year": 2025 },
        "frequency": 2,
        "endDate": { "month": 12, "year": 2025 }
      }
    ]
  }'
```

Car payment ends in December 2025, so your cash flow improves after that.

### One-time bonus

```bash
curl -X POST "https://localhost:7176/api/v1/predictions" \
  -H "Content-Type: application/json" \
  -d '{
    "predictionMonths": 12,
    "initialBudget": 2000,
    "startPredictionMonth": { "month": 1, "year": 2025 },
    "incomes": [
      {
        "name": "Christmas Bonus",
        "value": 5000,
        "startDate": { "month": 12, "year": 2025 },
        "frequency": 1
      }
    ],
    "expenses": []
  }'
```

This gives you $5k in December only.
