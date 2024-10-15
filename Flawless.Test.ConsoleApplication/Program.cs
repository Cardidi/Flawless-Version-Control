// See https://aka.ms/new-console-template for more information

using Flawless.Api;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

var path = "http://localhost:5150";

var rpcChannel = GrpcChannel.ForAddress(path);
var authService = new Auth.AuthClient(rpcChannel);

var result = await authService.GainTokenAsync(new AuthRequest()
{
    UserName = "admin",
    Expires = 10,
    Password = "password"
});
    

if (result.Result == 0)
{
    Console.WriteLine($"Token granted: {result.Token}");
    
    // Thread.Sleep(5 * 1000);
    var userInfo = await authService.GetUserInfoAsync(new Empty(), new Metadata()
    {
        { "Authorization", $"Bearer {result.Token}" }
    });
    Console.WriteLine($"UserName: {userInfo.UserName}\nUID: {userInfo.UserId}\nIs Admin: {userInfo.IsSystemAdmin}");
}