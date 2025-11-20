using ExBot.Application;
using ExBot.Domain;
using ExBot.Infrastructure.Cosmos;
using ExBot.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Azure Entra ID authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddAuthorization();

// Add services to the container
builder.Services.AddControllers();

// Add API documentation with Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  add the domain layer
//  , useDatabase: builder.Configuration.GetValue<bool>("UseDatabase", false)
builder.Services.AddDomain(builder.Configuration);
//  add data layer cosmos and sql
builder.Services.AddSqlData(builder.Configuration);
builder.Services.AddCosmosData(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

//  ensure the databases are created and migrated
await app.Services.InitialiseSqlAsync();
await app.Services.InitialiseCosmosAsync();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ExBot API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
        
        // Configure OAuth2 for Swagger UI
        options.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
