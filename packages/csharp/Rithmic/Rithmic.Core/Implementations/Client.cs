using System.Diagnostics.CodeAnalysis;

namespace Rithmic.Core;

/// <summary>
/// Rithmic Auth.
/// </summary>
public class RithmicAuth : IRithmicAuth {
    public ILParams LParams { get; }
    public CParams  CParams { get; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public RithmicAuth(ILParams lParams, CParams cParams) {
        LParams = lParams;
        CParams = cParams;
    }
}

/// <summary>
/// Client implementation.
/// </summary>
public class RithmicClient<TRCallback, TAdmCallback> : IRithmicClient
    where TRCallback : IRCallbacks, new()
    where TAdmCallback : IAdmCallbacks, new() {
    /// <summary>
    /// Creates a new instance of the Client class.
    /// </summary>
    public RithmicClient(
    ) {
        RCallbacks   = new TRCallback();
        AdmCallbacks = new TAdmCallback();
    }

    /// <summary>
    /// REngine instance.
    /// </summary>
    public rapi.REngine? REngine { get; private set; }

    /// <summary>
    ///     RCallbacks instance.
    /// </summary>
    public IRCallbacks RCallbacks { get; private set; }

    /// <summary>
    ///     AdmCallbacks instance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public IAdmCallbacks AdmCallbacks { get; private set; }

    /// <summary>
    /// Login to the Rithmic system.
    /// </summary>
    public Task LoginAsync(IRithmicAuth auth, int timeout=5000) {
        return LoginAndWait(auth, timeout);
    }

    /// <summary>
    /// Logout from the Rithmic system.
    /// </summary>
    public Task LogoutAsync(int timeout=5000) {
        return LogoutAndWait(timeout);
    }

    /// <summary>
    /// LParams instance.
    /// </summary>
    public ILParams? LParams { get; private set; }

    /// <summary>
    /// CParams instance.
    /// </summary>
    public CParams? CParams { get; private set; }

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
    private readonly Dictionary<string, string> _pluginEnv = new() {
        { "RAPI_MD_ENCODING", "4" },
        { "RAPI_IH_ENCODING", "4" }
    };

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
            REngine?.logout();
        }
        catch (Exception) {
            // ignored
        }

        RCallbacks = new TRCallback();
        try {
            REngine?.shutdown();
        }
        catch (Exception) {
            // ignored
        }

        REngine      = null;
        AdmCallbacks = new TAdmCallback();
    }

    private object _lock = new();

    private void Login(IRithmicAuth auth) {
        CParams = auth.CParams;
        LParams = auth.LParams;

        ResetClientState();
        try {
            if (LParams.TradingSystemConnection == null && LParams.PnlConnection != null) {
                throw new Exception("PnL connection requires TradingSystem connection");
            }

            var mdCnnctPt = LParams.MarketDataConnection == null
                ? string.Empty
                : CParams.SMdCnnctPt;
            var sIhvCnnctPt = LParams.HistoricalDataConnection == null
                ? string.Empty
                : CParams.SIhCnnctPt;
            var sTsCnnctPt = LParams.TradingSystemConnection == null
                ? string.Empty
                : CParams.STsCnnctPt;
            var sPnlCnnctPt = LParams.PnlConnection == null
                ? string.Empty
                : CParams.SPnLCnnctPt;

            var context = new Context();
            LParams.HistoricalDataConnection?.RegisterRCallbackAlertHandler(context, RCallbacks);
            LParams.TradingSystemConnection?.RegisterRCallbackAlertHandler(context, RCallbacks);
            LParams.MarketDataConnection?.RegisterRCallbackAlertHandler(context, RCallbacks);
            LParams.PnlConnection?.RegisterRCallbackAlertHandler(context, RCallbacks);

            REngine = new rapi.REngine(new rapi.REngineParams() {
                AdmCallbacks = AdmCallbacks.GetAdmCallbacks(),
                AppName      = Application.Name,
                AppVersion   = Application.Version,
                DomainName   = CParams.DomainName,
                LoggerAddr   = CParams.LoggerAddr,
                DmnSrvrAddr  = CParams.DmnSrvrAddr,
                LicSrvrAddr  = CParams.LicSrvrAddr,
                LocBrokAddr  = CParams.LocBrokAddr,
                LogFilePath  = CParams.LogFilePath,
            });

            if (LParams.PlugInMode) {
                if (LParams is { MarketDataConnection: not null, HistoricalDataConnection: not null }) {
                    SetPlugInMode(REngine, rapi.Constants.DEFAULT_ENVIRONMENT_KEY);
                    mdCnnctPt   = LParams.MarketDataConnection == null ? string.Empty : _pluginMdCnnctPt;
                    sIhvCnnctPt = LParams.HistoricalDataConnection == null ? string.Empty : _pluginIhCnnctPt;
                }
            }

            REngine.login
            (
                RCallbacks.GetRCallbacks(),
                sMdEnvKey: rapi.Constants.DEFAULT_ENVIRONMENT_KEY,
                sTsEnvKey: rapi.Constants.DEFAULT_ENVIRONMENT_KEY,
                sIhEnvKey: rapi.Constants.DEFAULT_ENVIRONMENT_KEY,
                sMdCnnctPt: mdCnnctPt,
                sMdUser: LParams.MarketDataConnection?.Login ?? string.Empty,
                sMdPassword: LParams.MarketDataConnection?.Password ?? string.Empty,
                sIhCnnctPt: sIhvCnnctPt,
                sIhUser: LParams.HistoricalDataConnection?.Login ?? string.Empty,
                sIhPassword: LParams.HistoricalDataConnection?.Password ?? string.Empty,
                sTsCnnctPt: sTsCnnctPt,
                sTsUser: LParams.TradingSystemConnection?.Login ?? string.Empty,
                sTsPassword: LParams.TradingSystemConnection?.Password ?? string.Empty,
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
        if (LParams == null) {
            return Task.CompletedTask;
        }
        lock (_lock) {
            List<IConnection> connections = LParams.ConnectionsEnumerable.ToList();
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