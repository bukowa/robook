global using NUnit.Framework;
global using Robook;
using com.omnesys.rapi;
using Rithmic;

namespace Robook.UnitTest;

public static class Categories {
    public const string RithmicConnectionTestServer  = "RithmicConnectionTest";
    public const string RithmicConnectionPaperServer = "RithmicConnectionPaper";
}

public class TestConnection {
    public string? Login             = Environment.GetEnvironmentVariable("RITHMIC_TEST_LOGIN");
    public string? Password          = Environment.GetEnvironmentVariable("RITHMIC_TEST_PASSWORD");
    public string  cParamsSourcePath = @"C:\Users\buk\Documents\RApiConfig";
    public bool    pluginMode        = false;
    public string  Server            = "Rithmic Test";
    public string  Gateway           = "Orangeburg";
    public string  Exchange          = "CME";
    public string  Symbol            = "ES";
    public string  LogFilePath       = "rithmic.test.log.txt";

    public CParams GetCParams() {
        var s = new CParamsSource(cParamsSourcePath);
        return s.CParamsBySystemName[Server][Gateway];
    }

    public Client NewPaperReadyClient() {
        Login      = Environment.GetEnvironmentVariable("RITHMIC_PAPER_LOGIN");
        Password   = Environment.GetEnvironmentVariable("RITHMIC_PAPER_PASSWORD");
        pluginMode = false;
        Gateway    = "Europe";
        Server     = "Rithmic Paper Trading";
        return NewReadyClient();
    }

    /// <summary>
    ///     Creates a new Rithmic client by logging in to all connections.
    /// </summary>
    /// <returns></returns>
    public Client NewReadyClient() {
        var cp = new LoginParams(GetCParams());
        cp.PlugInMode = pluginMode;
        cp.MarketDataConnection =
            new Connection(Login, Password, ConnectionId.MarketData);
        cp.HistoricalDataConnection =
            new Connection(Login, Password, ConnectionId.History);
        cp.TradingSystemConnection =
            new Connection(Login, Password, ConnectionId.TradingSystem);
        cp.PnlConnection = new Connection(Login, Password, ConnectionId.PnL);
        var client = new Client();
        var ce     = new CountdownEvent(4);
        var sub    = new Connection.AlertInfoHandler((c, a, dt) => { ce.Signal(); });
        cp.MarketDataConnection.OnLoginComplete     += sub;
        cp.HistoricalDataConnection.OnLoginComplete += sub;
        cp.TradingSystemConnection.OnLoginComplete  += sub;
        cp.PnlConnection.OnLoginComplete            += sub;
        cp.MarketDataConnection.OnLoginFailed       += sub;
        cp.HistoricalDataConnection.OnLoginFailed   += sub;
        cp.TradingSystemConnection.OnLoginFailed    += sub;
        cp.PnlConnection.OnLoginFailed              += sub;
        client.Login(cp);
        ce.Wait(TimeSpan.FromSeconds(15));
        Assert.That(client.MarketDataConnection.IsLoggedIn,     Is.True);
        Assert.That(client.HistoricalDataConnection.IsLoggedIn, Is.True);
        Assert.That(client.TradingSystemConnection.IsLoggedIn,  Is.True);
        Assert.That(client.PnlConnection.IsLoggedIn,            Is.True);
        return client;
    }
}