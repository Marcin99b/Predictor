---
id: contributing
slug: /contributing
title: Contributing
sidebar_position: 5
---

# Contributing

Want to help make financial planning easier? Here's how to contribute.

## What needs work

**Easy stuff (good for beginners):**

- Fix typos in docs
- Add more realistic examples
- Improve error messages
- Write tests for edge cases

**Medium stuff:**

- Add new API endpoints
- Improve validation logic
- Performance optimizations
- Better example data

**Hard stuff:**

- Frontend/UI (currently just an API)
- Database integration (currently just memory cache)
- Multi-currency support
- Inflation adjustments

Browse [issues](https://github.com/Marcin99b/Predictor/issues) to see what's open. Look for `good-first-issue` if you're new.

## Getting started

1. Fork the repo on GitHub
2. Clone your fork: `git clone https://github.com/YOUR-USERNAME/Predictor.git`
3. Create a branch: `git checkout -b fix-something-cool`
4. Make your changes
5. Test them: `dotnet test`
6. Push: `git push origin fix-something-cool`
7. Submit a pull request

## Development setup

You need .NET 8.

```bash
git clone https://github.com/Marcin99b/Predictor.git
cd Predictor/src
dotnet restore
dotnet build
dotnet test
dotnet run --project Predictor.Web
```

API runs at `https://localhost:7176`. Swagger docs at `/swagger`.

## Code style

Follow the existing patterns. There's an `.editorconfig` file that handles most formatting.

Key points:

- Use meaningful variable names
- Add tests for new features
- Keep methods small and focused
- Don't break existing tests

## Testing

```bash
# Run all tests
dotnet test

# Just integration tests
dotnet test src/Predictor.Tests.Integration

# Performance tests (need API running first)
dotnet test src/Predictor.Tests.Performance
```

Add tests for new features. Integration tests are often easier than unit tests for this kind of logic.

## What gets accepted

**Definitely yes:**

- Bug fixes
- Better documentation
- More test coverage
- Performance improvements
- New features from the roadmap

**Probably yes:**

- New API endpoints that make sense
- Better validation
- Code cleanup

**Probably no:**

- Breaking changes to existing API
- Features that make the API way more complex
- Stuff that doesn't relate to financial planning

When in doubt, open an issue first to discuss.

## Pull request process

1. Make sure tests pass
2. Update docs if you changed the API
3. Write a good description of what you did and why
4. Link to any related issues

I'll try to review PRs quickly. If something needs changes, I'll let you know.

## Ideas for contributions

**Documentation:**

- More real-world examples
- FAQ section
- Video tutorials
- API client examples in different languages

**Features:**

- Goal tracking endpoints
- Scenario comparison
- Data export (CSV, PDF)
- Webhook notifications when goals are hit

**Performance:**

- Database backend instead of memory cache
- Caching for common calculations
- Background processing for complex scenarios

**UI/Frontend:**

- Web dashboard
- Mobile app
- Interactive charts and graphs

**Developer experience:**

- Client SDKs (Python, JavaScript, etc.)
- Docker Compose setup
- Kubernetes manifests
- CLI tool

## Questions?

Open an issue or start a discussion on GitHub. I'm usually pretty responsive.

## Code of conduct

Please treat everyone with respect. We're building something useful together.
