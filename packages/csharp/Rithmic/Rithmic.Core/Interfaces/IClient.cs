namespace Rithmic.Core;

public interface IRithmicService {

    /// <summary>
    /// Wrapper around <see cref="rapi.RCallbacks"/> for easier event handling.
    /// </summary>
    IRCallbacksFacade RCallbacksFacade { get; set; }

    /// <summary>
    /// Wrapper around <see cref="rapi.AdmCallbacks"/> for easier event handling.
    /// </summary>
    IAdmCallbacksFacade AdmCallbacksFacade { get; set; }

    /// <summary>
    /// Main entrypoint for the Rithmic API.
    /// </summary>
    IREngineOperations? REngineOperations { get; set; }
}

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

/// <summary>
/// Rithmic client interface.
/// </summary>
public interface IRithmicClient {
    /// <summary>
    /// Rithmic API auth.
    /// </summary>
    IRithmicAuth? RithmicAuth { get; }

    /// <summary>
    /// Rithmic API service.
    /// </summary>
    IRithmicService RithmicService { get; }

    /// <summary>
    /// Logs in to the Rithmic API.
    /// </summary>
    Task LoginAsync(IRithmicAuth auth, int timeout);

    /// <summary>
    /// Logs out of the Rithmic API.
    /// </summary>
    Task LogoutAsync(int timeout);
}