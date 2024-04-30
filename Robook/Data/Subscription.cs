using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using com.omnesys.rapi;
using Rithmic;
using Robook.Accounts;

namespace Robook.Data;

public class Subscriber {
    public ConcurrentQueue<object> Queue { get; } = new();
}

public class Subscription : INotifyPropertyChanged {
    [Browsable(false)] public Client Client { get; }

    #region Connection

    private Connection? _connection;

    [JsonIgnore]
    public Connection? Connection {
        get => _connection;
        set {
            _connection = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ConnectionId));
        }
    }

    public string? ConnectionId => Connection?.Id;

    #endregion

    public Symbol? Symbol { get; set; }

    [Browsable(false)] public ConcurrentQueue<object> Queue { get; } = new();

    [Browsable(false)] public ConcurrentBag<Subscriber> Subscribers { get; } = new();

    private IContext Context => new Context();

    public void Subscribe(Subscriber subscriber) {
        Subscribers.Add(subscriber);
    }

    private void _subscribeStream() {
        Client.Engine.subscribe(
            Symbol.Exchange, Symbol.Name,
            SubscriptionFlags.All, Context
        );
    }

    private void _unsubscribeStream() {
        Client.Engine.unsubscribe(
            Symbol.Exchange, Symbol.Name
        );
    }

    public void StopStream() {
        _unsubscribeStream();
    }

    private CancellationTokenSource _cts = new();

    public void StartStream() {
        Client.SubscribeToPropertyChangedEvent(
            nameof(Client.MarketDataConnection),
            (client, args) => {
                client.MarketDataConnection?.SubscribeToOnLoginComplete(
                    (sender, alert, time) => { _subscribeStream(); });
            });
        if (Client.MarketDataConnection?.IsLoggedIn ?? false) {
            _subscribeStream();
        }
    }

    public Task StartAsync() {
        SpinWait sw = new();

        while (!_cts.Token.IsCancellationRequested) {
            while (Queue.TryDequeue(out var o)) {
                foreach (var subscriber in Subscribers) {
                    subscriber.Queue.Enqueue(o);
                }
            }

            sw.SpinOnce();
        }

        return Task.CompletedTask;
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