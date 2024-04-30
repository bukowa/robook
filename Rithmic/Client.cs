using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.omnesys.omne.om;
using com.omnesys.rapi;

namespace Rithmic;

/// <summary>
/// Parameters for the Client login method.
/// </summary>
public class LoginParams {
    public CParams CParams;
    public bool    PlugInMode;

    public Connection? PnlConnection;
    public Connection? MarketDataConnection;
    public Connection? HistoricalDataConnection;
    public Connection? TradingSystemConnection;


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

    public LoginParams(CParams cParams) {
        CParams = cParams;
    }
}

/// <summary>
///     Client class.
/// </summary>
public class Client : INotifyPropertyChanged {
    /// <summary>
    ///     REngine instance.
    /// </summary>
    public REngine? Engine { get; private set; }

    /// <summary>
    ///     RCallbacks instance.
    /// </summary>
    public readonly RHandler RHandler;

    /// <summary>
    ///     AdmCallbacks instance.
    /// </summary>
    public readonly AdmHandler AdmCallbacks;

    /// <summary>
    ///     Application instance.
    /// </summary>
    public readonly Application Application;

    #region PluginMode

    /// <summary>
    ///     Historical Data connection point when in PlugIn mode.
    /// </summary>
    public static string PluginIhCnnctPt = "127.0.0.1:3012";

    /// <summary>
    ///     Market Data connection point when in PlugIn mode.
    /// </summary>
    public static string PluginMdCnnctPt = "127.0.0.1:3010";

    /// <summary>
    ///     Required environment variables when in PlugIn mode.
    /// </summary>
    public static Dictionary<string, string> PluginEnv = new() {
        { "RAPI_MD_ENCODING", "4" },
        { "RAPI_IH_ENCODING", "4" }
    };

    /// <summary>
    ///     Sets the required environment variables when in PlugIn mode.
    /// </summary>
    public static void SetPlugInMode(REngine engine, string env) {
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
    ///     Creates a new instance of the Client class.
    /// </summary>
    public Client(
        RHandler    rh = null,
        AdmHandler  ah = null,
        Application ap = null
    ) {
        Application  = ap ?? new Application();
        AdmCallbacks = ah ?? new AdmHandler();
        RHandler     = rh ?? new RHandler();
    }

    /// <summary>
    ///     Parameters for the Client constructor.
    /// </summary>
    public LoginParams? Params {
        get => _loginParams;
        set {
            _loginParams = value;
            notifyPropertyChanged();
        }
    }

    private LoginParams _loginParams;

    /// <summary>
    /// Logout from Rithmic.
    /// </summary>
    public void Logout() {
        Engine?.logout();
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
                : CParams.sMdCnnctPt;
            var sIhvCnnctPt = HistoricalDataConnection == null
                ? string.Empty
                : CParams.sIhCnnctPt;
            var sTsCnnctPt = TradingSystemConnection == null
                ? string.Empty
                : CParams.sTsCnnctPt;
            var sPnlCnnctPt = PnlConnection == null
                ? string.Empty
                : CParams.sPnLCnnctPt;

            var context = new Context();
            HistoricalDataConnection?.TrySubscribeRHandler(RHandler, context);
            TradingSystemConnection?.TrySubscribeRHandler(RHandler, context);
            MarketDataConnection?.TrySubscribeRHandler(RHandler, context);
            PnlConnection?.TrySubscribeRHandler(RHandler, context);

            Engine?.shutdown();
            Engine = null;
            Engine = new REngine(new REngineParams() {
                AdmCallbacks = AdmCallbacks,
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
                    SetPlugInMode(Engine, Constants.DEFAULT_ENVIRONMENT_KEY);
                    mdCnnctPt   = MarketDataConnection == null ? string.Empty : PluginMdCnnctPt;
                    sIhvCnnctPt = HistoricalDataConnection == null ? string.Empty : PluginIhCnnctPt;
                }
            }

            Engine.login
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
        catch (Exception e) {
            Engine?.shutdown();
            Engine = null;
            throw e;
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
        CountdownEvent?  logoutEvent  = null;
        
        void ActionLogoutEvent(Connection c, ConnectionAlert a, DateTime dt) {
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
        List<Connection>            connections      = loginParams.ConnectionsEnumerable.ToList();
        int                         eventsCount      = 0;
        CountdownEvent?             loginEvent       = null;

        void ActionLoginEvent(Connection c, ConnectionAlert a, DateTime dt) {
            loginEvent?.Signal();
        }
        
        void SubscribeToLoginEvents(Connection? connection) {
            connection?.SubscribeToOnLoginComplete(ActionLoginEvent);
            connection?.SubscribeToOnLoginFailed(ActionLoginEvent);
            eventsCount++;
        }

        void UnsubscribeFromLoginEvents(Connection? connection) {
            connection?.UnsubscribeFromOnLoginComplete(ActionLoginEvent);
            connection?.UnsubscribeFromOnLoginFailed(ActionLoginEvent);
        }

        bool IsLoggedId(Connection? connection) {
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

        if (connections.All(IsLoggedId)) {
            return true;
        }

        return false;
    }

    #region INotifyPropertyChanged

    public void SubscribeToPropertyChangedEvent(Action<Client, PropertyChangedEventArgs> action) {
        PropertyChanged += (sender, args) => { action(this, args); };
    }

    public void SubscribeToPropertyChangedEvent(string property, Action<Client, PropertyChangedEventArgs> action) {
        PropertyChanged += (sender, args) => {
            if (args.PropertyName == property) {
                action(this, args);
            }
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void setProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
        if (!EqualityComparer<T>.Default.Equals(field, value)) {
            field = value;
            notifyPropertyChanged(propertyName);
        }
    }

    private void notifyPropertyChanged([CallerMemberName] String propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}