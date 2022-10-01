using L1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<HotelsDbContext>();


var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
