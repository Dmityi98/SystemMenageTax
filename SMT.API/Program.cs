using SMT.Application;
using SMT.Domain.Models;
using SMT.Persistence;
using SMT.Persistence.SMTConfiguration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddPersistence(configuration);
builder.Services.AddApplication();
// Add services to the container.
Console.WriteLine(Guid.NewGuid());

var provider = builder.Services.BuildServiceProvider();
var context = provider.GetRequiredService<SMTDBContext>();
DbInitialize.Initialize(context);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();