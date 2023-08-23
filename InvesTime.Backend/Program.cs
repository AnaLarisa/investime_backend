using System.Reflection;
using System.Text;
using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;

namespace InvesTime.BackEnd;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("MongoDb")!;
        var databaseName = Environment.GetEnvironmentVariable("MongoDbDatabaseName");

        builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        builder.Services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });


        builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRegistrationRequestRepository, RegistrationRequestRepository>();
        builder.Services.AddScoped<IArticleFromManagerRepository, ArticleFromManagerRepository>();
        builder.Services.AddScoped<IUserStatisticsRepository, UserStatisticsRepository>();
        builder.Services.AddScoped<INewsApiRepository>(_ => new NewsApiRepository(config));


        builder.Services.AddScoped<IMeetingService, MeetingService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRegistrationRequestService, RegistrationRequestService>();
        builder.Services.AddScoped<IArticleFromManagerService, ArticleFromManagerService>();
        builder.Services.AddScoped<IUserStatisticsService, UserStatisticsService>();
        builder.Services.AddScoped<INewsService, NewsService>();

        builder.Services.AddScoped<IUserHelper, UserHelper>();

        builder.Services.AddHttpContextAccessor();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });


        builder.Services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("Authentication:Token").Value!))
            };
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}