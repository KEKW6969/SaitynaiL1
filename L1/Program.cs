using L1.Auth.Model;
using L1.Auth;
using L1.Data;
using L1.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.ResourceOwner, policy => policy.Requirements.Add(new ResourceOwnerRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("https://stunning-moonbeam-d478d5.netlify.app", "http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("corspolicy");

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

var dbSeeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthDbSeeder>();
await dbSeeder.SeedAsync();

app.Run();
