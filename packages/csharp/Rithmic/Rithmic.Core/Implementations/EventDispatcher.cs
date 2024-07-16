using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using com.omnesys.rapi;
using Microsoft.Extensions.Logging;
using static Rithmic.Core.LoggingService;

namespace Rithmic.Core;

/// <summary>
/// Event dispatcher for Rithmic callbacks.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventDispatcher<T> : IEventDispatcher<T> {
    private readonly ConcurrentDictionary<IContext, List<Action<IContext, T>>> _callbacks = new();

    /// <summary>
    /// Adds a new event handler to the dictionary.
    /// </summary>
    /// <param name="ctx"> Context associated with the event handler. </param>
    /// <param name="callback"> Event handler to add. </param>
    public void RegisterHandler(IContext ctx, Action<IContext, T> callback) {
        _callbacks.TryAdd(ctx, []);
        _callbacks[ctx].Add(callback);
    }

    /// <summary>
    ///     Removes the event handler associated with the specified context.
    /// </summary>
    /// <param name="ctx"> Context associated with the event handler. </param>
    public void UnregisterHandler(IContext ctx) {
        _callbacks.TryRemove(ctx, out _);
    }

    /// <summary>
    ///     Removes the event handler associated with the specified key.
    /// </summary>
    /// <param name="key"> Context key associated with the event handler. </param>
    public void UnregisterHandler(string key) {
        foreach (var ctx in _callbacks.Keys) {
            if (ctx.Key == key) {
                UnregisterHandler(ctx);
            }
        }
    }

    /// <summary>
    ///     Invokes the event handler associated with the specified context.
    /// </summary>
    /// <param name="ctx"> Context associated with the event handler. </param>
    /// <param name="info"> Event information. </param>
    public void Dispatch(IContext ctx, T info) {
        if (_callbacks.TryGetValue(ctx, out var callback)) {
            foreach (var cb in callback) {
                cb(ctx, info);
            }

            if (ctx.Once) {
                UnregisterHandler(ctx);
            }
        }
    }

    /// <summary>
    ///     Invokes all event handlers for the specified event information.
    ///     Passes new <see cref="Context"/> for each event handler with the specified key.
    /// </summary>
    /// <param name="info"></param>
    public void Dispatch(T info) {
        foreach (var kvp in _callbacks) {
            foreach (var cb in kvp.Value) {
                cb(kvp.Key, info);
                if (kvp.Key.Once) {
                    UnregisterHandler(kvp.Key);
                }
            }
        }
    }
}
