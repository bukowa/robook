namespace Rithmic.Core;

public interface IEventDispatcher<T> {
    /// <summary>
    /// Invokes all event handlers associated with a specified context.
    /// </summary>
    /// <param name="ctx">The context associated with the event handlers.</param>
    /// <param name="info">The event information to pass to the handlers.</param>
    void Dispatch(IContext ctx, T info);

    /// <summary>
    /// Invokes all event handlers with the specified event information.
    /// </summary>
    /// <param name="info">The event information to pass to all handlers.</param>
    void Dispatch(T info);

    /// <summary>
    /// Subscribes an event handler to a specified context.
    /// </summary>
    /// <param name="ctx">The context associated with the event handler.</param>
    /// <param name="callback">The event handler to subscribe.</param>
    void RegisterHandler(IContext ctx, Action<IContext, T> callback);

    /// <summary>
    /// Unsubscribes all event handlers associated with a specified context.
    /// </summary>
    /// <param name="ctx">The context associated with the event handler.</param>
    void UnregisterHandler(IContext ctx);

    /// <summary>
    /// Unsubscribes all event handlers associated with a specified key.
    /// </summary>
    /// <param name="key">The key associated with the event handlers.</param>
    void UnregisterHandler(string key);
}
