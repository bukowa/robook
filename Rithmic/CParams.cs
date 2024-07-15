using System.Text.RegularExpressions;
using com.omnesys.rapi;

namespace Rithmic;

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
    ///     Path to the connection files.
    /// </summary>
    public string DirPath { get; }

    /// <summary>
    ///     Holds the CParams instances by system name and gateway name.
    ///     Example: CParamsBySystemName["Rithmic Paper Trading"]["Europe"]
    /// </summary>
    public Dictionary<string, Dictionary<string, CParams>> CParamsBySystemName { get; } = new();

    /// <summary>
    ///   Gets the CParams instance for the given system name and gateway name.
    /// </summary>
    /// <param name="systemName"> System name like "Rithmic Paper Trading". </param>
    /// <param name="gatewayName"> Gateway name like "Europe". </param>
    /// <returns> CParams instance or null if not found. </returns>
    public CParams GetCParams(string systemName, string gatewayName) {
        if (!CParamsBySystemName.TryGetValue(systemName, out var value)) {
            throw new ExceptionCParamsSystemDoesNotExist(systemName);
        }

        if (!value.ContainsKey(gatewayName)) {
            throw new ExceptionCParamsGatewayDoesNotExist(systemName, gatewayName);
        }

        return CParamsBySystemName[systemName][gatewayName];
    }

    /// <summary>
    ///     Creates a new instance of the Source class for the given path.
    /// </summary>
    /// <param name="dirPath">Path to the connection files.</param>
    /// 
    public CParamsSource(string dirPath) {
        DirPath = dirPath;

        if (!Directory.Exists(dirPath)) {
            throw new ExceptionCParamsSourcePathDoesNotExist(dirPath);
        }

        var files = Directory.GetFiles(dirPath, "*_connection_params.txt");

        foreach (var file in files) {
            var cParams = Parse(file);
            CParamsBySystemName.TryAdd(cParams.SystemName, new Dictionary<string, CParams>());
            CParamsBySystemName[cParams.SystemName].Add(cParams.GatewayName, cParams);
        }
    }

    /// <summary>
    ///     Parses the given file and returns the CParams instance.
    /// </summary>
    /// <param name="path">Path to the connection file.</param>
    /// <returns></returns>
    public CParams Parse(string path) {
        var content = File.ReadAllText(path);
        var cParams = new CParams();
        cParams.SystemName  = systemNameRegex.Match(content).Groups[1].Value;
        cParams.GatewayName = gatewayNameRegex.Match(content).Groups[1].Value;
        cParams.AdmCnnctPt  = admCnnctPtRegex.Match(content).Groups[1].Value;
        cParams.sCnnctPt    = sCnnctPtRegex.Match(content).Groups[1].Value;
        cParams.sMdCnnctPt  = sMdCnnctPtRegex.Match(content).Groups[1].Value;
        cParams.sIhCnnctPt  = sIhCnnctPtRegex.Match(content).Groups[1].Value;
        cParams.sTsCnnctPt  = sTsCnnctPtRegex.Match(content).Groups[1].Value;
        cParams.sPnLCnnctPt = sPnLCnnctPtRegex.Match(content).Groups[1].Value;
        cParams.DomainName  = domainNameRegex.Match(content).Groups[1].Value;
        cParams.LoggerAddr  = loggerAddrRegex.Match(content).Groups[1].Value;
        cParams.DmnSrvrAddr = dmnSrvrAddrRegex.Match(content).Groups[1].Value;
        cParams.LicSrvrAddr = licSrvrAddrRegex.Match(content).Groups[1].Value;
        cParams.LocBrokAddr = locBrokAddrRegex.Match(content).Groups[1].Value;
        return cParams;
    }

    private static Regex systemNameRegex  = new Regex(@"\s*System\s*Name\s*:\s*(\S.*)");
    private static Regex gatewayNameRegex = new Regex(@"\s*Gateway\s*Name\s*:\s*(\S.*)");
    private static Regex sCnnctPtRegex    = new Regex(@"\s*sCnnctPt\s*:\s*(\S+)");
    private static Regex sMdCnnctPtRegex  = new Regex(@"\s*sMdCnnctPt\s*:\s*(\S+)");
    private static Regex sIhCnnctPtRegex  = new Regex(@"\s*sIhCnnctPt\s*:\s*(\S+)");
    private static Regex sTsCnnctPtRegex  = new Regex(@"\s*sTsCnnctPt\s*:\s*(\S+)");
    private static Regex sPnLCnnctPtRegex = new Regex(@"\s*sPnLCnnctPt\s*:\s*(\S+)");
    private static Regex admCnnctPtRegex  = new Regex(@"\s*REngineParams\.AdmCnnctPt\s*:\s*(\S+)");
    private static Regex domainNameRegex  = new Regex(@"\s*REngineParams\.DomainName\s*:\s*(\S+)");
    private static Regex loggerAddrRegex  = new Regex(@"\s*REngineParams\.LoggerAddr\s*:\s*(\S+)");
    private static Regex dmnSrvrAddrRegex = new Regex(@"\s*REngineParams\.DmnSrvrAddr\s*:\s*(\S+)");
    private static Regex locBrokAddrRegex = new Regex(@"\s*REngineParams\.LocBrokAddr\s*:\s*(\S+)");
    private static Regex licSrvrAddrRegex = new Regex(@"\s*REngineParams\.LicSrvrAddr\s*:\s*(\S+)");
}

/// <summary>
///     Represents a set of parameters used for configuring
///     connections and logins in the Rithmic system.
/// </summary>
public class CParams : REngineParams {
    /// <summary>
    ///     System name like "Rithmic Paper Trading".
    /// </summary>
    public string SystemName { get; set; }

    /// <summary>
    ///     Gateway name like "Europe".
    /// </summary>
    public string GatewayName { get; set; }

    /// <summary>
    ///     Gets or sets the administrative connection point.
    /// </summary>
    public string AdmCnnctPt { get; set; }

    /// <summary>
    ///     Gets or sets the connection point for login repository.
    /// </summary>
    public string sCnnctPt { get; set; }

    /// <summary>
    ///     Gets or sets the market data connection point.
    /// </summary>
    public string sMdCnnctPt { get; set; }

    /// <summary>
    ///     Gets or sets the historical data connection point.
    /// </summary>
    public string sIhCnnctPt { get; set; }

    /// <summary>
    ///     Gets or sets the trading system connection point.
    /// </summary>
    public string sTsCnnctPt { get; set; }

    /// <summary>
    ///     Gets or sets the PnL connection point.
    /// </summary>
    public string sPnLCnnctPt { get; set; }
}