using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class Party : BaseEntity
{
    [Column(TypeName = "varchar(100)")]
    public string CompanyName { get; set; }

    [Column(TypeName = "nvarchar(13)")]
    public string CompanyContact { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string CompanyEmail { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string CompanyAddress { get; set; }
    public bool IsRegisterd { get; set; } = false;

    [Column(TypeName = "nvarchar(9)")]
    public string NTN { get; set; }

    [Column(TypeName = "nvarchar(17)")]
    public string STRNo { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string FocalPersonName { get; set; }

    [Column(TypeName = "nvarchar(13)")]
    public string FocalPersonContact { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string FocalPersonEmail { get; set; }
    
    [Column(TypeName = "nvarchar(12)")]
    public string PartyType { get; set; }
}
