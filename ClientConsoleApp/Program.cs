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

            using var client = new WebApiClient("https://localhost:5001/");

            var result = await client.GetStringAsync("weatherForecast");

            var users = await client.GetAsync<IEnumerable<User>>("api/users");

            await client.LoginAsync("api/users/login", new User { Login = "Admin", Password = "@dmin" });
            users = await client.GetAsync<IEnumerable<User>>("api/users");
        }
    }
}
