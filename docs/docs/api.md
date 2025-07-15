---
id: api
slug: /api
sidebar_position: 2
title: API Reference
---

# API Reference

## Endpoints

### `POST /api/v1/predictions`
Send your financial data, get month-by-month predictions.

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

## Example Request

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

## Example Response

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
