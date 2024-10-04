using webapi_tutorial;
using webapi_tutorial.Datos;
using Microsoft.EntityFrameworkCore;
using webapi_tutorial.Repositorio.IRepositorio;
using webapi_tutorial.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(); // Add NewtonsoftJson
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
); // Add Entity Framework Core Connection

builder.Services.AddAutoMapper(typeof(MappingConfig)); // Add AutoMapper
builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>(); // Add service (Design Pattern - Scoped)
builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use routing and enable controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();