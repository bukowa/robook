using System.Text.Json;
using Microsoft.Extensions.Logging;
using Robook.Accounts;
namespace Robook;

internal static class Program {
	/// <summary>
	///     The main entry point for the application.
	/// </summary>
	[STAThread]
    private static void Main() {
        ApplicationConfiguration.Initialize();
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        var logger = LoggerFactory.Create(builder => {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddJsonConsole(options => {
                options.JsonWriterOptions = new JsonWriterOptions {
                    Indented = false,
                    MaxDepth = 2
                };
                options.UseUtcTimestamp = true;
            });
            builder.AddSimpleConsole();
        }).CreateLogger("Robook");
        Rithmic.LoggingService.SetLogger(logger);
        Application.Run(new Main());
    }
}