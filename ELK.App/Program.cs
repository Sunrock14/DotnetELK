using ELK.App.Services;
using ELK.FakeData.Services;
using ELK.FakeData.Utils;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddTransient<DataGenerator>();
builder.Services.Configure<ElasticsearchSettings>(builder.Configuration.GetSection("ElasticsearchSettings"));
builder.Services.AddScoped<IDataService, DataManager>();
builder.Services.AddScoped<IElasticService, ElasticService>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
