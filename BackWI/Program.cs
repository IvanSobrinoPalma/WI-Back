using BackWI.Data;
using BackWI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Claims based Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("owner", policy =>
    policy.RequireClaim("roleId", builder.Configuration.GetSection("AppSettings:RoleId").Value!));
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:Token").Value!))
    };
});
builder.Services.AddDbContext<WildInfoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("conexionSQL")));
builder.Services.AddControllers().AddJsonOptions(opt => { opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;});

// Mapping of services
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();

var reglas = "reglasPD";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: reglas, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(reglas);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
