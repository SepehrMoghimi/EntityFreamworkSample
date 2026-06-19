using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Http;

namespace Finshark.Infrastructure.Logging;

public class SplunkHttpClient : IHttpClient
{
    private readonly HttpClient _httpClient;

    public SplunkHttpClient(string token)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Splunk {token}");
    }

    public Task<HttpResponseMessage> PostAsync(string requestUri, Stream content, CancellationToken cancellationToken)
    {
        var streamContent = new StreamContent(content);
        streamContent.Headers.Add("Content-Type", "application/json");
        return _httpClient.PostAsync(requestUri, streamContent, cancellationToken);
    }

    public void Configure(IConfiguration configuration)
    {
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
