using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.omnesys.rapi;

namespace Rithmic;

/// <summary>
/// Parameters for <see cref="Connection"/> constructor.
/// </summary>
public class ConnectionParams {
    public string       Login        { get; set; }
    public string       Password     { get; set; }
    public ConnectionId ConnectionId { get; set; }
}

/// <summary>
/// Represents a connection to Rithmic.
/// </summary>
public class Connection : INotifyPropertyChanged {
    /// <summary>
    ///     Login for this connection.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    ///     Password for this connection.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///     ConnectionId for this connection.
    /// </summary>
    public readonly ConnectionId ConnectionId;

    /// <summary>
    /// Constructor.
    /// </summary>
    public Connection(
        string       login,
        string       password,
        ConnectionId connectionId
    ) {
        Login        = login;
        Password     = password;
        ConnectionId = connectionId;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Connection(ConnectionParams connectionParams) {
        Login        = connectionParams.Login;
        Password     = connectionParams.Password;
        ConnectionId = connectionParams.ConnectionId;
    }

    /// <summary>
    ///     Returns boolean indicating if the connection is logged in.
    /// </summary>
    public bool IsLoggedIn {
        get => _isLoggedId;
        private set {
            _isLoggedId = value;
            _notifyPropertyChanged();
        }
    }

    private bool _isLoggedId;

    /// <summary>
    ///     Indicates the last alert info for connection.
    /// </summary>
    public AlertInfo? LastAlertInfo {
        get => _lastAlertInfo;
        set {
            _lastAlertInfo = value;
            _notifyPropertyChanged();
        }
    }

    private AlertInfo? _lastAlertInfo;

    /// <summary>
    ///     Indicates latest update time for this connection.
    /// </summary>
    public DateTime? LastAlertTime {
        get => _lastAlertTime;
        private set {
            _lastAlertTime = value;
            _notifyPropertyChanged();
        }
    }

    private DateTime? _lastAlertTime;

    /// <summary>
    /// Delegate for handling alerts for this connection.
    /// </summary>
    public delegate void AlertInfoHandler(Connection sender, AlertInfo alertInfo, DateTime time);

    /// <summary>
    /// <see cref="`OnAlertInfo`"/> is invoked before any modification to the connection
    /// instance is made. It is an event for all alert types. Other alert type events
    /// are invoked after this event and after the instance attributes are modified.
    /// </summary>
    public event AlertInfoHandler OnAlertInfo;

    public event AlertInfoHandler OnLoginFailed;
    public event AlertInfoHandler OnForcedLogout;
    public event AlertInfoHandler OnLoginComplete;
    public event AlertInfoHandler OnShutdownSignal;
    public event AlertInfoHandler OnConnectionOpened;
    public event AlertInfoHandler OnConnectionClosed;
    public event AlertInfoHandler OnConnectionBroken;

    // these methods are a convenience way to subscribe to events
    // without having to check for the null value of the Connection
    // like this: if (MarketDataConnection != null) { ... }
    // instead, you can use this: MarketDataConnection?.SubscribeToOnAlertInfo(...);

    public void SubscribeToOnAlertInfo(AlertInfoHandler action) {
        OnAlertInfo += action;
    }

    public void SubscribeToOnLoginFailed(AlertInfoHandler action) {
        OnLoginFailed += action;
    }

    public void SubscribeToOnForcedLogout(AlertInfoHandler action) {
        OnForcedLogout += action;
    }

    public void SubscribeToOnShutdownSignal(AlertInfoHandler action) {
        OnShutdownSignal += action;
    }

    public void SubscribeToOnConnectionOpened(AlertInfoHandler action) {
        OnConnectionOpened += action;
    }

    public void SubscribeToOnConnectionClosed(AlertInfoHandler action) {
        OnConnectionClosed += action;
    }

    public void SubscribeToOnConnectionBroken(AlertInfoHandler action) {
        OnConnectionBroken += action;
    }

    public void SubscribeToOnLoginComplete(AlertInfoHandler action) {
        OnLoginComplete += action;
    }

    /// <summary>
    /// Handles alerts for this connection.
    /// </summary>
    /// <param name="info"> </param>
    public void HandleAlertInfo(IContext ctx, AlertInfo info) {
        var now = DateTime.UtcNow;

        // AlertInfo as received from Rithmic does not pass any Context -
        // so we cannot route it to the correct connection based on the Context.
        // to make sure that the AlertInfo is for this connection,
        // we check the ConnectionId of the AlertInfo.
        if (info.ConnectionId != ConnectionId)
            return;

        // global event invoked before any
        // modification to the connection instance
        OnAlertInfo?.Invoke(this, info, now);

        // per alert type event
        switch (info.AlertType) {
            case AlertType.LoginFailed:
                IsLoggedIn = false;
                OnLoginFailed?.Invoke(this, info, now);
                break;
            case AlertType.ForcedLogout:
                IsLoggedIn = false;
                OnForcedLogout?.Invoke(this, info, now);
                break;
            case AlertType.LoginComplete:
                IsLoggedIn = true;
                OnLoginComplete?.Invoke(this, info, now);
                break;
            case AlertType.ShutdownSignal:
                IsLoggedIn = false;
                OnShutdownSignal?.Invoke(this, info, now);
                break;
            case AlertType.ConnectionClosed:
                IsLoggedIn = false;
                OnConnectionClosed?.Invoke(this, info, now);
                break;
            case AlertType.ConnectionOpened:
                OnConnectionOpened?.Invoke(this, info, now);
                break;
            case AlertType.ConnectionBroken:
                IsLoggedIn = false;
                OnConnectionBroken?.Invoke(this, info, now);
                break;
        }

        // set the latest update time and info
        LastAlertTime = now;
        LastAlertInfo = info;
    }

    /// <summary>
    /// A HashSet of RHandlers subscribed to alerts for this connection.
    /// Making sure that the same RHandler is not subscribed twice.
    /// </summary>
    private HashSet<RHandler> _subscribedRHandlers = new();

    /// <summary>
    /// Shortcut to subscribe to alerts for this connection.
    /// </summary>
    public bool TrySubscribeRHandler(RHandler rHandler, IContext ctx) {
        if (_subscribedRHandlers.Contains(rHandler)) return false;
        _subscribedRHandlers.Add(rHandler);
        rHandler.AlertClb.Subscribe(new Context(), HandleAlertInfo);
        return true;
    }

    #region PropertyChanged

    /// <summary>
    /// Handles PropertyChanged events for this connection.
    /// </summary> 
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Subscribe to all PropertyChanged events for this connection.
    /// </summary>
    public void SubscribeToPropertyChangedEvent(
        Action<Connection, PropertyChangedEventArgs> action
    ) {
        PropertyChanged += (sender, args) => { action(this, args); };
    }

    /// <summary>
    /// Subscribe to PropertyChanged events for a specific property.
    /// </summary>
    public void SubscribeToPropertyChangedEvent(
        string                                       propertyName,
        Action<Connection, PropertyChangedEventArgs> action
    ) {
        PropertyChanged += (sender, args) => {
            if (args.PropertyName == propertyName) {
                action(this, args);
            }
        };
    }

    protected virtual void _notifyPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}