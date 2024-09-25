using CrudAppStorm.app.Database;
using CrudAppStorm.app.Extensions;
using CrudAppStorm.app.Repositories;
using CrudAppStorm.app.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<ProductContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));



var app = builder.Build();

app.AddDataBaseSeedingService();

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
