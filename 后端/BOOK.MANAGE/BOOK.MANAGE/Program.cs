using BOOK.DB;
using BOOK.MANAGE;
using BOOK.Repository;
using BOOK.SERVERS;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ��� Consul ע��
builder.Services.RegisterConsul(builder.Configuration);

// ��ȡ�����ļ��е����ݿ������ַ���
string connectionString = builder.Configuration.GetConnectionString("BooksDatabase");

// ע��BooksContext
builder.Services.AddDbContext<BooksContext>(options =>
    options.UseSqlServer(connectionString));

// �����������
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddScoped<BOOK.SERVERS.IBookService, BOOK.SERVERS.BookSerivceImp>();
builder.Services.AddScoped<BOOK.SERVERS.IBorrowedService,BOOK.SERVERS.BorrowedServiceImp>();
builder.Services.AddScoped<DB_Borrwoed>();
builder.Services.AddScoped<Redis_Borrowed>();
builder.Services.AddScoped<DB_Book>();
builder.Services.AddScoped<Redis_Book>();

// ��ȡappsettings.json�ļ���������֤����Կ��Secret�������ڣ�Aud����Ϣ
var audienceConfig = builder.Configuration.GetSection("Audience");
// ��ȡ��ȫ��Կ
var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
// tokenҪ��֤�Ĳ�������
var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
{
    ValidateIssuerSigningKey = true, // ������֤��ȫ��Կ
    IssuerSigningKey = signingKey, // ��ֵ��ȫ��Կ
    ValidateIssuer = true, // ������֤ǩ����
    ValidIssuer = audienceConfig["Iss"], // ��ֵǩ����
    ValidateAudience = true, // ������֤����
    ValidAudience = audienceConfig["Aud"], // ��ֵ����
    ValidateLifetime = true, // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
    ClockSkew = TimeSpan.Zero, // ����ķ�����ʱ��ƫ����
    RequireExpirationTime = true // �Ƿ�Ҫ��Token��Claims�б������Expires
};
// ��ӷ�����֤������ΪTestKey
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "TestKey";
})
.AddJwtBearer("TestKey", x =>
{
    x.RequireHttpsMetadata = false;
    // ��JwtBearerOptions�����У�IssuerSigningKey(ǩ����Կ)��ValidIssuer(Token�䷢����)��ValidAudience(�䷢��˭)���������Ǳ���ġ�
    x.TokenValidationParameters = tokenValidationParameters;
});



var app = builder.Build();
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ��֤
app.UseAuthorization(); // ��Ȩ

app.MapControllers();

app.Run();
