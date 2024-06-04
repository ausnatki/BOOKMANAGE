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

//��ȡappsettings.json�ļ���������֤����Կ��Secret�������ڣ�Aud����Ϣ
var audienceConfig = builder.Configuration.GetSection("Audience");
//��ȡ��ȫ��Կ
var signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
//tokenҪ��֤�Ĳ�������
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true, //������֤��ȫ��Կ
    IssuerSigningKey = signingKey, //��ֵ��ȫ��Կ
    ValidateIssuer = true, //������֤ǩ����
    ValidIssuer = audienceConfig["Iss"], //��ֵǩ����
    ValidateAudience = true,//������֤����
    ValidAudience = audienceConfig["Aud"],//��ֵ����
    ValidateLifetime = true,//�Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
    ClockSkew = TimeSpan.Zero,//����ķ�����ʱ��ƫ����
    RequireExpirationTime = true,//�Ƿ�Ҫ��Token��Claims�б������Expires
};

//��ӷ�����֤������ΪTestKey
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = "TestKey";
})
.AddJwtBearer("TestKey", x =>
{
    x.RequireHttpsMetadata = false;
    //��JwtBearerOptions�����У�IssuerSigningKey(ǩ����Կ)��ValidIssuer(Token�䷢����)��ValidAudience(�䷢��˭)���������Ǳ���ġ�
    x.TokenValidationParameters = tokenValidationParameters;
});

//���Ocelot���ط���ʱ,����Secret��Կ��Issǩ���ˡ�Aud����
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
