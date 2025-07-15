---
id: use-cases
slug: /use-cases
title: Use Cases
sidebar_position: 3
---

# Use Cases

Real scenarios where Predictor helps you make better financial decisions.

## House buying

**"I want a $400k house. When will I have 20% down?"**

You earn $8k per month, spend $6k, and have $25k saved. Enter your numbers and Predictor shows you'll hit $80k (20% down) in exactly 27.5 months. Now you can plan accordingly instead of hoping it works out.

```json
{
  "predictionMonths": 36,
  "initialBudget": 25000,
  "incomes": [
    { "name": "Salary", "value": 8000, "frequency": 2 }
  ],
  "expenses": [
    { "name": "Living costs", "value": 6000, "frequency": 2 }
  ]
}
```

## Emergency fund planning

**"How long until I have 6 months of expenses saved?"**

Your monthly expenses are $4,500, so you need $27k total. Currently saving $800/month with $5k already set aside. Predictor calculates you'll reach your goal in 27.5 months, accounting for any irregular income or expense changes you've planned.

```json
{
  "predictionMonths": 30,
  "initialBudget": 5000,
  "incomes": [
    { "name": "Job", "value": 5300, "frequency": 2 }
  ],
  "expenses": [
    { "name": "All expenses", "value": 4500, "frequency": 2 }
  ]
}
```

Check when you hit $27k by looking at the month-by-month `budgetAfter` values.

## Career transition

**"If I take this lower-paying job, how will it affect my car purchase timeline?"**

Compare scenarios: current job vs. new job with $1k less monthly income. See exactly how much longer you'll need to save for that $25k car, helping you make an informed decision about the career move.

**Current job scenario:**

```json
{
  "predictionMonths": 24,
  "initialBudget": 10000,
  "incomes": [
    { "name": "Current job", "value": 6000, "frequency": 2 }
  ],
  "expenses": [
    { "name": "Expenses", "value": 4000, "frequency": 2 }
  ]
}
```

**New job scenario:**

```json
{
  "predictionMonths": 24,
  "initialBudget": 10000,
  "incomes": [
    { "name": "New job", "value": 5000, "frequency": 2 }
  ],
  "expenses": [
    { "name": "Expenses", "value": 4000, "frequency": 2 }
  ]
}
```

Compare the results to see how much longer it takes to save $25k.

## Debt freedom

**"When will I be completely debt-free?"**

Enter all your loans with their payment schedules. See not just when each individual debt disappears, but how your cash flow improves month by month as payments end, and when you'll have true financial freedom.

```json
{
  "predictionMonths": 60,
  "initialBudget": 5000,
  "incomes": [
    { "name": "Salary", "value": 5500, "frequency": 2 }
  ],
  "expenses": [
    { "name": "Living costs", "value": 3000, "frequency": 2 },
    { 
      "name": "Student loan", 
      "value": 300, 
      "frequency": 2,
      "endDate": { "month": 6, "year": 2027 }
    },
    { 
      "name": "Car payment", 
      "value": 450, 
      "frequency": 2,
      "endDate": { "month": 3, "year": 2026 }
    }
  ]
}
```

Watch how your monthly balance jumps up each time a loan ends.

## Seasonal business

**"My business makes most money in summer. Will I survive the winter?"**

Model irregular income and see if you have enough cash to get through slow periods.

```json
{
  "predictionMonths": 24,
  "initialBudget": 15000,
  "incomes": [
    { "name": "Summer revenue", "value": 8000, "startDate": { "month": 5, "year": 2025 }, "frequency": 2, "endDate": { "month": 9, "year": 2025 } },
    { "name": "Summer revenue", "value": 8000, "startDate": { "month": 5, "year": 2026 }, "frequency": 2, "endDate": { "month": 9, "year": 2026 } },
    { "name": "Winter income", "value": 2000, "frequency": 2 }
  ],
  "expenses": [
    { "name": "Business costs", "value": 4000, "frequency": 2 }
  ]
}
```

See if your `budgetAfter` ever goes negative during winter months.

## Freelancer cash flow

**"I have irregular project income. When should I take on that big project?"**

Model your existing contracts and see when you'll have capacity for new work.

```json
{
  "predictionMonths": 18,
  "initialBudget": 8000,
  "incomes": [
    { "name": "Client A retainer", "value": 3000, "frequency": 2 },
    { "name": "Project B", "value": 5000, "startDate": { "month": 3, "year": 2025 }, "frequency": 1 },
    { "name": "Project C", "value": 8000, "startDate": { "month": 7, "year": 2025 }, "frequency": 1 }
  ],
  "expenses": [
    { "name": "Living expenses", "value": 3500, "frequency": 2 },
    { "name": "Quarterly taxes", "value": 2000, "frequency": 3 }
  ]
}
```

## Family budget with kids

**"When can we afford to move to a bigger house?"**

Factor in growing family expenses and see when you'll have enough for a down payment.

```json
{
  "predictionMonths": 48,
  "initialBudget": 35000,
  "incomes": [
    { "name": "Parent 1 salary", "value": 6500, "frequency": 2 },
    { "name": "Parent 2 salary", "value": 4800, "frequency": 2 },
    { "name": "Tax refund", "value": 3500, "startDate": { "month": 4, "year": 2025 }, "frequency": 5 }
  ],
  "expenses": [
    { "name": "Current mortgage", "value": 2200, "frequency": 2 },
    { "name": "Childcare", "value": 1500, "frequency": 2, "endDate": { "month": 8, "year": 2027 } },
    { "name": "Other expenses", "value": 4000, "frequency": 2 },
    { "name": "College savings", "value": 800, "frequency": 2 }
  ]
}
```

Notice how cash flow improves when childcare ends.

## Retirement planning

**"Am I saving enough for retirement?"**

Model your current savings rate and see how much you'll have accumulated.

```json
{
  "predictionMonths": 120,
  "initialBudget": 75000,
  "incomes": [
    { "name": "Salary", "value": 7500, "frequency": 2 },
    { "name": "Annual bonus", "value": 12000, "frequency": 5 }
  ],
  "expenses": [
    { "name": "Living expenses", "value": 5500, "frequency": 2 },
    { "name": "401k contribution", "value": 1200, "frequency": 2 },
    { "name": "IRA contribution", "value": 500, "frequency": 2 }
  ]
}
```

This doesn't account for investment growth, but shows your contribution accumulation.

## Medical emergency planning

**"If I have a major medical expense, how long to recover?"**

See how a big one-time expense affects your timeline.

```json
{
  "predictionMonths": 24,
  "initialBudget": 15000,
  "incomes": [
    { "name": "Salary", "value": 5000, "frequency": 2 }
  ],
  "expenses": [
    { "name": "Normal expenses", "value": 3500, "frequency": 2 },
    { "name": "Medical emergency", "value": 10000, "startDate": { "month": 6, "year": 2025 }, "frequency": 1 }
  ]
}
```

Watch how long it takes to rebuild your emergency fund after the expense hits.

## Starting a side business

**"When will my side business replace my day job income?"**

Model growing side income and see when
