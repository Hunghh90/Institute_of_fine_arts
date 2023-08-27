using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
// add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            //policy.WithOrigins("https://24h.com.vn");
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        }
        );
}
);
// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options
    => options.SerializerSettings.ReferenceLoopHandling
    = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("institute_of_fine_arts");
Institute_of_fine_arts.Entities.InstituteOfFineArtsContext.connectionString = connectionString;
builder.Services.AddDbContext<Institute_of_fine_arts.Entities.InstituteOfFineArtsContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString))
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// add Handlers

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("5"));
    options.AddPolicy("AdminAndManager", policy => policy.RequireRole("5", "1"));
    options.AddPolicy("AllManager", policy => policy.RequireRole("5", "1", "6"));
    options.AddPolicy("Director", policy => policy.RequireRole("6"));
    options.AddPolicy("Manager", policy => policy.RequireRole("1"));
    options.AddPolicy("Teacher", policy => policy.RequireRole("2"));
    options.AddPolicy("Student", policy => policy.RequireRole("3"));
    options.AddPolicy("Auth", policy => policy.RequireAuthenticatedUser());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

