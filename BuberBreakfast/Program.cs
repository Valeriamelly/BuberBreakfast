using BuberBreakfast.Services.Breakfasts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
}

var app = builder.Build();

{
    app.UseExceptionHandler("/error"); //permite manejar los errores internos del servidor y no mostrar información confidencial
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}