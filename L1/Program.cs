using L1.Data;
using L1.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<HotelsDbContext>();
builder.Services.AddTransient<IHotelsRepository, HotelsRepository>();
builder.Services.AddTransient<IFloorsRepository, FloorsRepository>();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
