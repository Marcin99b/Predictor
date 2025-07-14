using Microsoft.Extensions.Caching.Memory;
using Predictor.Web.Models;

namespace Predictor.Web.Integrations;

public class CacheRepository(IMemoryCache memoryCache)
{
    public void Set_PredictionResult(PredictionResult data)
    {
        var key = this.BuildKey(nameof(PredictionResult), data.Id);
        memoryCache.Set(key, data);
    }

    public PredictionResult? Get_PredictionResult(Guid id)
    {
        var key = this.BuildKey(nameof(PredictionResult), id);
        return memoryCache.Get<PredictionResult>(key);
    }

    private string BuildKey(string name, Guid id) => $"{name}_{id}";
}
