using System.Reflection;
using ExpertAdministration.Server.Interfaces;
using ExpertAdministration.Server.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7165").AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
