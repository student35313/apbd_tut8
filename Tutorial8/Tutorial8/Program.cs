using Tutorial8.Repositories;
using Tutorial8.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<ITripsRepository, TripsRepository>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

//using System;
// Program.cs
// using System;
// using Microsoft.Data.SqlClient;
//
// class Program
// {
//     static void Main()
//     {
//         var config = new ConfigurationBuilder()
//             .AddJsonFile("appsettings.json")
//             .Build();
//         var connectionString = config.GetConnectionString("Default");
//         // 2. Открываем соединение
//         using var conn = new SqlConnection(connectionString);
//         conn.Open();
//         Console.WriteLine("Connected to SQL Server!");
//
//         // 3. Простейший SELECT: покажем первые 5 туров
//         using (var cmd = new SqlCommand("SELECT TOP 5 IdTrip, Name, DateFrom, DateTo FROM Trip", conn))
//         using (var reader = cmd.ExecuteReader())
//         {
//             Console.WriteLine();
//             Console.WriteLine("=== Top 5 Trips ===");
//             while (reader.Read())
//             {
//                 int id   = reader.GetInt32(0);
//                 string name = reader.GetString(1);
//                 DateTime from = reader.GetDateTime(2);
//                 DateTime to   = reader.GetDateTime(3);
//                 Console.WriteLine($"{id}: {name} ({from:yyyy-MM-dd} → {to:yyyy-MM-dd})");
//             }
//         }
//
//         // 4. Простейший INSERT: добавим тестового клиента
//         Console.WriteLine();
//         Console.WriteLine("=== Inserting new test client ===");
//         using (var cmd = new SqlCommand(
//             "INSERT INTO Client (FirstName, LastName, Email) VALUES ('Test', 'User', 'test@example.com'); SELECT SCOPE_IDENTITY();",
//             conn))
//         {
//             var newId = Convert.ToInt32(cmd.ExecuteScalar());
//             Console.WriteLine($"Inserted Client.IdClient = {newId}");
//         }
//
//         // 5. SELECT клиентов, чтобы убедиться, что он в таблице
//         using (var cmd = new SqlCommand("SELECT TOP 5 IdClient, FirstName, LastName FROM Client", conn))
//         using (var reader = cmd.ExecuteReader())
//         {
//             Console.WriteLine();
//             Console.WriteLine("=== Top 5 Clients ===");
//             while (reader.Read())
//             {
//                 Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)} {reader.GetString(2)}");
//             }
//         }
//
//         Console.WriteLine();
//         Console.WriteLine("Done. Press any key to exit.");
//         Console.ReadKey();
//     }
// }    