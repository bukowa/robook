global using rapi = com.omnesys.rapi;
using com.omnesys.rapi;
using NUnit.Framework;
using Rithmic.Core;
namespace Rithmic.Core.UnitTest;

/// <summary>
/// Configuration for Rithmic Tests.
/// </summary>
public static class TestConfig {
    public static readonly string RApiConfigPath = @"C:\Users\buk\Documents\RApiConfig";

    public static readonly string? RTestLogin    = Environment.GetEnvironmentVariable("RITHMIC_TEST_LOGIN");
    public static readonly string? RTestPassword = Environment.GetEnvironmentVariable("RITHMIC_TEST_PASSWORD");
    public static readonly string  RTestServer   = "Rithmic Test";
    public static readonly string  RTestGateway  = "Orangeburg";

    public static readonly string? RPaperLogin    = Environment.GetEnvironmentVariable("RITHMIC_PAPER_LOGIN");
    public static readonly string? RPaperPassword = Environment.GetEnvironmentVariable("RITHMIC_PAPER_PASSWORD");
    public static readonly string  RPaperServer   = "Rithmic Paper Trading";
    public static readonly string  RPaperGateway  = "Europe";
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
    public override string? Login {
        get => TestConfig.RTestLogin ?? base.Login;
        set => base.Login = value;
    }

    public override string? Password {
        get => TestConfig.RTestPassword ?? base.Password;
        set => base.Password = value;
    }

    public override string Server {
        get => TestConfig.RTestServer;
        set => base.Server = value;
    }

    public override string Gateway {
        get => TestConfig.RTestGateway;
        set => base.Gateway = value;
    }
}

/// <summary>
/// Helper class to create a connection to Rithmic Paper Trading Server.
/// </summary>
public class TestPaperConnection : BaseConnection {
    public override string? Login {
        get => TestConfig.RPaperLogin ?? base.Login;
        set => base.Login = value;
    }

    public override string? Password {
        get => TestConfig.RPaperPassword ?? base.Password;
        set => base.Password = value;
    }

    public override string Server {
        get => TestConfig.RPaperServer;
        set => base.Server = value;
    }

    public override string Gateway {
        get => TestConfig.RPaperGateway;
        set => base.Gateway = value;
    }

    public override bool PluginMode {
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

    public string CParamsSourcePath = TestConfig.RApiConfigPath;
    public string Exchange          = "CME";
    public string Symbol            = "ES";
    public string LogFilePath       = "rithmic.test.log.txt";

    public CParams GetCParams() {
        var s = new CParamsSource(CParamsSourcePath);
        return s.CParamsDict[Server][Gateway];
    }
}