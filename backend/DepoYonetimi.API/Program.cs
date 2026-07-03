using DepoYonetimi.API.Data;
using DepoYonetimi.API.Managers;
using DepoYonetimi.API.Managers.Interfaces;
using DepoYonetimi.API.Repositories;
using DepoYonetimi.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS politikası - frontend için
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// DbContext - SQL Server bağlantısı
builder.Services.AddDbContext<DepoDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));

// Repository Dependency Injection
builder.Services.AddScoped<IUrunRepository, UrunRepository>();
builder.Services.AddScoped<IDepoRepository, DepoRepository>();
builder.Services.AddScoped<IStokRepository, StokRepository>();
builder.Services.AddScoped<IHareketRepository, HareketRepository>();

// Manager Dependency Injection
builder.Services.AddScoped<IUrunManager, UrunManager>();
builder.Services.AddScoped<IDepoManager, DepoManager>();
builder.Services.AddScoped<IStokManager, StokManager>();
builder.Services.AddScoped<IHareketManager, HareketManager>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Depo Yönetimi API", Version = "v1" });
});

var app = builder.Build();

// Seed data - ilk çalıştırmada örnek veri yükle
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DepoDbContext>();
    await SeedData.YukleAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
