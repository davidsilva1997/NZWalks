using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;
using NZWalks.API.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject FluentValidation
// builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<WalkDifficultyRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<WalkRequestValidator>();


// Inject DBContextClass into services collection
builder.Services.AddDbContext<NZWalksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));
});

// Inject Region Repository
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

// Inject Walk Repository
builder.Services.AddScoped<IWalkRepository, WalkRepository>();

// Inject Walk Difficulty Repository
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

// Inject AutoMapper (Profiles for DTO)
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
