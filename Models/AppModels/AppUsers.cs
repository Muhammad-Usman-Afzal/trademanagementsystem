using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class AppUsers : BaseEntity
{
    [Column(TypeName = "varchar(30)")]
    public string UserName { get; set; }
    [Column(TypeName = "nvarchar(30)")]
    public string Password { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string FullName { get; set; }
    [NotMapped]
    public string BearerToken { get; set; }
    public bool? IsSuperUser { get; set; } = false;
    public List<UserAuthorizedScreens> AuthorizedScreens { get; set; }=new List<UserAuthorizedScreens>();
}