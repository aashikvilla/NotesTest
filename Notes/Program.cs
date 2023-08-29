using MongoDB.Driver;
using Notes.Application.Services.Notes;
using Notes.Application.Validators.Notes;
using Notes.Domain.RepositoryInterfaces;
using Notes.Infrastructure.Data;
using Notes.Infrastructure.Repositories;
using Notes.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//Mongo Db setup
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

MongoDbSettings mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoClient, MongoClient>(_ =>
{
    return new MongoClient(mongoDbSettings?.ConnectionString);
});
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
});

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<NoteDtoValidator>();

builder.Services.AddAutoMapper(typeof(NoteProfile));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();



var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }