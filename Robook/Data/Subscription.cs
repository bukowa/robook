using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using com.omnesys.rapi;
using Rithmic;
using Robook.SymbolNS;

namespace Robook.Data;

public class Subscriber {
    public ConcurrentQueue<object> Queue { get; } = new();
}

public class Subscription: INotifyPropertyChanged {
    [Browsable(false)] public Client Client { get; }

    [Browsable(false)] public Symbol Symbol { get; }

    [Browsable(false)] public ConcurrentQueue<object> Queue { get; } = new();

    [DisplayName("Symbol")]
    public string? SymbolName => Symbol?.Name;
    
    [DisplayName("Exchange")]
    public string? Exchange => Symbol?.Exchange;
    
    [Browsable(false)]
    public ConcurrentBag<Subscriber> Subscribers { get; } = new();

    public void Subscribe(Subscriber subscriber) {
        Subscribers.Add(subscriber);
    }

    private IContext Context => new Context();

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
                switch (client.MarketDataConnection) {
                    case null:
                        _unsubscribeStream();
                        break;
                    default:
                        _subscribeStream();
                        break;
                }
            });
        _subscribeStream();
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