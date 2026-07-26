using Microsoft.OpenApi.Models;
using DTO.Mapper;
using BLL.Functions;
using Microsoft.AspNetCore;
using DTO.Mapper;
using BLL.Functions;

var builder = WebApplication.CreateBuilder(args);

 //Add services to the container.

builder.Services.AddControllers();

// ✅ Add AutoMapper - Register the AutoMap profile
//builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMap>());

// ✅ Register BLL services
//builder.Services.AddScoped<UsersBLL>();

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

builder.Services.AddCors();

var app = builder.Build();

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

    // הגדרת CORS
    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "http://localhost:3001"));
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();






























//using DAL.Data;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//// Register DbContext with PostgreSQL
////var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
////    ?? "Host=localhost;Port=5432;Database=tasks_project_db;Username=postgres;Password=postgres;";

////builder.Services.AddDbContext<AppDbContext>(options =>
////    options.UseNpgsql(connectionString));

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
