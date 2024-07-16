using Microsoft.Extensions.Logging;

namespace Rithmic.Core;

/// <summary>
/// This class is used to provide a logger to the rest of the application.
/// </summary>
public static class LoggingService {
    /// <summary>
    /// The logger that will be used to log messages.
    /// </summary>
    public static ILogger? Logger { get; private set; }

    /// <summary>
    /// Method to set the logger.
    /// </summary>
    public static void SetLogger(ILogger? logger) {
        Logger = logger;
    }
}