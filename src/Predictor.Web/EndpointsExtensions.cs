using MediatR;
using Microsoft.AspNetCore.Mvc;
using Predictor.Web.Integrations;
using Predictor.Web.Models;

namespace Predictor.Web;

public static class EndpointsExtensions
{
    public static RouteGroupBuilder MapPredictionsV1(this RouteGroupBuilder apiV1)
    {
        var predictions = apiV1.MapGroup("/predictions");

        _ = predictions.MapPost("/", (PredictionRequest request, IMediator mediator)
            => mediator.Send(request));

        _ = predictions.MapPut("/{id:guid}", ([FromRoute] Guid id, PredictionRequest request, IMediator mediator)
            => mediator.Send(request with { PutId = id }));

        _ = predictions.MapGet("/example", ()
            => ExampleData.CalculateInputExample);

        _ = predictions.MapGet("/{id:guid}", ([FromRoute] Guid id, CacheRepository cache)
            => Task.FromResult(cache.Get_PredictionResult(id)));

        _ = predictions.MapGet("/{id:guid}/summary", ([FromRoute] Guid id, CacheRepository cache)
            => Task.FromResult(cache.Get_PredictionResult(id)?.Summary));

        _ = predictions.MapGet("/{id:guid}/months", ([FromRoute] Guid id, CacheRepository cache)
            => Task.FromResult(cache.Get_PredictionResult(id)?.Months));

        return apiV1;
    }

    public static RouteGroupBuilder MapAnalyticsV1(this RouteGroupBuilder apiV1)
    {
        var analytics = apiV1.MapGroup("/analytics");

        _ = analytics.MapPost("/check-goal", (CheckGoalRequest request, IMediator mediator) => mediator.Send(request));

        return apiV1;
    }
}
