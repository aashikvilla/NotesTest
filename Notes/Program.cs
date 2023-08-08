using Notes.Application.Validators.Notes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<NoteDtoValidator>();

builder.Services.AddAutoMapper(typeof(NoteProfile));

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }