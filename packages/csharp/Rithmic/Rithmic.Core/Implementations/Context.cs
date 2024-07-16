namespace Rithmic.Core;

/// <summary>
///     Represents contextual information associated with an event handler.
/// </summary>
public class Context : IContext {
    /// <summary>
    ///     Gets or sets the key associated with the context.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Gets or sets the optional value associated with the context.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    ///     Indicates whether the event handler should be removed after the first invocation.
    /// </summary>
    public bool Once { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Core.Interfaces.IContext"/> class with the specified key and optional value.
    /// </summary>
    /// <param name="value">The optional value associated with the context.</param>
    public Context(object? value = null) {
        Key   = Guid.NewGuid().ToString();
        Value = value;
    }
}

/// <summary>
///    Represents contextual information associated with an event handler.
///    The event handler will be removed after the first invocation.
/// </summary>
public class ContextOnce : Context {
    /// <summary>
    ///    Initializes a new instance of the <see cref="ContextOnce"/>.
    /// </summary>
    /// <param name="value"></param>
    public ContextOnce(object? value = null) :
        base(value) {
        Once = true;
    }
}