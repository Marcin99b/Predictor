# 🔮 Predictor

> **When will you be able to afford that dream purchase?** Predictor helps you answer this question with precision.

Predictor is a powerful financial forecasting API that simulates your budget months and years into the future. Whether you're planning to buy a house, a new car, or wondering about your financial stability, Predictor gives you the clarity you need to make informed decisions.

## ✨ Why Predictor?

**The Problem**: Traditional budgeting apps show you where your money went, but they don't help you plan for the future. You're left wondering: *"When will I have enough for a down payment? Can I afford that vacation next year? What if I lose my job?"*

**The Solution**: Predictor runs sophisticated simulations of your financial future, accounting for recurring income, planned expenses, and one-time events. Get answers to your "when will I be able to afford..." questions with confidence.

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK
- Docker (optional, for containerized development)

### Run in 30 seconds

```bash
git clone https://github.com/Marcin99b/Predictor.git
cd predictor/src
dotnet run --project Predictor.Web
```

Navigate to `https://localhost:7176/swagger` to explore the API.

### Try it immediately

The API comes with rich example data pre-loaded. Make your first prediction:

```bash
curl -X GET "https://localhost:7176/example-data"
curl -X POST "https://localhost:7176/calc" \
  -H "Content-Type: application/json" \
  -d @example-data.json
```

## 💡 Core Features

### 🎯 **Smart Budget Forecasting**

- Simulate your finances up to 3 years into the future
- Account for salary changes, loan payments, and irregular expenses
- See exactly when you'll reach financial milestones

### 📊 **Flexible Income & Expense Modeling**

- **Recurring payments**: Salary, rent, subscriptions (monthly, quarterly, annually)
- **One-time events**: Tax refunds, bonuses, emergency repairs
- **Time-limited items**: Loan payments that end, temporary contract work

### 🔄 **Real-time Calculations**

- Lightning-fast API responses
- RESTful design for easy integration
- JSON-based data exchange

## 📖 How It Works

Predictor uses a simple but powerful model:

1. **Start with your current budget** - How much money do you have today?
2. **Define your income streams** - Salary, side hustles, investments
3. **List your expenses** - Both fixed costs and planned purchases
4. **Get month-by-month predictions** - See your financial trajectory

### Example Scenario

```json
{
  "initialBudget": 48750,
  "startCalculationMonth": { "month": 7, "year": 2025 },
  "incomes": [
    {
      "name": "Primary Salary",
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
```

**Result**: Month-by-month breakdown showing when you'll have enough for major purchases.

## 🛠 API Reference

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/example-data` | Get sample data to try the API |
| `POST` | `/calc` | Run financial prediction simulation |

### Data Models

**CalculateInput**

- `initialBudget` (decimal) - Your starting amount
- `startCalculationMonth` (MonthDate) - When to begin simulation
- `incomes` (PaymentItem[]) - All income sources
- `outcomes` (PaymentItem[]) - All expenses

**PaymentItem**

- `name` (string) - Description of the payment
- `value` (decimal) - Amount per occurrence
- `startDate` (MonthDate) - When it begins
- `recurringConfig` (optional) - How often it repeats

Full API documentation available at `/swagger` when running locally.

## 🌟 Real-World Use Cases

### 🏠 **Home Buying Planning**

*"I want to buy a $400k house. When will I have enough for a 20% down payment?"*

Model your savings rate, planned bonuses, and see exactly when you'll reach $80k.

### 🚗 **Vehicle Upgrade Timeline**

*"My car lease ends in 18 months. Can I afford to buy instead of lease?"*

Factor in your lease end date, expected car prices, and available cash flow.

### 💰 **Emergency Fund Goals**

*"I want 6 months of expenses saved. How long will it take?"*

Input your monthly costs and savings rate to get a precise timeline.

### 🎓 **Education Investment**

*"I want to do an MBA in 3 years. Will I have enough for tuition?"*

Plan for program costs, lost income, and required savings.

## 🗺 Roadmap

### 🎯 **Phase 1: Foundation** (Q3 2025)

- [x] Basic budget calculation engine
- [x] Recurring payment support  
- [x] REST API with Swagger documentation
- [ ] Inflation-adjusted calculations
- [ ] Enhanced API documentation with examples
- [ ] Docker Compose development environment

### 🚀 **Phase 2: Core Features** (Q4 2025)

- [ ] Financial goals tracking and achievement prediction
- [ ] "What-if" scenario comparison engine
- [ ] Goal-based recommendations
- [ ] Multi-currency support

### ⚡ **Phase 3: Performance & Scale** (Q1 2026)

- [ ] Redis-compatible caching layer
- [ ] Background processing for complex calculations
- [ ] Performance benchmarking suite
- [ ] Rate limiting and API security

### 📊 **Phase 4: Analytics & Intelligence** (Q2 2026)

- [ ] Historical data storage and trend analysis
- [ ] External data integration (exchange rates, market data)
- [ ] Smart spending optimization suggestions
- [ ] Risk analysis and confidence intervals

### 🤖 **Phase 5: Advanced Features** (Q3+ 2026)

- [ ] Monte Carlo simulation engine for uncertainty modeling
- [ ] Machine learning-based financial predictions
- [ ] Advanced investment portfolio modeling
- [ ] Integration marketplace for financial services

## 🤝 Contributing

We welcome contributions! Predictor is designed as a learning project for .NET performance, scalability, and financial modeling.

### Getting Started

#### 🚀 Quick Start for Contributors

**Step 1: Pick a task**

1. Go to [Issues](https://github.com/Marcin99b/Predictor/issues)
2. Look for tasks with `good-first-issue` label if you're new
3. Read the issue description and comment "I'd like to work on this" to claim it

**Step 2: Set up your workspace**

1. **Fork the repository**: Click the "Fork" button at the top of this page
2. **Clone YOUR fork** (not the original repo):

   ```bash
   git clone https://github.com/YOUR-USERNAME/Predictor.git
   cd Predictor
   ```

3. **Create a new branch** with this naming pattern:

   ```bash
   # Format: ISSUE-NUMBER-short-description
   # Examples:
   git checkout -b 1-add-mit-license
   git checkout -b 5-health-check-endpoint
   git checkout -b 12-performance-tests
   ```

**Step 3: Make your changes**

1. Work on your task in your branch
2. Test your changes locally:

   ```bash
   cd src
   dotnet run --project Predictor.Web
   ```

3. Commit with clear messages:

   ```bash
   git add .
   git commit -m "Add MIT license file"
   ```

**Step 4: Submit your work**

1. **Push to YOUR fork**:

   ```bash
   git push origin YOUR-BRANCH-NAME
   ```

2. **Create Pull Request**:
   - Go to your fork on GitHub
   - Click "Pull Request" button
   - Make sure it's going from your branch to `main` branch of original repo
   - In the PR description, add: `Closes #ISSUE-NUMBER` (this auto-closes the issue)
   - Example: `Closes #1` or `Fixes #5`

**Step 5: Wait for review**

- We'll review your code and might ask for changes
- Make requested changes in the same branch
- Push again - the PR updates automatically

#### 📚 Need Help?

- 🆘 **Stuck?** Comment on the issue you're working on
- 💬 **Questions?** Use [GitHub Discussions](https://github.com/Marcin99b/Predictor/discussions)
- 📖 **Git help?** Check [GitHub's Git Handbook](https://guides.github.com/introduction/git-handbook/)

#### 🎯 Contribution Tips

- **One issue = one PR** - don't mix multiple features
- **Small commits** - easier to review and understand
- **Test your changes** - make sure everything still works
- **Be patient** - code review takes time but makes the project better

### Areas We Need Help

- 🔧 **Backend Development**: .NET 8, performance optimization
- 📊 **Financial Modeling**: Advanced calculation algorithms  
- 🦀 **Rust Components**: High-performance caching and data processing
- 📚 **Documentation**: API guides, tutorials, examples
- 🧪 **Testing**: Unit tests, integration tests, performance benchmarks

## 💻 Technology Stack

- **Backend**: .NET 8, ASP.NET Core, Swagger/OpenAPI
- **Future Components**: Rust (for high-performance modules)
- **Deployment**: Docker, Linux containers
- **Integration**: RESTful API, JSON data exchange

## 📋 Development Setup

### Local Development

```bash
# Clone the repository
git clone https://github.com/Marcin99b/Predictor.git
cd predictor

# Run with .NET CLI
cd src
dotnet run --project Predictor.Web

# Or with Docker
docker build -t predictor .
docker run -p 8080:8080 predictor
```

### Development with Docker Compose (Coming Soon)

```bash
docker-compose up -d  # Includes Redis, PostgreSQL, and monitoring
```

## 🔗 Integration

*Building a finance app? We'd love to integrate! Open an issue to discuss.*

## 🙋‍♀️ Support & Community

- 🐛 **Bug Reports**: [GitHub Issues](https://github.com/Marcin99b/Predictor/issues)
- 💡 **Feature Requests**: [GitHub Discussions](https://github.com/Marcin99b/Predictor/discussions)

---

<div align="center">

**Made with ❤️ for developers who care about financial planning**

[⭐ Star this repo](https://github.com/Marcin99b/Predictor) if you find it useful!

*Predictor is a learning project focused on .NET performance, avaliability, and scalability.*

</div>
