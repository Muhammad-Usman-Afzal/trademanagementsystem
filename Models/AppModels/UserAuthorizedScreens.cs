using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class UserAuthorizedScreens : BaseEntity
{
    public int MasterMenuId { get; set; }
    public MasterMenu MasterMenu { get; set; }
    public UserPermission AuthorizedPermission { get; set; } = UserPermission.NoAccess;
}

public enum UserPermission
{
    FullAccess=1,
    NoAccess,
    ViewOnly
}