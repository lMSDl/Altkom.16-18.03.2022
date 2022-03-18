using Grpc.Net.Client;
using GrpcService.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ClientConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TestWebAPI();

            //await TestSignalR();

             await TestUnraryGrpc();

            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new GrpcService.Services.GrpcStream.GrpcStreamClient(channel);

            //var stream = client.FromServer(new Request { Text = "Hello!" }, deadline: DateTime.UtcNow.AddSeconds(5));
            //var source = new CancellationTokenSource();
            //var counter = 0;
            //while(await stream.ResponseStream.MoveNext(source.Token))
            //{
            //    Console.WriteLine( $"{counter} {stream.ResponseStream.Current.Text}");
            //    counter++;
            //    if (counter == 10000)
            //    {
            //        source.Cancel();
            //        break;
            //    }
            //}
        }

        private static async Task TestUnraryGrpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcService.Services.GrpcUsers.GrpcUsersClient(channel);
            var grpcUser = new GrpcService.Services.User() { Login = "Grpc", Password = "Service", BirthDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow) };
            var response = await client.CreateAsync(grpcUser);

            var user = await client.ReadByIdAsync(new GrpcService.Services.User() { Id = 0});

            Console.WriteLine(JsonConvert.SerializeObject(user));

            //var request = new GrpcService.HelloRequest() { ClientName = "GrpcClient", Message = "Hi!" };
            //var response = await client.SayHelloAsync(request);
            //Console.WriteLine(response.Message);
        }

        private static async Task TestSignalR()
        {
            var connection = new HubConnectionBuilder()
                            .WithUrl("https://localhost:5001/signalR/users")
                            .WithAutomaticReconnect()
                            .Build();

            connection.Reconnecting += connectionId =>
            {
                Console.WriteLine($"{connectionId}: Reconnecting...  ");
                return Task.CompletedTask;
            };

            connection.Closed += connectionId =>
            {
                Console.WriteLine($"{connectionId}: Closed");
                return Task.CompletedTask;
            };

            connection.On<string>("New Client Arrived!", x => Console.WriteLine($"new client: {x}"));

            connection.On<string>("WelcomeMessage", x => Console.WriteLine(x));

            connection.On<User>("ResetPassword", x => Console.WriteLine($"User {x.Login} has new password!"));

            while (connection.State != HubConnectionState.Connected)
            {
                Console.WriteLine("Connecting...");
                try
                {
                    await connection.StartAsync();
                }
                catch
                {
                    Console.WriteLine("Connection failed!");
                }
            }

            await connection.SendAsync("WelcomeMessage", $"Hello from {connection.ConnectionId}");

            await connection.SendAsync("JoinGroup", "Admins");

            Console.ReadLine();

            await connection.DisposeAsync();
        }

        private static async Task TestWebAPI()
        {
            var client = new WebApiClient("https://localhost:5001/");

            var result = await client.GetStringAsync("weatherForecast");

            var users = await client.GetAsync<IEnumerable<User>>("api/users");

            await client.LoginAsync("api/users/login", new User { Login = "Admin", Password = "@dmin" });
            users = await client.GetAsync<IEnumerable<User>>("api/users");

            var swagger = new swaggerClient("https://localhost:5001/", new HttpClient());

            var weatherResult = await swagger.WeatherForecastAsync();
        }
    }
}
