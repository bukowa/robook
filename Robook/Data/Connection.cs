using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using com.omnesys.rapi;
using Rithmic;
using Robook.Store;
using Application = Rithmic.Application;

namespace Robook.Data;

/// <summary>
/// Connection represents Rithmic connection.
/// </summary>
public class Connection : INotifyPropertyChanged {
    /// <summary>
    /// String representation of this connection.
    /// </summary>
    public string DisplayName =>
        $"{Login}:{Server}:{Gateway}";

    /// <summary>
    /// Ability to reference this connection as "ValueMember".
    /// </summary>
    [JsonIgnore]
    public Connection Self => this;

    /// <summary>
    /// Unique identifier for this connection.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    # region CPARAMS

    [Browsable(false)] private CParamsSource CParamsSource => new(CParamsDirPath);

    [Browsable(false)]
    private CParams CParams {
        get {
            var x = CParamsSource.GetCParams(Server, Gateway);
            x.LogFilePath = LogFilePath;
            return x;
        }
    }

    [Browsable(false)]
    private Rithmic.Connection? CreateConnection(bool shouldCreate, ConnectionId id) {
        return shouldCreate ? new Rithmic.Connection(Login, Password.Decrypted(), id) : null;
    }

    [Browsable(false)]
    private LoginParams LoginParams => new(CParams) {
        PlugInMode               = PlugInMode,
        PnlConnection            = CreateConnection(PnlConnection,            ConnectionId.PnL),
        HistoricalDataConnection = CreateConnection(HistoricalDataConnection, ConnectionId.History),
        MarketDataConnection     = CreateConnection(MarketDataConnection,     ConnectionId.MarketData),
        TradingSystemConnection  = CreateConnection(TradingSystemConnection,  ConnectionId.TradingSystem),
    };

    #endregion

    public Task LoginAsync() {
        return Task.Run(() => {
            Client ??= new Client();
            Client?.LoginAndWait(LoginParams);
        });
    }

    public Task LogoutAsync() {
        return Task.Run(() => { Client?.LogoutAndWait(); });
    }

    /// <summary>
    /// Application used for this connection.
    /// </summary>
    [JsonIgnore] public static Application Application = new();

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
                    nameof(Rithmic.Connection.LastConnectionAlert),
                    (_, _) => { NotifyPropertyChanged(nameof(TradingSystemConnectionStatus)); });
                c.Params?.MarketDataConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Rithmic.Connection.LastConnectionAlert),
                    (_, _) => { NotifyPropertyChanged(nameof(MarketDataConnectionStatus)); });
                c.Params?.PnlConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Rithmic.Connection.LastConnectionAlert),
                    (_, _) => { NotifyPropertyChanged(nameof(PnlConnectionStatus)); });
                c.Params?.HistoricalDataConnection?.SubscribeToPropertyChangedEvent(
                    nameof(Rithmic.Connection.LastConnectionAlert),
                    (_, _) => { NotifyPropertyChanged(nameof(HistoricalDataConnectionStatus)); });
            });
            NotifyPropertyChanged();
        }
    }

    private Client? _client;
    
    /// <summary>
    /// Properties that can be edited by the user.
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

    private string? _cParamsDirPath;

    public string CParamsDirPath {
        get => _cParamsDirPath;
        set => SetProperty(ref _cParamsDirPath, value);
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

    [JsonIgnore]
    [DisplayName("PnlStatus")]
    public string? PnlConnectionStatus
        => Client?.PnlConnection?.LastConnectionAlert?.AlertInfo.Message;

    [JsonIgnore]
    [DisplayName("MarketStatus")]
    public string? MarketDataConnectionStatus
        => Client?.MarketDataConnection?.LastConnectionAlert?.AlertInfo.Message;

    [JsonIgnore]
    [DisplayName("TradeStatus")]
    public string? TradingSystemConnectionStatus
        => Client?.TradingSystemConnection?.LastConnectionAlert?.AlertInfo.Message;

    [JsonIgnore]
    [DisplayName("HistoryStatus")]
    public string? HistoricalDataConnectionStatus
        => Client?.HistoricalDataConnection?.LastConnectionAlert?.AlertInfo.Message;

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;

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