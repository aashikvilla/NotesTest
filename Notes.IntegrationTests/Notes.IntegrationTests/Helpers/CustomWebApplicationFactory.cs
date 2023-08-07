using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Notes.Constants;
using Notes.Infrastructure.Data;

namespace Notes.IntegrationTests.Helpers
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public IConfiguration Configuration { get; private set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .AddJsonFile(ConfigConstants.IntegrationAppSettingsFile)
                    .Build();

                config.AddConfiguration(Configuration);
            });

            builder.ConfigureServices(services =>
            {
                var mongoDbSettings = Configuration.GetSection(ConfigConstants.MongoDbSettings).Get<MongoDbSettings>();

                // Setup MongoClient
                services.AddSingleton<IMongoClient, MongoClient>(_ =>
                {
                    return new MongoClient(mongoDbSettings?.ConnectionString);
                });

                // Setup MongoDatabase
                services.AddSingleton<IMongoDatabase>(sp =>
                {
                    var mongoClient = sp.GetRequiredService<IMongoClient>();
                    return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database              
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<IMongoDatabase>();


                try
                {
                    Utilities.ReinitializeDbForTests(db, mongoDbSettings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured while seeding data for Test Db: {ex}");
                }
            });

        }
    }
}
