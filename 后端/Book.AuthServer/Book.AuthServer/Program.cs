using Book.AuthServer;
using Book.AuthServer.DataAccessor;
using Book.AuthServer.Models;
using Book.AuthServer.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services to the container.
// 读取配置文件中的数据库连接字符串
string connectionString = builder.Configuration.GetConnectionString("BooksDatabase");

// 注册BooksContext
builder.Services.AddDbContext<Book.AuthServer.DataAccessor.AuthContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.AddDbContext<AuthContext>();
builder.Services.Configure<Audience>(builder.Configuration.GetSection("Audience"));
builder.Services.AddScoped<Book.AuthServer.Server.ILoginService, Book.AuthServer.Server.LoginServiceImp>();
// consul服务引入
builder.Services.RegisterConsul(builder.Configuration);

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
