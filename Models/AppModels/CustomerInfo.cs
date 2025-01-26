using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class CustomerInfo : BaseEntity
{
    [Column(TypeName = "varchar(30)")]
    public string CompanyName { get; set; }

    [Column(TypeName = "nvarchar(15)")]
    public string CompanyContact { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string CompanyEmail { get; set; }

    [Column(TypeName = "nvarchar(125)")]
    public string CompanyAddress { get; set; }
    public bool IsRegisterd { get; set; } = false;

    [Column(TypeName = "nvarchar(9)")]
    public string NTN { get; set; }

    [Column(TypeName = "nvarchar(15)")]
    public string STRNo { get; set; }

    [Column(TypeName = "varchar(30)")]
    public string FocalPersonName { get; set; }

    [Column(TypeName = "nvarchar(15)")]
    public string FocalPersonContact { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string FocalPersonEmail { get; set; }
}
