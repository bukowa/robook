﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.omnesys.rapi;

namespace Rithmic;

/// <summary>
/// Parameters for the Client login method.
/// </summary>
public class LoginParams {
    public readonly CParams     CParams;
    public          bool        PlugInMode;
    public          Connection? PnlConnection;
    public          Connection? MarketDataConnection;
    public          Connection? HistoricalDataConnection;
    public          Connection? TradingSystemConnection;

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

    public Connection? PnlConnection            => Params.PnlConnection;
    public Connection? MarketDataConnection     => Params.MarketDataConnection;
    public Connection? TradingSystemConnection  => Params.TradingSystemConnection;
    public Connection? HistoricalDataConnection => Params.HistoricalDataConnection;
    public CParams?    CParams                  => Params.CParams;

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
        
        // if there are new CParams make sure to update the Engine
        // and notify the subscribers that the Engine has changed
        // meaning they have to re-subscribe to the new Engine
        // we can subscribe to engine only after login complete event
        // todo figure out how to handle this...
        // todo probably the proper way to do it is to make others
        // interact with the engine / rhandler only via this class
        // meaning they can subscribe an action that we will save to the internal list
        // and later when the engine is ready / client is logged in we will subscribe
        // todo this needs proper way of handling when to exactly do this
        // but to be realistic when engine loginparams change it means
        // the client changed too so...
        bool isNewEngineRequired = 
            Params?.CParams.GatewayName != loginParams?.CParams.GatewayName || 
            Params?.CParams.SystemName != loginParams?.CParams.SystemName ||
            Params == null;
        
        var oldParams = Params;
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
            
            if (isNewEngineRequired) {
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
            }
            
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
            Params = oldParams;
            Engine?.shutdown();
            Engine = null;
            throw e;
        }
    }

    /// <summary>
    /// Login and wait for all connections to be ready.
    /// </summary>
    /// <returns>True if all connections logged in successfully.</returns>
    public bool LoginAndWait(LoginParams loginParams, int timeout = 15000) {
        CountdownEvent? loginEvent  = null;
        var             eventsCount = 0;


        var actionLoginEvent = new Connection.AlertInfoHandler((c, a, dt) => { loginEvent?.Signal(); });
        
        if (loginParams.MarketDataConnection != null) {
            loginParams.MarketDataConnection.OnLoginComplete += actionLoginEvent;
            loginParams.MarketDataConnection.OnLoginFailed   += actionLoginEvent;
            eventsCount++;
        }

        if (loginParams.HistoricalDataConnection != null) {
            loginParams.HistoricalDataConnection.OnLoginComplete += actionLoginEvent;
            loginParams.HistoricalDataConnection.OnLoginFailed   += actionLoginEvent;
            eventsCount++;
        }

        if (loginParams.TradingSystemConnection != null) {
            loginParams.TradingSystemConnection.OnLoginComplete += actionLoginEvent;
            loginParams.TradingSystemConnection.OnLoginFailed   += actionLoginEvent;
            eventsCount++;
        }

        if (loginParams.PnlConnection != null) {
            loginParams.PnlConnection.OnLoginComplete += actionLoginEvent;
            loginParams.PnlConnection.OnLoginFailed   += actionLoginEvent;
            eventsCount++;
        }

        loginEvent = new CountdownEvent(eventsCount);
        Login(loginParams);
        loginEvent.Wait(timeout);

        if (MarketDataConnection != null) {
            MarketDataConnection.OnLoginComplete -= actionLoginEvent;
            MarketDataConnection.OnLoginFailed   -= actionLoginEvent;
        }

        if (HistoricalDataConnection != null) {
            HistoricalDataConnection.OnLoginComplete -= actionLoginEvent;
            HistoricalDataConnection.OnLoginFailed   -= actionLoginEvent;
        }

        if (TradingSystemConnection != null) {
            TradingSystemConnection.OnLoginComplete -= actionLoginEvent;
            TradingSystemConnection.OnLoginFailed   -= actionLoginEvent;
        }

        if (PnlConnection != null) {
            PnlConnection.OnLoginComplete -= actionLoginEvent;
            PnlConnection.OnLoginFailed   -= actionLoginEvent;
        }

        // return false if any connection failed to login
        if (MarketDataConnection != null && !MarketDataConnection.IsLoggedIn) return false;
        if (HistoricalDataConnection != null && !HistoricalDataConnection.IsLoggedIn) return false;
        if (TradingSystemConnection != null && !TradingSystemConnection.IsLoggedIn) return false;
        if (PnlConnection != null && !PnlConnection.IsLoggedIn) return false;

        return true;
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