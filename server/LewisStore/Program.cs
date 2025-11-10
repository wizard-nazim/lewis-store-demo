using LewisStore.Data;
using LewisStore.Middleware;
using LewisStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
//  PostgreSQL Database
// -----------------------------------------------------
builder.Services.AddDbContext<LewisDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------------------------------------
//  Dependency Injection for Services
// -----------------------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICreditService, CreditService>();
builder.Services.AddScoped<IPaymentService, MockPaymentService>(); // this is your existing mock implementation

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// -----------------------------------------------------
// Swagger Config (JWT auth enabled)
// -----------------------------------------------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LewisStore API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// -----------------------------------------------------
// JWT Authentication Setup
// -----------------------------------------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            )
        };
    });

// -----------------------------------------------------
// CORS (Frontend at port 5173)
// -----------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

// -----------------------------------------------------
// Apply migrations & seed initial data
// -----------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LewisDbContext>();
    context.Database.Migrate();
    DataSeeder.Seed(context, services);
}

// -----------------------------------------------------
// Middleware & Routing
// -----------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("\n");
Console.ForegroundColor = ConsoleColor.Green;
CenterText("╔════════════════════════════════════════╗");
CenterText("║       Lewis Store API is Live!         ║");
CenterText("╚════════════════════════════════════════╝");
Console.WriteLine("\n");
Console.ResetColor();

app.Run();

// Methods down here! 
static void CenterText(string text)
{
    int windowWidth = Console.WindowWidth;
    int padding = Math.Max(0, (windowWidth - text.Length) / 2);
    Console.WriteLine(new string(' ', padding) + text);

}