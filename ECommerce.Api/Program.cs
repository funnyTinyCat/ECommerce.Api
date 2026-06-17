using ECommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using ECommerce.Api.DTOs;
using ECommerce.Api.Mappings;
using ECommerce.Api.Services.Interfaces;
using ECommerce.Api.Services;
using ECommerce.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddMaps(typeof(ProductProfile).Assembly);
});
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
        options.RoutePrefix = "swagger"; // Access via localhost:port/swagger
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
