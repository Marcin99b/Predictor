namespace Predictor.Web.Models;

public class PredictorSettings
{
    public const string SectionName = "PredictorSettings";
    
    /// <summary>
    /// Maximum number of months to calculate predictions for
    /// </summary>
    public int MaxCalculationPeriodMonths { get; set; } = 36;
    
    /// <summary>
    /// Default initial budget for examples
    /// </summary>
    public decimal DefaultInitialBudget { get; set; } = 48_750m;
    
    /// <summary>
    /// Enable/disable example data endpoint
    /// </summary>
    public bool EnableExampleData { get; set; } = true;
    
    /// <summary>
    /// Maximum allowed calculation period to prevent abuse
    /// </summary>
    public int MaxAllowedCalculationPeriod { get; set; } = 120; // 10 years
}
