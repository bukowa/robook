using System.ComponentModel;

namespace Rithmic.Core;

public interface IRithmicAuth {
    /// <summary>
    /// Login Parameters.
    /// </summary>
    ILParams LParams { get; }

    /// <summary>
    /// Connection Parameters.
    /// </summary>
    CParams CParams { get; }
}

public interface IRithmicService {
    /// <summary>
    /// Main entrypoint for the Rithmic API.
    /// </summary>
    rapi.REngine? REngine { get; }

    /// <summary>
    /// Wrapper around <see cref="rapi.RCallbacks"/> for easier event handling.
    /// </summary>
    IRCallbacks RCallbacks { get; }

    /// <summary>
    /// Wrapper around <see cref="rapi.AdmCallbacks"/> for easier event handling.
    /// </summary>
    IAdmCallbacks AdmCallbacks { get; }
}

/// <summary>
/// Rithmic client interface.
/// </summary>
public interface IRithmicClient : IRithmicService {
    /// <summary>
    /// Logs in to the Rithmic API.
    /// </summary>
    Task LoginAsync(IRithmicAuth auth, int timeout);

    /// <summary>
    /// Logs out of the Rithmic API.
    /// </summary>
    Task LogoutAsync(int timeout);
}