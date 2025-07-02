using System.ComponentModel.DataAnnotations.Schema;

namespace Models.AppModels;

public class ProductDetails : BaseEntity
{
    public int PartyId { get; set; }
    public Party Party { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public string ItemName { get; set; }
    public bool Isprocessed { get; set; }
    public ItemType ItemType { get; set; }
    public decimal? StandardWeight { get; set; }
}

public enum ItemType
{
    Cone=1,
    Hanks=2
}