using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using com.omnesys.rapi;
using Rithmic;
using Robook.Store;
using Application = Rithmic.Application;

namespace Robook.Accounts;

/// <summary>
/// Rithmic account.
/// </summary>
public class Account : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Default application.
    /// </summary>
    [JsonIgnore] public static Application Application = new();

    
    private Client? _client;
    
    /// <summary>
    /// Client for this account.
    /// </summary>
    [JsonIgnore]
    [Browsable(false)]
    public Client? Client {
        get => _client;
        set {
            _client = value;
            _client?.SubscribeToPropertyChangedEvent(nameof(Client.Params), (c, _) => {
                c.Params?.TradingSystemConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Connection.LastConnectionAlert),
                    (c, _) => { NotifyPropertyChanged(nameof(TradingSystemConnectionStatus)); });
                c.Params?.MarketDataConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Connection.LastConnectionAlert),
                    (c, _) => { NotifyPropertyChanged(nameof(MarketDataConnectionStatus)); });
                c.Params?.PnlConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Connection.LastConnectionAlert),
                    (c, _) => { NotifyPropertyChanged(nameof(PnlConnectionStatus)); });
                c.Params?.HistoricalDataConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Connection.LastConnectionAlert),
                    (c, _) => { NotifyPropertyChanged(nameof(HistoricalDataConnectionStatus)); });
            });
        }
    }
    
    /// <summary>
    /// String representation of this account.
    /// </summary>
    public override string ToString() {
        return Login + ":" + Server + ":" + Gateway;
    }

    /// <summary>
    /// Properties that can be edited.
    /// </summary>
    #region PropertyEditable

    private string _login = "";

    public string Login {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    private ProtectedString _password;

    public ProtectedString Password {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _server = "";

    public string Server {
        get => _server;
        set => SetProperty(ref _server, value);
    }

    private string _gateway = "";

    public string Gateway {
        get => _gateway;
        set => SetProperty(ref _gateway, value);
    }

    private bool _plugInMode;

    public bool PlugInMode {
        get => _plugInMode;
        set => SetProperty(ref _plugInMode, value);
    }

    private string? _logFilePath;

    public string? LogFilePath {
        get => _logFilePath;
        set => SetProperty(ref _logFilePath, value);
    }

    #endregion

    #region PropertyConnection

    private bool _pnlConnection;
    private bool _marketDataConnection;
    private bool _tradingSystemConnection;
    private bool _historicalDataConnection;

    [DisplayName("Pnl")]
    public bool PnlConnection {
        get => _pnlConnection;
        set => SetProperty(ref _pnlConnection, value);
    }

    [DisplayName("Market")]
    public bool MarketDataConnection {
        get => _marketDataConnection;
        set => SetProperty(ref _marketDataConnection, value);
    }

    [DisplayName("Trade")]
    public bool TradingSystemConnection {
        get => _tradingSystemConnection;
        set => SetProperty(ref _tradingSystemConnection, value);
    }

    [DisplayName("History")]
    public bool HistoricalDataConnection {
        get => _historicalDataConnection;
        set => SetProperty(ref _historicalDataConnection, value);
    }

    #endregion

    #region PropertyStatus

    [DisplayName("PnlStatus")]
    public string? PnlConnectionStatus
        => Client?.PnlConnection?.LastConnectionAlert?.AlertInfo.Message;

    [DisplayName("MarketStatus")]
    public string? MarketDataConnectionStatus
        => Client?.MarketDataConnection?.LastConnectionAlert?.AlertInfo.Message;

    [DisplayName("TradeStatus")]
    public string? TradingSystemConnectionStatus
        => Client?.TradingSystemConnection?.LastConnectionAlert?.AlertInfo.Message;

    [DisplayName("HistoryStatus")]
    public string? HistoricalDataConnectionStatus
        => Client?.HistoricalDataConnection?.LastConnectionAlert?.AlertInfo.Message;

    #endregion

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
        if (!EqualityComparer<T>.Default.Equals(field, value)) {
            field = value;
            NotifyPropertyChanged(propertyName);
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}