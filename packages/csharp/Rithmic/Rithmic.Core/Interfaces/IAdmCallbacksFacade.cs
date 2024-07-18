// ReSharper disable RedundantNameQualifier

namespace Rithmic.Core;

public interface IAdmCallbacksFacade {
    rapi.AdmCallbacks                          GetAdmCallbacks();
    IEventDispatcher<rapi.AlertInfo>           AlertDispatcher           { get; }
    IEventDispatcher<rapi.EnvironmentInfo>     EnvironmentDispatcher     { get; }
    IEventDispatcher<rapi.EnvironmentListInfo> EnvironmentListDispatcher { get; }
}