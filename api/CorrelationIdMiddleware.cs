using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Try to get the correlation ID from the request header or generate a new one
        string correlationId = context.Request.Headers.ContainsKey(CorrelationIdHeader)
            ? context.Request.Headers[CorrelationIdHeader].ToString()
            : Guid.NewGuid().ToString();

        // Set the correlation ID to both request and response
        context.Items[CorrelationIdHeader] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
