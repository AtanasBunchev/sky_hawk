using Docker.DotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SkyHawk.ApplicationServices.Implementation;
using SkyHawk.ApplicationServices.Interfaces;
using SkyHawk.Data.Contexts;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<SkyHawkDbContext>();
builder.Services.AddSingleton<IDockerClient>(serviceProvider => new DockerClientConfiguration().CreateClient());

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IServersService, ServersService>();
builder.Services.AddScoped<ISnapshotsService, SnapshotsService>();


var signingKey = new SymmetricSecurityKey(
    Encoding.ASCII.GetBytes("785e26cdd9464e8b92c0cd41ae8df74c"));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = "SkyHawk",
    ValidAudience = "SkyHawk",

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = tokenValidationParameters);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseStatusCodePages(); // https://stackoverflow.com/questions/61150329/
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
