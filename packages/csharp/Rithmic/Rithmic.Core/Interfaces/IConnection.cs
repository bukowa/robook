using System.ComponentModel;

namespace Rithmic.Core;

/// <summary>
/// Interface for a connection alert in the Rithmic system.
/// </summary>
public interface IConnectionAlert {
    /// <summary>
    /// Gets the date and time when the alert occurred.
    /// </summary>
    DateTime DateTime { get; }

    /// <summary>
    /// Gets the underlying alert information from Rithmic.
    /// </summary>
    rapi.AlertInfo AlertInfo { get; }
}

public interface IConnectionAlertHandler {
    /// <summary>
    /// Handler for connection alerts.
    /// </summary>
    delegate void ConnectionAlertHandler(IConnection connection, IConnectionAlert alert);

    /// <summary>
    /// Register to receive alerts from the specific IRCallback.
    /// </summary>
    void RegisterRCallbackAlertHandler(IContext ctx, IRCallbacks callback);

    /// <summary>
    /// Registers an alert handler for a specific alert type.
    /// </summary>
    void RegisterHandler(rapi.AlertType eventType, ConnectionAlertHandler handler);

    /// <summary>
    /// Unregisters a previously registered alert handler for a specific alert type.
    /// </summary>
    void UnregisterHandler(rapi.AlertType eventType, ConnectionAlertHandler handler);
}

/// <summary>
/// Interface for a connection to the Rithmic trading platform.
/// </summary>
public interface IConnection : IConnectionAlertHandler, INotifyPropertyChanged {
    /// <summary>
    /// Gets the unique connection identifier.
    /// </summary>
    rapi.ConnectionId ConnectionId { get; }

    /// <summary>
    /// Gets the login name used for the connection.
    /// </summary>
    string Login { get; }

    /// <summary>
    /// Gets the password used for the connection.
    /// Note: Consider using SecureString for sensitive data in production environments.
    /// </summary>
    string Password { get; }

    /// <summary>
    /// Gets a value indicating whether the connection is currently logged in.
    /// </summary>
    bool IsLoggedIn { get; }
}