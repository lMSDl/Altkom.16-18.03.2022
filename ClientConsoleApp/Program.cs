using Microsoft.AspNetCore.SignalR.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TestWebAPI();

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
