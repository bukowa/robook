using System.Text.RegularExpressions;

namespace Rithmic.Core;

/// <summary>
///     Represents a set of parameters used for configuring
///     connections and logins in the Rithmic system.
/// </summary>
// ReSharper disable once RedundantNameQualifier
public class CParams : rapi.REngineParams {
    /// <summary>
    ///     System name like "Rithmic Paper Trading".
    /// </summary>
    public required string SystemName { get; init; }

    /// <summary>
    ///     Gateway name like "Europe".
    /// </summary>
    public required string GatewayName { get; init; }

    /// <summary>
    ///     Gets or sets the administrative connection point.
    /// </summary>
    public required string AdmCnnctPt { get; init; }

    /// <summary>
    ///     Gets or sets the connection point for login repository.
    /// </summary>
    public required string SCnnctPt { get; init; }

    /// <summary>
    ///     Gets or sets the market data connection point.
    /// </summary>
    public required string SMdCnnctPt { get; init; }

    /// <summary>
    ///     Gets or sets the historical data connection point.
    /// </summary>
    public required string SIhCnnctPt { get; init; }

    /// <summary>
    ///     Gets or sets the trading system connection point.
    /// </summary>
    public required string STsCnnctPt { get; init; }

    /// <summary>
    ///     Gets or sets the PnL connection point.
    /// </summary>
    public required string SPnLCnnctPt { get; init; }
}

/// <summary>
/// Base class for CParams exceptions.
/// </summary>
public class ExceptionCParams(string message) : Exception(message);

/// <summary>
/// Exception thrown when the path does not exist.
/// </summary>
public class ExceptionCParamsSourcePathDoesNotExist(string path)
    : ExceptionCParams($"CParams source path does not exist: {path}");

/// <summary>
/// Exception thrown when the CParams System does not exist.
/// </summary>
public class ExceptionCParamsSystemDoesNotExist(string systemName)
    : ExceptionCParams($"CParams system does not exist: {systemName}");

/// <summary>
/// Exception thrown when the CParams Gateway does not exist.
/// </summary>
public class ExceptionCParamsGatewayDoesNotExist(string systemName, string gatewayName)
    : ExceptionCParams($"CParams gateway does not exist: {systemName} {gatewayName}");

///
/// <summary>
///     Responsible for parsing the connection files.
///     Exposes the CParams instances by system name and gateway name.
/// </summary>
///
public class CParamsSource {
    /// <summary>
    ///     Holds the CParams instances by system name and gateway name.
    ///     Example: CParamsBySystemName["Rithmic Paper Trading"]["Europe"]
    /// </summary>
    public Dictionary<string, Dictionary<string, CParams>> CParamsDict { get; } = new();

    /// <summary>
    ///     Creates a new instance of the Source class for the given path.
    /// </summary>
    /// <param name="dirPath">Path to the connection files (directory). </param>
    /// <param name="searchPattern">Search pattern for the connection files.</param>
    /// 
    public CParamsSource(string dirPath, string searchPattern = "*_connection_params.txt") {
        if (!Directory.Exists(dirPath)) {
            throw new ExceptionCParamsSourcePathDoesNotExist(dirPath);
        }

        var files = Directory.GetFiles(dirPath, searchPattern);

        foreach (var file in files) {
            var cParams = ParseFile(file);
            CParamsDict.TryAdd(cParams.SystemName, new Dictionary<string, CParams>());
            CParamsDict[cParams.SystemName].Add(cParams.GatewayName, cParams);
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CParamsSource"/>.
    /// </summary>
    public CParamsSource() {
    }
    
    /// <summary>
    /// Adds the given <see cref="CParams"/> instance to the source.
    /// </summary>
    public void AddCParams(CParams cParams) {
        CParamsDict.TryAdd(cParams.SystemName, new Dictionary<string, CParams>());
        CParamsDict[cParams.SystemName].Add(cParams.GatewayName, cParams);
    }
    
    /// <summary>
    ///   Gets the CParams instance for the given system name and gateway name.
    /// </summary>
    /// <param name="systemName"> System name like "Rithmic Paper Trading". </param>
    /// <param name="gatewayName"> Gateway name like "Europe". </param>
    /// <returns> CParams instance or null if not found. </returns>
    public CParams GetCParams(string systemName, string gatewayName) {
        if (!CParamsDict.TryGetValue(systemName, out var value)) {
            throw new ExceptionCParamsSystemDoesNotExist(systemName);
        }

        if (!value.ContainsKey(gatewayName)) {
            throw new ExceptionCParamsGatewayDoesNotExist(systemName, gatewayName);
        }

        return CParamsDict[systemName][gatewayName];
    }

    /// <summary>
    /// Parses the given <see cref="System.IO.StreamReader"/> content and returns the CParams instance.
    /// </summary>
    public static CParams Parse(StreamReader reader) => Parse(reader.ReadToEnd());

    /// <summary>
    /// Parses the given string content and returns the CParams instance.
    /// </summary>
    public static CParams Parse(string content) {
        var cParams = new CParams {
            SystemName  = SystemNameRegex.Match(content).Groups[1].Value,
            GatewayName = GatewayNameRegex.Match(content).Groups[1].Value,
            AdmCnnctPt  = AdmCnnctPtRegex.Match(content).Groups[1].Value,
            SCnnctPt    = SCnnctPtRegex.Match(content).Groups[1].Value,
            SMdCnnctPt  = SMdCnnctPtRegex.Match(content).Groups[1].Value,
            SIhCnnctPt  = SIhCnnctPtRegex.Match(content).Groups[1].Value,
            STsCnnctPt  = STsCnnctPtRegex.Match(content).Groups[1].Value,
            SPnLCnnctPt = SPnLCnnctPtRegex.Match(content).Groups[1].Value,
            DomainName  = DomainNameRegex.Match(content).Groups[1].Value,
            LoggerAddr  = LoggerAddrRegex.Match(content).Groups[1].Value,
            DmnSrvrAddr = DmnSrvrAddrRegex.Match(content).Groups[1].Value,
            LicSrvrAddr = LicSrvrAddrRegex.Match(content).Groups[1].Value,
            LocBrokAddr = LocBrokAddrRegex.Match(content).Groups[1].Value
        };
        return cParams;
    }

    /// <summary>
    ///     Parses the given file and returns the CParams instance.
    /// </summary>
    /// <param name="filePath">Path to the connection file.</param>
    /// <returns></returns>
    // ReSharper disable once MemberCanBePrivate.Global
    public static CParams ParseFile(string filePath) {
        var content = File.ReadAllText(filePath);
        var cParams = new CParams {
            SystemName  = SystemNameRegex.Match(content).Groups[1].Value,
            GatewayName = GatewayNameRegex.Match(content).Groups[1].Value,
            AdmCnnctPt  = AdmCnnctPtRegex.Match(content).Groups[1].Value,
            SCnnctPt    = SCnnctPtRegex.Match(content).Groups[1].Value,
            SMdCnnctPt  = SMdCnnctPtRegex.Match(content).Groups[1].Value,
            SIhCnnctPt  = SIhCnnctPtRegex.Match(content).Groups[1].Value,
            STsCnnctPt  = STsCnnctPtRegex.Match(content).Groups[1].Value,
            SPnLCnnctPt = SPnLCnnctPtRegex.Match(content).Groups[1].Value,
            DomainName  = DomainNameRegex.Match(content).Groups[1].Value,
            LoggerAddr  = LoggerAddrRegex.Match(content).Groups[1].Value,
            DmnSrvrAddr = DmnSrvrAddrRegex.Match(content).Groups[1].Value,
            LicSrvrAddr = LicSrvrAddrRegex.Match(content).Groups[1].Value,
            LocBrokAddr = LocBrokAddrRegex.Match(content).Groups[1].Value
        };
        return cParams;
    }

    private static readonly Regex SystemNameRegex  = new(@"\s*System\s*Name\s*:\s*(\S.*)");
    private static readonly Regex GatewayNameRegex = new(@"\s*Gateway\s*Name\s*:\s*(\S.*)");
    private static readonly Regex SCnnctPtRegex    = new(@"\s*sCnnctPt\s*:\s*(\S+)");
    private static readonly Regex SMdCnnctPtRegex  = new(@"\s*sMdCnnctPt\s*:\s*(\S+)");
    private static readonly Regex SIhCnnctPtRegex  = new(@"\s*sIhCnnctPt\s*:\s*(\S+)");
    private static readonly Regex STsCnnctPtRegex  = new(@"\s*sTsCnnctPt\s*:\s*(\S+)");
    private static readonly Regex SPnLCnnctPtRegex = new(@"\s*sPnLCnnctPt\s*:\s*(\S+)");
    private static readonly Regex AdmCnnctPtRegex  = new(@"\s*REngineParams\.AdmCnnctPt\s*:\s*(\S+)");
    private static readonly Regex DomainNameRegex  = new(@"\s*REngineParams\.DomainName\s*:\s*(\S+)");
    private static readonly Regex LoggerAddrRegex  = new(@"\s*REngineParams\.LoggerAddr\s*:\s*(\S+)");
    private static readonly Regex DmnSrvrAddrRegex = new(@"\s*REngineParams\.DmnSrvrAddr\s*:\s*(\S+)");
    private static readonly Regex LocBrokAddrRegex = new(@"\s*REngineParams\.LocBrokAddr\s*:\s*(\S+)");
    private static readonly Regex LicSrvrAddrRegex = new(@"\s*REngineParams\.LicSrvrAddr\s*:\s*(\S+)");
}