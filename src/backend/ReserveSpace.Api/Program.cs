using Microsoft.EntityFrameworkCore;
using ReserveSpace.Api.Data;
using ReserveSpace.Api.Services.Abstractions;
using ReserveSpace.Api.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// 1. База даних
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Сервіси бізнес-логіки
builder.Services.AddScoped<IPricingCalculator, PricingCalculator>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// 3. Контролери та OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Автоматичне застосування міграцій та посів початкових даних (Seed Data)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await dbContext.Database.MigrateAsync();
        await DbInitializer.SeedAsync(dbContext);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Помилка під час міграції або ініціалізації початкових даних.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
