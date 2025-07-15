---
id: roadmap
slug: /roadmap
title: Roadmap
sidebar_position: 4
---

# Roadmap

What's working now, what's coming next.

## Current features

**Core prediction engine:**

- [x] Month-by-month budget calculations
- [x] Multiple payment frequencies (monthly, quarterly, etc.)
- [x] Time-limited payments (loans that end, contracts that expire)
- [x] One-time events (bonuses, emergencies)
- [x] Input validation with FluentValidation
- [x] Goal checking analytics
- [x] Prediction caching and retrieval

**API:**

- [x] REST endpoints with JSON
- [x] Swagger/OpenAPI documentation
- [x] Error handling with proper HTTP status codes
- [x] Memory caching for predictions
- [x] Health check endpoints

**Development:**

- [x] Integration tests with realistic scenarios
- [x] Performance testing with NBomber
- [x] Docker support
- [x] GitHub Actions CI/CD
- [x] Code coverage tracking

## Next up (3-6 months)

**Inflation adjustments** - Account for cost increases over time

- [ ] Automatic inflation on expenses
- [ ] Custom inflation rates per payment
- [ ] Regional inflation data

**Scenario comparison** - Compare different "what if" situations

- [ ] Side-by-side results
- [ ] Difference highlighting
- [ ] Sensitivity analysis

**Data export** - Get your data out

- [ ] CSV export
- [ ] PDF reports
- [ ] JSON backup/restore

**Better performance & scale**

- [ ] Database backend (replace memory cache)
- [ ] Rate limiting
- [ ] Background processing for complex calculations
- [ ] Caching layer improvements

## Planned features (6-12 months)

**Web dashboard** - Actual UI instead of just API

- [ ] Interactive charts and graphs
- [ ] Goal progress tracking
- [ ] Scenario builder interface
- [ ] Mobile-friendly design

**Multi-currency support** - Handle different currencies

- [ ] Exchange rate integration
- [ ] Multi-currency portfolios
- [ ] Currency conversion history

**Advanced analytics** - Smarter insights

- [ ] Risk analysis and confidence intervals
- [ ] Monte Carlo simulations
- [ ] Trend analysis
- [ ] Financial goal tracking alerts

**External integrations** - Connect to real data

- [ ] Economic data feeds (inflation, exchange rates)
- [ ] Enhanced API documentation with more examples
- [ ] SDK/client libraries for popular languages

## Maybe someday

**Mobile apps** - Native iOS/Android

- [ ] Offline sync
- [ ] Push notifications for goals
- [ ] Photo receipt scanning

**Investment modeling** - Beyond just cash

- [ ] Stock portfolio growth projections
- [ ] Retirement account modeling
- [ ] Real estate appreciation

**Advanced financial planning** - More sophisticated features

- [ ] Tax bracket optimization
- [ ] Retirement planning scenarios
- [ ] Insurance needs analysis

**Collaboration features** - Multi-user support

- [ ] Shared family budgets
- [ ] Financial advisor access
- [ ] Team/business planning

## Technical debt

**Code quality:**

- Better error handling
- More modular architecture
- Performance optimizations
- Security hardening

**Infrastructure:**

- Real database instead of memory cache
- Rate limiting
- User authentication
- Data backup

**Documentation:**

- More examples
- Video tutorials
- Client library guides
- Best practices

## How priorities work

Features get prioritized based on:

1. **User feedback** - What people actually ask for
2. **Usage patterns** - What endpoints get hit most
3. **Development effort** - Easy wins first
4. **Strategic value** - Features that unlock other features

## Want to influence this?

- [Open issues](https://github.com/Marcin99b/Predictor/issues) for feature requests
- [Start discussions](https://github.com/Marcin99b/Predictor/discussions) about use cases
- [Contribute code](./contributing) to move things faster
- Share how you're using Predictor

## What won't change

The core API will stay backwards compatible. Your existing integrations should keep working as new features get added.

The focus stays on practical financial planning, not complex investment modeling or tax software features.
