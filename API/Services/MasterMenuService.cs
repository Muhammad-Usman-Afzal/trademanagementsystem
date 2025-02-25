namespace API.Services;

public class MasterMenuService : BaseService<MasterMenu>, IMasterMenuRepo
{
    private readonly IBaseRepo<MasterMenu> _baseRepo;

    public MasterMenuService(TMSContext context, IBaseRepo<MasterMenu> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}