# Configuration Guide

## Application Settings

The Predictor application is fully configurable through the `PredictorSettings` section in `appsettings.json`. All settings can be overridden using environment variables or other ASP.NET Core configuration sources.

### Available Settings

| Setting | Default | Description |
|---------|---------|-------------|
| `MaxCalculationPeriodMonths` | 36 | Maximum number of months to calculate predictions for |
| `DefaultInitialBudget` | 48750 | Default initial budget used in example data |
| `EnableExampleData` | true | Enable/disable the `/example-data` endpoint |
| `MaxAllowedCalculationPeriod` | 120 | Maximum allowed calculation period to prevent abuse (10 years) |

### Configuration Examples

#### appsettings.json
```json
{
  "PredictorSettings": {
    "MaxCalculationPeriodMonths": 24,
    "DefaultInitialBudget": 25000,
    "EnableExampleData": true,
    "MaxAllowedCalculationPeriod": 60
  }
}
```

#### Environment Variables
```bash
# Docker/Container deployment
PREDICTOR__MAXCALCULATIONPERIODMONTHS=24
PREDICTOR__DEFAULTINITIALBUDGET=25000
PREDICTOR__ENABLEEXAMPLEDATA=true
PREDICTOR__MAXALLOWEDCALCULATIONPERIOD=60

# Or use the ASPNETCORE prefix
ASPNETCORE_PREDICTORSETTINGS__MAXCALCULATIONPERIODMONTHS=24
```

#### Docker Compose
```yaml
version: '3.8'
services:
  predictor:
    image: predictor:latest
    environment:
      - PREDICTOR__MAXCALCULATIONPERIODMONTHS=24
      - PREDICTOR__DEFAULTINITIALBUDGET=25000
      - PREDICTOR__ENABLEEXAMPLEDATA=true
      - PREDICTOR__MAXALLOWEDCALCULATIONPERIOD=60
    ports:
      - "8080:8080"
```

### Environment-Specific Configuration

The application uses standard ASP.NET Core configuration layering:

1. **appsettings.json** - Base configuration
2. **appsettings.{Environment}.json** - Environment-specific overrides
3. **Environment variables** - Runtime overrides
4. **Command line arguments** - Highest priority

#### Development vs Production

- **Development**: Uses `appsettings.Development.json` with shorter calculation periods for faster testing
- **Production**: Uses `appsettings.json` with full calculation periods

### Configuration Validation

The application validates configuration at startup and uses sensible defaults if values are missing. The calculation period is automatically capped at `MaxAllowedCalculationPeriod` to prevent resource abuse.

### Why This Matters

✅ **Deployment Flexibility**: Change limits without code changes  
✅ **Environment-Specific**: Different settings for dev/staging/production  
✅ **Docker Ready**: Full support for container deployment  
✅ **Twelve-Factor**: Follows twelve-factor app configuration principles  
✅ **Security**: Sensitive settings can be injected via environment variables  
