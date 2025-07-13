using MediatR;
using Predictor.Web.Models;

namespace Predictor.Web;

public static class EndpointsExtensions
{
    public static RouteGroupBuilder MapPredictionsV1(this RouteGroupBuilder apiV1)
    {
        var predictions = apiV1.MapGroup("/predictions");
        predictions.MapPost("/", (PredictionRequest request, IMediator mediator) => mediator.Send(request));
        predictions.MapGet("/example", () => ExampleData.CalculateInputExample);

        return apiV1;
    }
}
