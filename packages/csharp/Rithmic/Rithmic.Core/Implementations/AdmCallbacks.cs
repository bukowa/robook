namespace Rithmic.Core;

public class AdmCallbacksFacade : rapi.AdmCallbacks, IAdmCallbacksFacade {
    
    public rapi.AdmCallbacks GetAdmCallbacks() {
        return this;
    }

    public override void Alert(rapi.AlertInfo info) =>
        AlertDispatcher.Dispatch(info);

    public override void EnvironmentList(rapi.EnvironmentListInfo info) =>
        EnvironmentListDispatcher.Dispatch(info);

    public override void Environment(rapi.EnvironmentInfo oInfo) =>
        EnvironmentDispatcher.Dispatch(oInfo);

    public IEventDispatcher<rapi.AlertInfo>           AlertDispatcher           { get; } = new EventDispatcher<rapi.AlertInfo>();
    public IEventDispatcher<rapi.EnvironmentInfo>     EnvironmentDispatcher     { get; } = new EventDispatcher<rapi.EnvironmentInfo>();
    public IEventDispatcher<rapi.EnvironmentListInfo> EnvironmentListDispatcher { get; } = new EventDispatcher<rapi.EnvironmentListInfo>();
}