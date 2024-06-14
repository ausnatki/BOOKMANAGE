using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("OcelotConfig.json", optional: false, reloadOnChange: true); // 确认文件名一致
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 获取 appsettings.json 文件中配置认证中密钥（Secret）跟受众（Aud）信息
var audienceConfig = builder.Configuration.GetSection("Audience");
// 获取安全秘钥
var signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
// token 要验证的参数集合
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true, // 必须验证安全秘钥
    IssuerSigningKey = signingKey, // 赋值安全秘钥
    ValidateIssuer = true, // 必须验证签发人
    ValidIssuer = audienceConfig["Iss"], // 赋值签发人
    ValidateAudience = true, // 必须验证受众
    ValidAudience = audienceConfig["Aud"], // 赋值受众
    ValidateLifetime = true, // 是否验证 Token 有效期
    ClockSkew = TimeSpan.Zero, // 允许的服务器时间偏移量
    RequireExpirationTime = true, // 是否要求 Token 的 Claims 中必须包含 Expires
};

// 添加服务验证，方案为 TestKey
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = "TestKey";
})
.AddJwtBearer("TestKey", x =>
{
    x.RequireHttpsMetadata = false;
    // 在 JwtBearerOptions 配置中，IssuerSigningKey（签名秘钥）、ValidIssuer（Token 颁发机构）、ValidAudience（颁发给谁）三个参数是必须的。
    x.TokenValidationParameters = tokenValidationParameters;
});

// 添加 Ocelot 网关服务时，包括 Secret 秘钥、Iss 签发人、Aud 受众
builder.Services.AddOcelot(builder.Configuration).AddConsul().AddPolly();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAnyPolicy");

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.MapControllers();

app.Run();
