using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class MasterMenu : BaseEntity
{
    [Column(TypeName = "varchar(50)")]
    public string MenuName { get; set; }
    public string URL { get; set; }
    public string IconURL { get; set; }
    [NotMapped]
    public UserPermission AuthorizedPermission { get; set; } = UserPermission.NoAccess;
}