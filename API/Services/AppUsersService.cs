using Microsoft.IdentityModel.Tokens;
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

    public string GenerateToken(AppUsers oUser)
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