using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Book.AuthServer.DataAccessor;
using Book.AuthServer.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;


namespace Book.AuthServer.Server
{
    public class LoginServiceImp : ILoginService
    {
        private IOptions<Audience> m_Settings;
        private static AuthServer.DataAccessor.AuthContext m_DbContext;
        public LoginServiceImp(AuthServer.DataAccessor.AuthContext dbContext, IOptions<Audience> settings)
        {
            m_DbContext = dbContext;
            this.m_Settings = settings;
        }

        #region 通过jwt获取用户信息
        public object GetInfoByName(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                #region 无需验证的方法 这里是因为的我验证报错始终找不到错误信息
                // 从 JWT 中获取 Payload（负载）部分
                string authInfo = Jose.JWT.Payload(token.Replace("Bearer ", ""));
                // 将 JSON 数据转换为 JwtData 对象
                JwtData jwtData = JsonConvert.DeserializeObject<JwtData>(authInfo);
                // 存入一个变量中
                var subVariable = jwtData.sub;
                #endregion

                var usernameClaim = subVariable;

                if (usernameClaim != null)
                {
                    var info = m_DbContext.Users
                        .Where(c => c.LoginName == usernameClaim)
                        .Select(c => new
                        {
                            roles = "admin",
                            introduction = "I am a super administrator",
                            avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                            name = c.LoginName,
                            email = "ausnatki@qq.com",
                            phone = "18581348911",
                            uid = c.Id,
                            img = "dsadasdasda"
                        }).FirstOrDefault();
                    return info;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region 获取jwt
        public object GetJwt(string username, string password)
        {
            //string username = userInfo.username;
            //string password = userInfo.password;
            //验证登录用户名和密码
            var user = m_DbContext.Users.Where(c => c.LoginName == username && c.Password == password).FirstOrDefault();
            if (user != null)
            {
                var now = DateTime.UtcNow; //添加用户的信息，转成一组声明，还可以写入更多用户信息声明
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),//声明主题
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//JWT ID 唯一标识符
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)//发布时间戳 issued timestamp
                };


                //下面使用 Microsoft.IdentityModel.Tokens帮助库下的类来创建JwtToken
                //创建安全秘钥
                var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(m_Settings.Value.Secret));

                //声明jwt验证参数
                var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//必须验证安全秘钥
                    IssuerSigningKey = signingKey,//赋值安全秘钥
                    ValidateIssuer = true,//必须验证签发人
                    ValidIssuer = m_Settings.Value.Iss,//赋值签发人
                    ValidateAudience = true,//必须验证受众
                    ValidAudience = m_Settings.Value.Aud,//赋值受众
                    ValidateLifetime = true,//是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ClockSkew = TimeSpan.Zero,//允许的服务器时间偏移量
                    RequireExpirationTime = true,//是否要求Token的Claims中必须包含Expires
                };
                var jwt = new JwtSecurityToken(
                issuer: m_Settings.Value.Iss,//jwt签发人
                    audience: m_Settings.Value.Aud,//jwt受众
                    claims: claims,//jwt一组声明
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),//jwt令牌过期时间
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)//签名凭证: 安全密钥、签名算法
                );


                //生成jwt令牌(json web token)
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
                };
                var token = "Bearer" + " " + encodedJwt;
                return new { code = 20000, data = new { token } };
            }
            else
            {
                return new { code = 400 };
            }
        }
        #endregion

    }
}
