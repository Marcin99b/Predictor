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

        predictions.MapPost("/", (PredictionRequest request, IMediator mediator) 
            => mediator.Send(request));

        predictions.MapPut("/{id:guid}", ([FromRoute] Guid id, PredictionRequest request, IMediator mediator) 
            => mediator.Send(request with { PutId = id }));

        predictions.MapGet("/example", () 
            => ExampleData.CalculateInputExample);

        predictions.MapGet("/{id:guid}", ([FromRoute] Guid id, CacheRepository cache) 
            => Task.FromResult(cache.Get_PredictionResult(id)));

        predictions.MapGet("/{id:guid}/summary", ([FromRoute] Guid id, CacheRepository cache)
            => Task.FromResult(cache.Get_PredictionResult(id)?.Summary));

        predictions.MapGet("/{id:guid}/months", ([FromRoute] Guid id, CacheRepository cache)
            => Task.FromResult(cache.Get_PredictionResult(id)?.Months));

        return apiV1;
    }
}
