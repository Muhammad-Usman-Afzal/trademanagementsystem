using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Composition;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services;

public class AppUsersService : BaseService<AppUsers>, IAppUsersRepo
{
    private readonly IBaseRepo<AppUsers> _baseRepo;
    private IConfiguration _configuration;

    public AppUsersService(TMSContext context, IBaseRepo<AppUsers> baseRepo, IConfiguration configuration)
        : base(context)
    {
        _baseRepo = baseRepo;
        _configuration = configuration;
    }

    public string exGenerateToken(AppUsers oUser)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, oUser.UserName??""),
                new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()),
                new Claim(ClaimTypes.Surname, oUser.FullName??""),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(1)).ToUnixTimeSeconds().ToString())
            };

            ////Role Admin, User
            //if (user.flgSuper)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            //}
            //else
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, "User"));
            //}

            var Token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TechnologyiansSolutionsKiSecretKeyBuhatHiSecret")), SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims)
                );

            string TokenString = "";
            TokenString = new JwtSecurityTokenHandler().WriteToken(Token);
            return TokenString;
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return "";
        }
    }
    
    public string xGenerateToken(AppUsers oUser)
    {
        try
        {
            string role = "Guest";
            var claims = new List<Claim>
            {
                                        new Claim(JwtRegisteredClaimNames.Sub,oUser.Id.ToString()),
                                        new Claim(JwtRegisteredClaimNames.Name,oUser.UserName),
                                        new Claim("Role",role),
                                        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TechnologyiansSolutionsKiSecretKeyBuhatHiSecret"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Sectoken = new JwtSecurityToken(
                null,
                null,
            claims,
            expires: DateTime.Now.AddMinutes(double.Parse("60")), // will be valid one hour
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return token;
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return "";
        }
    }

    private string GenerateToken(AppUsers oUser)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, oUser.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return "";
        }
    }

    public async Task<AppUsers> ValidateLogin(AppUsers AppUsers)
    {
        try
        {
            //SingleSignOn(AppUsers.UserCode, AppUsers.UserCode, AppUsers.Password);

            AppUsers User = new AppUsers();

            User = await _baseRepo.GetSingleByCondition(o => o.UserName == AppUsers.UserName && EF.Functions.Collate(o.Password, "SQL_Latin1_General_CP1_CS_AS") == AppUsers.Password) ?? new AppUsers();

            if (User != null && User.Id > 0)
            {
                User.BearerToken = GenerateToken(User);
                return User;
            }
            else
            {
                return new AppUsers();
            }
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new AppUsers();
        }
    }
}