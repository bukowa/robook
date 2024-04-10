using System.Collections.ObjectModel;
using com.omnesys.rapi;
using Microsoft.VisualBasic;
using Rithmic;

var rapiconfig = @"C:\Users\buk\Documents\RApiConfig";

var login       = Environment.GetEnvironmentVariable("TJ_LOGIN");
var pass        = Environment.GetEnvironmentVariable("TJ_PASS");
var system      = Environment.GetEnvironmentVariable("TJ_SYSTEM");
var gatewayName = Environment.GetEnvironmentVariable("TJ_GATEWAY");


var cParamsSource = new CParamsSource(rapiconfig);
var cParams       = cParamsSource.GetCParams(system, gatewayName);
var client        = new Client();
var loginParams   = new LoginParams(cParams);
loginParams.PnlConnection           = new Connection(login, pass, ConnectionId.PnL);
loginParams.TradingSystemConnection = new Connection(login, pass, ConnectionId.TradingSystem);
loginParams.HistoricalDataConnection = new Connection(login, pass, ConnectionId.History);
loginParams.PlugInMode               = false;
loginParams.MarketDataConnection     = new Connection(login, pass, ConnectionId.MarketData);
Console.WriteLine($"Login...{cParams.SystemName}:{cParams.GatewayName}");

//
var loggedIn = client.LoginAndWait(loginParams);
Console.WriteLine("Done...");
if (!loggedIn) {
    Console.WriteLine("Login failed...");
    return;
}
else {
    Console.WriteLine("Login success...");
}

//
var job = new TaskCompletionSource<Collection<AccountInfo>>();
client.RHandler.AccountListClb.Subscribe(new Context(), (context, info) => {
    Console.WriteLine(info.Accounts.Count);
    job.SetResult(info.Accounts);
});
Console.WriteLine("Get accounts...");
client.Engine.getAccounts("");
await job.Task;
//
var job2 = new TaskCompletionSource<Collection<LineInfo>>();
var ctx2 = new ContextOnce();
client.RHandler.OrderReplayCallbacks.Subscribe(ctx2, (context, info) => {
    Console.WriteLine(info.Orders.Count);
    job2.SetResult(info.Orders);
});
Console.WriteLine("Replay orders...");
client.Engine.replayHistoricalOrders(job.Task.Result.First(), "20240410", ctx2);
await job2.Task;
client.Engine.shutdown();