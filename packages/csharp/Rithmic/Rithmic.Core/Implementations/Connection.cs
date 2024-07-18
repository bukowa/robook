using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using com.omnesys.rapi;
using Microsoft.Extensions.Logging;
using static Rithmic.Core.LoggingService;

namespace Rithmic.Core;

/// <summary>
/// Wraps <see cref="com.omnesys.rapi.AlertInfo"/>.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public class ConnectionAlert : IConnectionAlert {
    public rapi.AlertInfo AlertInfo { get; }
    public DateTime       DateTime  { get; } = DateTime.UtcNow;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ConnectionAlert(rapi.AlertInfo alertInfo) {
        AlertInfo = alertInfo;
    }
}

/// <summary>
/// Represents a connection to Rithmic.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public class Connection : IConnection {
    /// <summary>
    /// Constructor.
    /// </summary>
    public Connection(
        string            login,
        string            password,
        rapi.ConnectionId connectionId
    ) {
        Login        = login;
        Password     = password;
        ConnectionId = connectionId;
    }

    /// <summary>
    /// Unique identifier for this connection.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public Guid Guid { get; } = Guid.NewGuid();

    /// <summary>
    ///     Login for this connection.
    /// </summary>
    public string Login { get; }

    /// <summary>
    ///     Password for this connection.
    /// </summary>
    public string Password { get; }

    /// <summary>
    ///     ConnectionId for this connection.
    /// </summary>
    public rapi.ConnectionId ConnectionId { get; }


    private bool _isLoggedIn;

    /// <summary>
    ///     Returns boolean indicating if the connection is logged in.
    /// </summary>
    public bool IsLoggedIn {
        get => _isLoggedIn;
        private set => SetField(ref _isLoggedIn, value);
    }

    /// <summary>
    /// Make sure to actually receive the alerts.
    /// </summary>
    public void RegisterRCallbackAlertHandler(IContext ctx, IRCallbacksFacade callbackFacade) {
        callbackFacade.AlertDispatcher.RegisterHandler(new Context(), DispatchAlertInfo);
    }

    /// <summary>
    /// Register a handler for a specific alert type.
    /// </summary>
    public void RegisterHandler(rapi.AlertType eventType, IConnection.ConnectionAlertHandler handler) {
        switch (eventType) {
            case AlertType.LoginFailed:
                OnLoginFailed += handler;
                break;
            case AlertType.ForcedLogout:
                OnForcedLogout += handler;
                break;
            case AlertType.LoginComplete:
                OnLoginComplete += handler;
                break;
            case AlertType.ShutdownSignal:
                OnShutdownSignal += handler;
                break;
            case AlertType.ConnectionOpened:
                OnConnectionOpened += handler;
                break;
            case AlertType.ConnectionClosed:
                OnConnectionClosed += handler;
                break;
            case AlertType.ConnectionBroken:
                OnConnectionBroken += handler;
                break;
        }
    }

    /// <summary>
    /// Unregister a handler for a specific alert type.
    /// </summary>
    public void UnregisterHandler(AlertType eventType, IConnection.ConnectionAlertHandler handler) {
        switch (eventType) {
            case AlertType.LoginFailed:
                OnLoginFailed -= handler;
                break;
            case AlertType.ForcedLogout:
                OnForcedLogout -= handler;
                break;
            case AlertType.LoginComplete:
                OnLoginComplete -= handler;
                break;
            case AlertType.ShutdownSignal:
                OnShutdownSignal -= handler;
                break;
            case AlertType.ConnectionOpened:
                OnConnectionOpened -= handler;
                break;
            case AlertType.ConnectionClosed:
                OnConnectionClosed -= handler;
                break;
            case AlertType.ConnectionBroken:
                OnConnectionBroken -= handler;
                break;
        }
    }

    private event IConnection.ConnectionAlertHandler? OnLoginFailed;
    private event IConnection.ConnectionAlertHandler? OnForcedLogout;
    private event IConnection.ConnectionAlertHandler? OnLoginComplete;
    private event IConnection.ConnectionAlertHandler? OnShutdownSignal;
    private event IConnection.ConnectionAlertHandler? OnConnectionOpened;
    private event IConnection.ConnectionAlertHandler? OnConnectionClosed;
    private event IConnection.ConnectionAlertHandler? OnConnectionBroken;

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
            alert.DateTime
        );

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
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}