namespace API.Services;

public class PartyService : BaseService<Party>, IPartyRepo
{
    private readonly IBaseRepo<Party> _baseRepo;

    public PartyService(TMSContext context, IBaseRepo<Party> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}