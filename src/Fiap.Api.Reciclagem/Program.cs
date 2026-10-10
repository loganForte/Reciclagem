using System.Text;
using Fiap.Api.Reciclagem.Data;
using Fiap.Api.Reciclagem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Services
builder.Services.AddScoped<IPontoColetaService, PontoColetaService>();
builder.Services.AddScoped<IDescarteService, DescarteService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
#endregion

#region Autenticacao
var chaveJwt = builder.Configuration["Jwt:Key"]!;

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
#endregion

builder.Services.AddHealthChecks();   // endpoint /health usado por Docker e pelo pipeline
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplica as migrations ao iniciar. Se o projeto nao tiver a pasta Migrations,
// cria o schema (e o seed do HasData) direto a partir do modelo.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.GetMigrations().Any())
        db.Database.Migrate();
    else
        db.Database.EnsureCreated();
}

// Swagger em Development ou quando habilitado por configuracao (Swagger__Enabled=true em staging)
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Em container o TLS costuma ser terminado fora da aplicacao
if (!app.Configuration.GetValue<bool>("DisableHttpsRedirection"))
    app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();

// Necessario para o WebApplicationFactory<Program> usado nos testes xUnit
public partial class Program { }
