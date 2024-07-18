using System.Diagnostics.CodeAnalysis;

namespace Rithmic.Core;

/// <summary>
/// Rithmic Auth.
/// </summary>
public class RithmicAuth : IRithmicAuth {
    public required ILParams LParams { get; set; }
    public required CParams  CParams { get; set; }
}

public class RithmicService : IRithmicService {
    public IREngineOperations? REngineOperations      { get; set; }
    public IRCallbacksFacade   RCallbacksFacade   { get; set; }
    public IAdmCallbacksFacade AdmCallbacksFacade { get; set; }

    public RithmicService() {
    }
}

/// <summary>
/// Client implementation.
/// </summary>
public class RithmicClient
<
    TRCallbacksFacade, TAdmCallbacksFacade, TRithmicService, TREngineOperationsFactory
> : IRithmicClient
    where TRCallbacksFacade : IRCallbacksFacade, new()
    where TAdmCallbacksFacade : IAdmCallbacksFacade, new()
    where TRithmicService : IRithmicService, new()
    where TREngineOperationsFactory : IREngineOperationsFactory {
    /// <summary>
    /// Creates a new instance of the Client class.
    /// </summary>
    public RithmicClient() {
        RithmicService = new TRithmicService {
            RCallbacksFacade = new TRCallbacksFacade(), AdmCallbacksFacade = new TAdmCallbacksFacade(),
        };
    }

    /// <summary>
    /// Rithmic Auth.
    /// </summary>
    public IRithmicAuth RithmicAuth { get; private set; }

    /// <summary>
    /// Rithmic Service.
    /// </summary>
    public IRithmicService RithmicService { get; }

    /// <summary>
    /// Login to the Rithmic system.
    /// </summary>
    public Task LoginAsync(IRithmicAuth auth, int timeout = 5000) {
        return LoginAndWait(auth, timeout);
    }

    /// <summary>
    /// Logout from the Rithmic system.
    /// </summary>
    public Task LogoutAsync(int timeout = 5000) {
        return LogoutAndWait(timeout);
    }

    /// <summary>
    ///     Application instance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    public Application Application = new();

    #region PluginMode

    /// <summary>
    ///     Historical Data connection point when in PlugIn mode.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    private readonly string _pluginIhCnnctPt = "127.0.0.1:3012";

    /// <summary>
    ///     Market Data connection point when in PlugIn mode.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    private readonly string _pluginMdCnnctPt = "127.0.0.1:3010";

    /// <summary>
    ///     Required environment variables when in PlugIn mode.
    /// </summary>
    private readonly Dictionary<string, string> _pluginEnv = new() { { "RAPI_MD_ENCODING", "4" }, { "RAPI_IH_ENCODING", "4" } };

    /// <summary>
    ///     Sets the required environment variables when in PlugIn mode.
    /// </summary>
    private void SetPlugInMode(rapi.REngine engine, string env) {
        foreach (var kv in _pluginEnv) {
            engine.setEnvironmentVariable(env, kv.Key, kv.Value);
        }
    }

    #endregion

    /// <summary>
    /// Reset the client according to Rithmic Documentation.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public void ResetClientState() {
        try {
            RithmicService.REngineOperations?.REngine.logout();
        }
        catch (Exception) {
            // ignored
        }

        RithmicService.RCallbacksFacade = new TRCallbacksFacade();
        try {
            RithmicService.REngineOperations?.REngine.shutdown();
        }
        catch (Exception) {
            // ignored
        }

        RithmicService.REngineOperations      = null;
        RithmicService.AdmCallbacksFacade = new TAdmCallbacksFacade();
    }

    private object _lock = new();

    private void Login(IRithmicAuth auth) {
        RithmicAuth = auth;
        var lParams = auth.LParams;
        var cParams = auth.CParams;

        ResetClientState();
        try {
            if (lParams.TradingSystemConnection == null && lParams.PnlConnection != null) {
                throw new Exception("PnL connection requires TradingSystem connection");
            }

            var mdCnnctPt = lParams.MarketDataConnection == null
                ? string.Empty
                : cParams.SMdCnnctPt;
            var sIhvCnnctPt = lParams.HistoricalDataConnection == null
                ? string.Empty
                : cParams.SIhCnnctPt;
            var sTsCnnctPt = lParams.TradingSystemConnection == null
                ? string.Empty
                : cParams.STsCnnctPt;
            var sPnlCnnctPt = lParams.PnlConnection == null
                ? string.Empty
                : cParams.SPnLCnnctPt;

            var context = new Context();
            lParams.HistoricalDataConnection?.RegisterRCallbackAlertHandler(context, RithmicService.RCallbacksFacade);
            lParams.TradingSystemConnection?.RegisterRCallbackAlertHandler(context, RithmicService.RCallbacksFacade);
            lParams.MarketDataConnection?.RegisterRCallbackAlertHandler(context, RithmicService.RCallbacksFacade);
            lParams.PnlConnection?.RegisterRCallbackAlertHandler(context, RithmicService.RCallbacksFacade);

            var rEngine = new rapi.REngine
            (
                new rapi.REngineParams {
                    AdmCallbacks = RithmicService.AdmCallbacksFacade.GetAdmCallbacks(),
                    AppName      = Application.Name,
                    AppVersion   = Application.Version,
                    DomainName   = cParams.DomainName,
                    LoggerAddr   = cParams.LoggerAddr,
                    DmnSrvrAddr  = cParams.DmnSrvrAddr,
                    LicSrvrAddr  = cParams.LicSrvrAddr,
                    LocBrokAddr  = cParams.LocBrokAddr,
                    LogFilePath  = cParams.LogFilePath,
                }
            );

            RithmicService.REngineOperations = TREngineOperationsFactory.Create(rEngine);

            if (lParams.PlugInMode) {
                if (lParams is { MarketDataConnection: not null, HistoricalDataConnection: not null }) {
                    SetPlugInMode(rEngine, rapi.Constants.DEFAULT_ENVIRONMENT_KEY);
                    mdCnnctPt   = lParams.MarketDataConnection == null ? string.Empty : _pluginMdCnnctPt;
                    sIhvCnnctPt = lParams.HistoricalDataConnection == null ? string.Empty : _pluginIhCnnctPt;
                }
            }

            rEngine.login
            (
                RithmicService.RCallbacksFacade.GetRCallbacks(),
                sMdEnvKey: rapi.Constants.DEFAULT_ENVIRONMENT_KEY,
                sTsEnvKey: rapi.Constants.DEFAULT_ENVIRONMENT_KEY,
                sIhEnvKey: rapi.Constants.DEFAULT_ENVIRONMENT_KEY,
                sMdCnnctPt: mdCnnctPt,
                sMdUser: lParams.MarketDataConnection?.Login ?? string.Empty,
                sMdPassword: lParams.MarketDataConnection?.Password ?? string.Empty,
                sIhCnnctPt: sIhvCnnctPt,
                sIhUser: lParams.HistoricalDataConnection?.Login ?? string.Empty,
                sIhPassword: lParams.HistoricalDataConnection?.Password ?? string.Empty,
                sTsCnnctPt: sTsCnnctPt,
                sTsUser: lParams.TradingSystemConnection?.Login ?? string.Empty,
                sTsPassword: lParams.TradingSystemConnection?.Password ?? string.Empty,
                sPnlCnnctPt: sPnlCnnctPt
            );
        }
        catch (Exception) {
            ResetClientState();
            throw;
        }
    }

    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
    private Task LoginAndWait(IRithmicAuth auth, int timeout) {
        lock (_lock) {
            List<IConnection> connections = auth.LParams.ConnectionsEnumerable.ToList();
            CountdownEvent?   loginEvent  = null;

            void LoginHandler(IConnection c, IConnectionAlert _) {
                loginEvent?.Signal();
            }

            void RegisterLoginHandler(IConnection c) {
                c.RegisterHandler(rapi.AlertType.LoginComplete, LoginHandler);
                c.RegisterHandler(rapi.AlertType.LoginFailed,   LoginHandler);
            }

            void UnregisterLoginHandler(IConnection c) {
                c.UnregisterHandler(rapi.AlertType.LoginComplete, LoginHandler);
                c.UnregisterHandler(rapi.AlertType.LoginFailed,   LoginHandler);
            }

            try {
                connections.ForEach(RegisterLoginHandler);
                loginEvent = new CountdownEvent(connections.Count);
                Login(auth);
                loginEvent.Wait(timeout);
            }

            finally {
                connections.ForEach(UnregisterLoginHandler);
                loginEvent?.Dispose();
                loginEvent = null;
            }
        }

        return Task.CompletedTask;
    }

    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
    private Task LogoutAndWait(int timeout) {
        lock (_lock) {
            List<IConnection> connections = RithmicAuth.LParams.ConnectionsEnumerable.ToList();
            CountdownEvent?   logoutEvent = null;

            void LogoutHandler(IConnection c, IConnectionAlert _) {
                logoutEvent?.Signal();
            }

            void RegisterLogoutHandler(IConnection c) {
                c.RegisterHandler(rapi.AlertType.ConnectionClosed, LogoutHandler);
            }

            void UnregisterLogoutHandler(IConnection c) {
                c.UnregisterHandler(rapi.AlertType.ConnectionClosed, LogoutHandler);
            }

            try {
                connections.ForEach(RegisterLogoutHandler);
                logoutEvent = new CountdownEvent(connections.Count);
                // this has to call the logout method
                ResetClientState();
                logoutEvent.Wait(timeout);
            }

            finally {
                connections.ForEach(UnregisterLogoutHandler);
                logoutEvent?.Dispose();
                logoutEvent = null;
            }
        }

        return Task.CompletedTask;
    }
}