namespace Rithmic.Core;

/// <summary>
/// Defines the contract for a context used in the event dispatcher.
/// </summary>
public interface IContext {
    /// <summary>
    /// Gets or sets the key associated with the context.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Gets or sets the value associated with the context.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the context is one-time use.
    /// </summary>
    bool Once { get; }
}