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
        Application.Run(new Main());
    }
}