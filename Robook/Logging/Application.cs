using Microsoft.Extensions.Logging;

namespace Robook.Logging;

/// <summary>
///  Global configuration for logging.
/// </summary>
public class Application {
    /// <summary>
    /// Used by the loggers in the application to create Loggers.
    /// </summary>
    public static ILoggerFactory LoggerFactory { get; set; } =
        Microsoft.Extensions.Logging.LoggerFactory.Create(
            builder => {
                builder.AddSimpleConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

    /// <summary>
    /// A convenience method to create a logger with the default configuration.
    /// </summary>
    /// <returns></returns>
    public static ILogger CreateDefaultLogger(Type type) {
        return LoggerFactory.CreateLogger($"{type.Namespace}.{type.Name}");
    }
}