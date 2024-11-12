using System.Net;

namespace MonitoringDemo.Endpoints;

public class EndpointFilter : IEndpointFilter
{
    public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception e)
        {
            var randomNumber = Random.Shared.Next(0, 100);

            return randomNumber switch
            {
                < 20 => Results.NotFound(),
                < 40 => Results.BadRequest(),
                < 60 => Results.StatusCode(statusCode: 403),
                < 80 => Results.StatusCode(statusCode: 401),
                _ => Results.StatusCode(statusCode: 500)
            };
        }
    }
}