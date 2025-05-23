using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp.Shared.Model;
using SamuraiApp_API.Endpoints;
using SamuraiApp_Model;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>
    (options=>options.SerializerOptions.ReferenceHandler =
    ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<SamuraiAppContext>();
builder.Services.AddIdentityApiEndpoints<AccessUser>()
    .AddEntityFrameworkStores<SamuraiAppContext>();
builder.Services.AddAuthorization();
builder.Services.AddTransient<DAL<Samurai>>();
builder.Services.AddTransient<DAL<Dojo>>();
builder.Services.AddTransient<DAL<Kenjutsu>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();

app.MapGroup("auth").MapIdentityApi<AccessUser>().WithTags("Authorization");

app.MapPost("auth/logout", async
    ([FromServices] SignInManager<AccessUser> manager) =>
{
    await manager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization().WithTags("Authorization");

app.AddEndpointsSamurai();
app.AddEndpointsDojo();
app.AddEndpointsKenjutsu();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
