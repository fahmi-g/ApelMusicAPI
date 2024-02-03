using ApelMusicAPI.Data;
using ApelMusicAPI.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
         {
               new OpenApiSecurityScheme
               {
                     Reference = new OpenApiReference
                     {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                     }
               },
               new string[] {}
         }
    });
});


builder.Services.AddScoped<ClassData>();
builder.Services.AddScoped<ClassCategoryData>();
builder.Services.AddScoped<UserRolesData>();
builder.Services.AddScoped<UserData>();
builder.Services.AddScoped<CheckoutStateData>();
builder.Services.AddScoped<OrderData>();
builder.Services.AddScoped<OrderDetailData>();
builder.Services.AddTransient<EmailService>();

builder.Services.AddCors();

var key = builder.Configuration.GetSection("JwtConfig:Key").Value;
var JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
        (options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = JwtKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
        });

//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//
app.UseCors(
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );

app.UseAuthentication();
//

app.UseAuthorization();

app.MapControllers();

app.Run();
