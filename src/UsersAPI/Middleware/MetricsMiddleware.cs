using System.Diagnostics;
using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;

namespace UsersAPI.Middleware;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAmazonCloudWatch _cw;
    private const string Namespace = "FCG/ApiMetrics";
    private const string ServiceName = "fcg-users-api";

    public MetricsMiddleware(RequestDelegate next, IAmazonCloudWatch cw)
    {
        _next = next;
        _cw = cw;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        var route = context.Request.Path.ToString();
        var status = context.Response.StatusCode;
        var now = DateTime.UtcNow;

        var dimensions = new List<Dimension>
        {
            new() { Name = "Service", Value = ServiceName },
            new() { Name = "Route", Value = route }
        };

        var data = new List<MetricDatum>
        {
            new() { MetricName = "RequestCount", Value = 1, Unit = StandardUnit.Count, Dimensions = dimensions },
            new() { MetricName = "Latency", Value = sw.ElapsedMilliseconds, Unit = StandardUnit.Milliseconds, Dimensions = dimensions }
        };

        if (status >= 500)
            data.Add(new MetricDatum { MetricName = "Http5xx", Value = 1, Unit = StandardUnit.Count, Dimensions = dimensions });
        else if (status >= 400)
            data.Add(new MetricDatum { MetricName = "Http4xx", Value = 1, Unit = StandardUnit.Count, Dimensions = dimensions });

        try
        {
            await _cw.PutMetricDataAsync(new PutMetricDataRequest
            {
                Namespace = Namespace,
                MetricData = data
            });
        }
        catch
        {
            // Non-critical - never fail a request due to metrics
        }
    }
}
