using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using com.omnesys.rapi;
using Microsoft.Extensions.Logging;
using static Rithmic.LoggingService;

namespace Rithmic;

/// <summary>
/// Wraps <see cref="com.omnesys.rapi.AlertInfo"/>.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public class ConnectionAlert(com.omnesys.rapi.AlertInfo alertInfo) {
    public com.omnesys.rapi.AlertInfo AlertInfo { get; } = alertInfo;
    public DateTime                   Time      { get; } = DateTime.UtcNow;
}

/// <summary>
/// Represents a connection to Rithmic.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public sealed class Connection : INotifyPropertyChanged {
    /// <summary>
    /// Unique identifier for this connection.
    /// </summary>
    public Guid Guid { get; } = Guid.NewGuid();

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
    public com.omnesys.rapi.ConnectionId ConnectionId;

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
    public ConnectionAlert? LastConnectionAlert {
        get => _connectionConnectionAlert;
        set {
            _connectionConnectionAlert = value;
            _notifyPropertyChanged();
        }
    }

    private ConnectionAlert? _connectionConnectionAlert;

    /// <summary>
    /// Delegate for handling alerts for this connection.
    /// </summary>
    public delegate void AlertInfoHandler(Connection sender, ConnectionAlert alert);

    /// <summary>
    /// <see ref="`OnAlertInfo`"/> is invoked before any modification to the connection
    /// instance is made. It is an event for all alert types. Other alert type events
    /// are invoked after this event and after the instance attributes are modified.
    /// </summary>
    public event AlertInfoHandler? OnAlertInfo;

    public event AlertInfoHandler? OnLoginFailed;
    public event AlertInfoHandler? OnForcedLogout;
    public event AlertInfoHandler? OnLoginComplete;
    public event AlertInfoHandler? OnShutdownSignal;
    public event AlertInfoHandler? OnConnectionOpened;
    public event AlertInfoHandler? OnConnectionClosed;
    public event AlertInfoHandler? OnConnectionBroken;

    // these methods are a convenience way to subscribe to events
    // without having to check for the null value of the Connection
    // like this: if (MarketDataConnection != null) { ... }
    // instead, you can use this: MarketDataConnection?.SubscribeToOnAlertInfo(...);

    public void SubscribeToAlertInfo(AlertInfoHandler? action) {
        OnAlertInfo += action;
    }

    public void SubscribeToLoginFailed(AlertInfoHandler? action) {
        OnLoginFailed += action;
    }

    public void SubscribeToForcedLogout(AlertInfoHandler? action) {
        OnForcedLogout += action;
    }

    public void SubscribeToOnLoginComplete(AlertInfoHandler? action) {
        OnLoginComplete += action;
    }

    public void SubscribeToOnShutdownSignal(AlertInfoHandler? action) {
        OnShutdownSignal += action;
    }

    public void SubscribeToOnConnectionOpened(AlertInfoHandler? action) {
        OnConnectionOpened += action;
    }

    public void SubscribeToOnConnectionClosed(AlertInfoHandler? action) {
        OnConnectionClosed += action;
    }

    public void SubscribeToOnConnectionBroken(AlertInfoHandler? action) {
        OnConnectionBroken += action;
    }

    public void UnsubscribeFromOnAlertInfo(AlertInfoHandler? action) {
        OnAlertInfo -= action;
    }

    public void UnsubscribeFromOnLoginFailed(AlertInfoHandler? action) {
        OnLoginFailed -= action;
    }

    public void UnsubscribeFromOnForcedLogout(AlertInfoHandler? action) {
        OnForcedLogout -= action;
    }

    public void UnsubscribeFromOnLoginComplete(AlertInfoHandler? action) {
        OnLoginComplete -= action;
    }

    public void UnsubscribeFromOnShutdownSignal(AlertInfoHandler? action) {
        OnShutdownSignal -= action;
    }

    public void UnsubscribeFromOnConnectionOpened(AlertInfoHandler? action) {
        OnConnectionOpened -= action;
    }

    public void UnsubscribeFromOnConnectionClosed(AlertInfoHandler? action) {
        OnConnectionClosed -= action;
    }

    public void UnsubscribeFromOnConnectionBroken(AlertInfoHandler? action) {
        OnConnectionBroken -= action;
    }

    /// <summary>
    /// Handles alerts for this connection.
    /// </summary>
    private void DispatchAlertInfo(IContext ctx, com.omnesys.rapi.AlertInfo alertInfo) {
        // build the ConnectionAlert instance
        var alert = new ConnectionAlert(alertInfo);

        // AlertInfo as received from Rithmic does not pass any Context -
        // so we cannot route it to the correct connection based on the Context.
        // to make sure that the AlertInfo is for this connection,
        // we check the ConnectionId of the AlertInfo.
        if (alertInfo.ConnectionId != ConnectionId)
            return;

        // log
        Logger?.LogInformation(
            "Connection: {@Guid} {@ConnectionId} {@AlertType} {@RpCode} {@AlertMessage} {@Time}",
            Guid,
            alert.AlertInfo.ConnectionId,
            alert.AlertInfo.AlertType,
            alert.AlertInfo.RpCode,
            alert.AlertInfo.Message,
            alert.Time
        );
        
        // global event
        OnAlertInfo?.Invoke(this, alert);

        // per alert type event
        switch (alertInfo.AlertType) {
            case AlertType.LoginFailed:
                IsLoggedIn = false;
                OnLoginFailed?.Invoke(this, alert);
                break;
            case AlertType.ForcedLogout:
                IsLoggedIn = false;
                OnForcedLogout?.Invoke(this, alert);
                break;
            case AlertType.LoginComplete:
                IsLoggedIn = true;
                OnLoginComplete?.Invoke(this, alert);
                break;
            case AlertType.ShutdownSignal:
                IsLoggedIn = false;
                OnShutdownSignal?.Invoke(this, alert);
                break;
            case AlertType.ConnectionClosed:
                IsLoggedIn = false;
                OnConnectionClosed?.Invoke(this, alert);
                break;
            case AlertType.ConnectionOpened:
                OnConnectionOpened?.Invoke(this, alert);
                break;
            case AlertType.ConnectionBroken:
                IsLoggedIn = false;
                OnConnectionBroken?.Invoke(this, alert);
                break;
        }

        // set the last alert info only after all events have been invoked
        LastConnectionAlert = alert;
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
        rHandler.AlertClb.Subscribe(new Context(), DispatchAlertInfo);
        return true;
    }

    #region PropertyChanged

    /// <summary>
    /// Handles PropertyChanged events for this connection.
    /// </summary> 
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Subscribe to PropertyChanged events for a specific property.
    /// </summary>
    public void ObservePropertyChange(
        string                                       propertyName,
        Action<Connection, PropertyChangedEventArgs> action
    ) {
        PropertyChanged += (sender, args) => {
            if (args.PropertyName == propertyName) {
                action(this, args);
            }
        };
    }

    private void _notifyPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}