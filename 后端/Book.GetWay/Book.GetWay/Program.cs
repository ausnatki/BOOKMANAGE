using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("OcelotConfig.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddOcelot();

//获取appsettings.json文件中配置认证中密钥（Secret）跟受众（Aud）信息
var audienceConfig = builder.Configuration.GetSection("Audience");
//获取安全秘钥
var signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
//token要验证的参数集合
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true, //必须验证安全秘钥
    IssuerSigningKey = signingKey, //赋值安全秘钥
    ValidateIssuer = true, //必须验证签发人
    ValidIssuer = audienceConfig["Iss"], //赋值签发人
    ValidateAudience = true,//必须验证受众
    ValidAudience = audienceConfig["Aud"],//赋值受众
    ValidateLifetime = true,//是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
    ClockSkew = TimeSpan.Zero,//允许的服务器时间偏移量
    RequireExpirationTime = true,//是否要求Token的Claims中必须包含Expires
};

//添加服务验证，方案为TestKey
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = "TestKey";
})
.AddJwtBearer("TestKey", x =>
{
    x.RequireHttpsMetadata = false;
    //在JwtBearerOptions配置中，IssuerSigningKey(签名秘钥)、ValidIssuer(Token颁发机构)、ValidAudience(颁发给谁)三个参数是必须的。
    x.TokenValidationParameters = tokenValidationParameters;
});

//添加Ocelot网关服务时,包括Secret秘钥、Iss签发人、Aud受众
builder.Services.AddOcelot(builder.Configuration);

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

await app.UseOcelot();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
