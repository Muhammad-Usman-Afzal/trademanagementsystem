namespace UI.Repositories;

public interface IAppUsersRepoUI : IBaseRepoUI<AppUsers>
{
    Task<AppUsers> ValidateLogin(AppUsers oModel);
}