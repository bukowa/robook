global using NUnit.Framework;
global using Rithmic;
using com.omnesys.rapi;


/// <summary>
/// Configuration for Rithmic Tests.
/// </summary>
public static class Config {
    public static string RApiConfigPath = @"C:\Users\buk\Documents\RApiConfig";
    
    public static string RTestLogin     = Environment.GetEnvironmentVariable("RITHMIC_TEST_LOGIN");
    public static string RTestPassword  = Environment.GetEnvironmentVariable("RITHMIC_TEST_PASSWORD");
    public static string RTestServer    = "Rithmic Test";
    public static string RTestGateway   = "Orangeburg";
    
    public static string RPaperLogin      = Environment.GetEnvironmentVariable("RITHMIC_PAPER_LOGIN");
    public static string RPaperPassword   = Environment.GetEnvironmentVariable("RITHMIC_PAPER_PASSWORD");
    public static string RPaperServer     = "Rithmic Paper Trading";
    public static string RPaperGateway    = "Europe";
}

/// <summary>
/// Testing Categories for NUnit.
/// </summary>
public static class Categories {
    public const string RConnectionTest  = "RithmicConnectionTest";
    public const string RConnectionPaper = "RithmicConnectionPaper";
}

/// <summary>
/// Helper class to create a connection to Rithmic Test Server.
/// </summary>
public class TestServerConnection : BaseConnection {
    public override string? Login
    {
        get => Config.RTestLogin ?? base.Login;
        set => base.Login = value;
    }
    public override string? Password
    {
        get => Config.RTestPassword ?? base.Password;
        set => base.Password = value;
    }
    public override string Server
    {
        get => Config.RTestServer;
        set => base.Server = value;
    }
    public override string Gateway
    {
        get => Config.RTestGateway;
        set => base.Gateway = value;
    }
}

/// <summary>
/// Helper class to create a connection to Rithmic Paper Trading Server.
/// </summary>
public class TestPaperConnection : BaseConnection {
    public override string? Login
    {
        get => Config.RPaperLogin ?? base.Login;
        set => base.Login = value;
    }
    public override string? Password
    {
        get => Config.RPaperPassword ?? base.Password;
        set => base.Password = value;
    }
    public override string Server
    {
        get => Config.RPaperServer;
        set => base.Server = value;
    }
    public override string Gateway
    {
        get => Config.RPaperGateway;
        set => base.Gateway = value;
    }
    public override bool PluginMode
    {
        get => true;
        set => base.PluginMode = value;
    }
}

/// <summary>
/// Base class for Rithmic connections used in tests.
/// </summary>
public class BaseConnection {
    public virtual string? Login      { get; set; }
    public virtual string? Password   { get; set; }
    public virtual string  Server     { get; set; }
    public virtual string  Gateway    { get; set; }
    public virtual bool    PluginMode { get; set; }

    public string CParamsSourcePath = Config.RApiConfigPath;
    public string Exchange          = "CME";
    public string Symbol            = "ES";
    public string LogFilePath       = "rithmic.test.log.txt";

    public CParams GetCParams() {
        var s = new CParamsSource(CParamsSourcePath);
        return s.CParamsBySystemName[Server][Gateway];
    }

    public LoginParams GetClientParams() {
        return new LoginParams(GetCParams()) {
            PlugInMode = PluginMode,
            MarketDataConnection =
                new Connection(Login, Password, ConnectionId.MarketData),
            HistoricalDataConnection =
                new Connection(Login, Password, ConnectionId.History),
            TradingSystemConnection =
                new Connection(Login, Password, ConnectionId.TradingSystem),
            PnlConnection = new Connection(Login, Password, ConnectionId.PnL)
        };
    }

    public Client GetClient() {
        var client = new Client();
        return client;
    }

    public Client GetReadyClient() {
        var client       = GetClient();
        var clientParams = GetClientParams();
        var success      = client.LoginAndWait(clientParams);
        Assert.That(success, Is.True);
        return client;
    }
}