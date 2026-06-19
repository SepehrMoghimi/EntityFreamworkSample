using Finshark.Infrastructure.Logging;
using Serilog;

namespace Finshark.Application.Logging;

public static class ApplicationSerilog
{
    public static ILogger CreateBootstrapLogger(string splunkToken)
    {
        return new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
                requestUri: "http://localhost:8000/en-US/app/launcher/home",
                bufferBaseFileName: "./logs/buffer",
                batchFormatter: new Serilog.Sinks.Http.BatchFormatters.ArrayBatchFormatter(),
                httpClient: new SplunkHttpClient(splunkToken)
            )
            .CreateLogger();
    }
}
