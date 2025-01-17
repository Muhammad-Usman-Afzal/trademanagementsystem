namespace Models.AppModels;

public class BaseEntity
{
    public int Id { get; set; }
    public bool? IsActive { get; set; } = true;
    public bool? IsDeleted { get; set; } = false;
    public int? CreateBy { get; set; }
    public DateTime? CreateOn { get; set; }
    public int? UpdateBy { get; set; }
    public DateTime? UpdateOn { get; set; }
}
