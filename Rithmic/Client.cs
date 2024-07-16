using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using com.omnesys.rapi;

namespace Rithmic;

[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public interface IClient : INotifyPropertyChanged {
    public Task LoginAsync(LoginParams  loginParams);
    public Task LogoutAsync(LoginParams loginParams);

    public RHandler                  RHandler   { get; }
    public AdmHandler                AdmHandler { get; }
    public com.omnesys.rapi.REngine? REngine    { get; }

    public Connection? PnlConnection            { get; }
    public Connection? MarketDataConnection     { get; }
    public Connection? TradingSystemConnection  { get; }
    public Connection? HistoricalDataConnection { get; }
}

/// <summary>
/// Parameters for the Client login method.
/// </summary>
public class LoginParams(CParams cParams) {
    public readonly CParams CParams = cParams;

    public Connection? PnlConnection;
    public Connection? MarketDataConnection;
    public Connection? HistoricalDataConnection;
    public Connection? TradingSystemConnection;

    public bool PlugInMode;

    public IEnumerable<Connection> ConnectionsEnumerable {
        get {
            if (MarketDataConnection != null) {
                yield return MarketDataConnection;
            }

            if (HistoricalDataConnection != null) {
                yield return HistoricalDataConnection;
            }

            if (TradingSystemConnection != null) {
                yield return TradingSystemConnection;
            }

            if (PnlConnection != null) {
                yield return PnlConnection;
            }
        }
    }
}

/// <summary>
///     Client class.
/// </summary>
public class Client : IClient {
    /// <summary>
    ///     Creates a new instance of the Client class.
    /// </summary>
    public Client() {
    }

    /// <summary>
    ///     REngine instance.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    public com.omnesys.rapi.REngine? REngine { get; private set; }

    public Task LoginAsync(LoginParams loginParams) {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(LoginParams loginParams) {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     RCallbacks instance.
    /// </summary>
    public RHandler RHandler { get; } = new();

    /// <summary>
    ///     AdmCallbacks instance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public AdmHandler AdmHandler { get; } = new();

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
    public static string PluginIhCnnctPt = "127.0.0.1:3012";

    /// <summary>
    ///     Market Data connection point when in PlugIn mode.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    public static string PluginMdCnnctPt = "127.0.0.1:3010";

    /// <summary>
    ///     Required environment variables when in PlugIn mode.
    /// </summary>
    private static readonly Dictionary<string, string> PluginEnv = new() {
        { "RAPI_MD_ENCODING", "4" },
        { "RAPI_IH_ENCODING", "4" }
    };

    /// <summary>
    ///     Sets the required environment variables when in PlugIn mode.
    /// </summary>
    private static void SetPlugInMode(REngine engine, string env) {
        foreach (var kv in PluginEnv) {
            engine.setEnvironmentVariable(env, kv.Key, kv.Value);
        }
    }

    #endregion

    #region Connections

    public Connection? PnlConnection            => Params?.PnlConnection;
    public Connection? MarketDataConnection     => Params?.MarketDataConnection;
    public Connection? TradingSystemConnection  => Params?.TradingSystemConnection;
    public Connection? HistoricalDataConnection => Params?.HistoricalDataConnection;
    public CParams?    CParams                  => Params?.CParams;

    #endregion

    /// <summary>
    ///     Parameters for the Client constructor.
    /// </summary>
    public LoginParams? Params {
        get => _loginParams;
        set {
            _loginParams = value;
            NotifyPropertyChanged();
        }
    }

    private LoginParams? _loginParams;

    /// <summary>
    /// Logout from Rithmic.
    /// </summary>
    public void Logout() {
        REngine?.logout();
    }

    /// <summary>
    ///     Establishes a connection to Rithmic.
    /// </summary>
    public void Login(LoginParams loginParams) {
        Params = loginParams;
        try {
            if (TradingSystemConnection == null && PnlConnection != null) {
                throw new Exception("PnL connection requires TradingSystem connection");
            }

            var mdCnnctPt = MarketDataConnection == null
                ? string.Empty
                : CParams.SMdCnnctPt;
            var sIhvCnnctPt = HistoricalDataConnection == null
                ? string.Empty
                : CParams.SIhCnnctPt;
            var sTsCnnctPt = TradingSystemConnection == null
                ? string.Empty
                : CParams.STsCnnctPt;
            var sPnlCnnctPt = PnlConnection == null
                ? string.Empty
                : CParams.SPnLCnnctPt;

            var context = new Context();
            HistoricalDataConnection?.TrySubscribeRHandler(RHandler, context);
            TradingSystemConnection?.TrySubscribeRHandler(RHandler, context);
            MarketDataConnection?.TrySubscribeRHandler(RHandler, context);
            PnlConnection?.TrySubscribeRHandler(RHandler, context);

            REngine?.shutdown();
            REngine = null;
            REngine = new REngine(new REngineParams() {
                AdmCallbacks = AdmHandler,
                AppName      = Application.Name,
                AppVersion   = Application.Version,
                DomainName   = CParams.DomainName,
                LoggerAddr   = CParams.LoggerAddr,
                DmnSrvrAddr  = CParams.DmnSrvrAddr,
                LicSrvrAddr  = CParams.LicSrvrAddr,
                LocBrokAddr  = CParams.LocBrokAddr,
                LogFilePath  = CParams.LogFilePath,
            });

            if (Params.PlugInMode) {
                if (MarketDataConnection != null && HistoricalDataConnection != null) {
                    SetPlugInMode(REngine, Constants.DEFAULT_ENVIRONMENT_KEY);
                    mdCnnctPt   = MarketDataConnection == null ? string.Empty : PluginMdCnnctPt;
                    sIhvCnnctPt = HistoricalDataConnection == null ? string.Empty : PluginIhCnnctPt;
                }
            }

            REngine.login
            (
                RHandler,
                sMdEnvKey: Constants.DEFAULT_ENVIRONMENT_KEY,
                sTsEnvKey: Constants.DEFAULT_ENVIRONMENT_KEY,
                sIhEnvKey: Constants.DEFAULT_ENVIRONMENT_KEY,
                sMdCnnctPt: mdCnnctPt,
                sMdUser: MarketDataConnection?.Login ?? string.Empty,
                sMdPassword: MarketDataConnection?.Password ?? string.Empty,
                sIhCnnctPt: sIhvCnnctPt,
                sIhUser: HistoricalDataConnection?.Login ?? string.Empty,
                sIhPassword: HistoricalDataConnection?.Password ?? string.Empty,
                sTsCnnctPt: sTsCnnctPt,
                sTsUser: TradingSystemConnection?.Login ?? string.Empty,
                sTsPassword: TradingSystemConnection?.Password ?? string.Empty,
                sPnlCnnctPt: sPnlCnnctPt
            );
        }
        catch (Exception) {
            REngine?.shutdown();
            REngine = null;
            throw;
        }
    }


    private object _logoutLoginLock = new();

    /// <summary>
    /// Logout and todo: wait for all connections to be ready.
    /// </summary>
    public void LogoutAndWait() {
        lock (_logoutLoginLock) {
            _logoutAndWait();
        }
    }

    private void _logoutAndWait() {
        List<Connection> connections = Params.ConnectionsEnumerable.ToList();
        int              eventsCount = 0;
        CountdownEvent?  logoutEvent = null;

        void ActionLogoutEvent(Connection c, ConnectionAlert a) {
            logoutEvent?.Signal();
        }

        void SubscribeToLogoutEvents(Connection? connection) {
            connection?.SubscribeToOnConnectionClosed(ActionLogoutEvent);
            eventsCount++;
        }

        void UnsubscribeFromLogoutEvents(Connection? connection) {
            connection?.UnsubscribeFromOnConnectionClosed(ActionLogoutEvent);
        }

        try {
            connections.ForEach(SubscribeToLogoutEvents);
            logoutEvent = new CountdownEvent(eventsCount);
            Logout();
            logoutEvent.Wait(15000);
        }
        finally {
            connections.ForEach(UnsubscribeFromLogoutEvents);
            logoutEvent?.Dispose();
            logoutEvent = null;
        }
    }

    /// <summary>
    /// Login and wait for all connections to be ready.
    /// </summary>
    /// <returns>True if all connections logged in successfully.</returns>
    public bool LoginAndWait(LoginParams loginParams, int timeout = 15000) {
        lock (_logoutLoginLock) {
            return _loginAndWait(loginParams, timeout);
        }
    }

    private bool _loginAndWait(LoginParams loginParams, int timeout = 1500) {
        List<Connection> connections = loginParams.ConnectionsEnumerable.ToList();
        int              eventsCount = 0;
        CountdownEvent?  loginEvent  = null;

        void ActionLoginEvent(Connection c, ConnectionAlert a) {
            loginEvent?.Signal();
        }

        void SubscribeToLoginEvents(Connection? connection) {
            connection?.SubscribeToOnLoginComplete(ActionLoginEvent);
            connection?.SubscribeToLoginFailed(ActionLoginEvent);
            eventsCount++;
        }

        void UnsubscribeFromLoginEvents(Connection? connection) {
            connection?.UnsubscribeFromOnLoginComplete(ActionLoginEvent);
            connection?.UnsubscribeFromOnLoginFailed(ActionLoginEvent);
        }

        bool IsLoggedIn(Connection? connection) {
            return connection?.IsLoggedIn ?? false;
        }

        try {
            connections.ForEach(SubscribeToLoginEvents);
            loginEvent = new CountdownEvent(eventsCount);
            Login(loginParams);
            loginEvent.Wait(timeout);
        }
        finally {
            connections.ForEach(UnsubscribeFromLoginEvents);
            loginEvent?.Dispose();
            loginEvent = null;
        }

        if (connections.All(IsLoggedIn)) {
            return true;
        }

        return false;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
        if (!EqualityComparer<T>.Default.Equals(field, value)) {
            field = value;
            NotifyPropertyChanged(propertyName);
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ObservePropertyChange(string property, Action<Client, PropertyChangedEventArgs> action) {
        PropertyChanged += (_, args) => {
            if (args.PropertyName == property) {
                action(this, args);
            }
        };
    }

    #endregion
}