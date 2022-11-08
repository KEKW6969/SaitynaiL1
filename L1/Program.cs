using L1.Auth.Model;
using L1.Auth;
using L1.Data;
using L1.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<HotelsDbContext>();

builder.Services.AddIdentity<HotelRestUser, IdentityRole>().AddEntityFrameworkStores<HotelsDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
   {
       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
   }).AddJwtBearer(options =>
   {
       options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
       options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
       options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
   });


builder.Services.AddTransient<IHotelsRepository, HotelsRepository>();
builder.Services.AddTransient<IFloorsRepository, FloorsRepository>();
builder.Services.AddTransient<IRoomsRepository, RoomsRepository>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<AuthDbSeeder>();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

var dbSeeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthDbSeeder>();
await dbSeeder.SeedAsync();

app.Run();
