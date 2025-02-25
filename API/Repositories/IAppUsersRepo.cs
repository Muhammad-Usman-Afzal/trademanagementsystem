namespace API.Repositories;

public interface IAppUsersRepo : IBaseRepo<AppUsers>
{
    Task<AppUsers> ValidateLogin(AppUsers AppUsers);
}