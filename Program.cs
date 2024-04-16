using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WikYModels.DbContexts;
using WikYModels.Models;
using WikYRepositories.IRepositories;
using WikYRepositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>(c =>
{
    
    //c.Password
}) // config mdp ...
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WikYDbContext>();

builder.Services.AddDbContext<WikYDbContext>(o =>
{
    //o.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Demo1004;Trusted_Connection=True;");
    o.UseSqlServer(builder.Configuration.GetConnectionString("WikY"));
});

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
