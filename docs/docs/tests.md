---
id: tests
slug: /tests
title: Running Tests
sidebar_position: 7
---

# Running Tests

```bash
# Performance tests  
cd src/Predictor.Tests.Performance
dotnet test
```

Performance tests validate that the API can handle high load scenarios. They're useful for ensuring calculations remain fast as complexity grows.

**Note:** Performance tests are marked with `[Ignore]` by default. Remove the ignore attribute to run them against a running instance of the API.
