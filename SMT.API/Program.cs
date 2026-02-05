using SMT.Application;
using SMT.Domain.Models;
using SMT.Infrastructure;
using SMT.Persistence;
using SMT.Persistence.SMTConfiguration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddPersistence(configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")           // твой фронт
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
            .AllowCredentials();                            // обязательно, если используешь куки / Authorization
    });
});
var provider = builder.Services.BuildServiceProvider();
var context = provider.GetRequiredService<SMTDBContext>();
DbInitialize.Initialize(context);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();