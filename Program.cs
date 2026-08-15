using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CreateApplicationHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateApplicationValidator>();

builder.Services.AddScoped<GetApplicationByIdHandeler>();
builder.Services.AddScoped<GetApplicationsHandeler>();
builder.Services.AddScoped<UpdateApplicationHandeler>();
builder.Services.AddScoped<UpdateApplicationValidator>();
builder.Services.AddScoped<AppicationDeleteHandeler>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApplyTrack API V1");
    c.RoutePrefix = string.Empty;
});


app.MapApplicationEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();




app.Run();

