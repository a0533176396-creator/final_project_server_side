using Microsoft.OpenApi.Models;
using DTO.Mapper;
using BLL.Functions;
using Microsoft.AspNetCore;
using DTO.Mapper;
using BLL.Functions;

var builder = WebApplication.CreateBuilder(args);

//using Microsoft.OpenApi.Models;
//using DTO.Mapper;
//using BLL.Functions;
//using Microsoft.AspNetCore;
//using DTO.Mapper;
//using BLL.Functions;

//var builder = WebApplicationBuilder.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ✅ Add AutoMapper - Register the AutoMap profile
//builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMap>());

// ✅ Register BLL services
//builder.Services.AddScoped<UsersBLL>();

// ✅ Add CORS Policy - לפני builder.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:5173") // הפורט של ה-React
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// ✅ Swagger Configuration (מפורט)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tasks Project API",
        Description = "API for managing tasks, categories, and chat sessions with PostgreSQL",
        Contact = new OpenApiContact
        {
            Name = "Development Team"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        Array.Empty<string>()
    //    }
    //});
});

var app = builder.Build();

// ✅ Use CORS Middleware - אחרי builder.Build() ולפני app.MapControllers()
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // ✅ Swagger UI (מפורט)
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasks API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
