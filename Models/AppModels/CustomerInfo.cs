using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class CustomerInfo : BaseEntity
{
    [Column(TypeName = "varchar(30)")]
    public string NameOfCustomer { get; set; }
    [Column(TypeName = "nvarchar(15)")]
    public string Contact { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public string Email { get; set; }
    [Column(TypeName = "nvarchar(125)")]
    public string Address { get; set; }
}
