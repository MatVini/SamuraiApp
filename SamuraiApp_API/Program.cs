using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp_API.Endpoints;
using SamuraiApp_Model;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options => options.SerializerOptions.ReferenceHandler =
    ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<SamuraiAppContext>();
builder.Services.AddTransient<DAL<Samurai>>();
builder.Services.AddTransient<DAL<Dojo>>();

var app = builder.Build();

app.AddEndpointsSamurai();
app.AddEndpointsDojo();

app.Run();
