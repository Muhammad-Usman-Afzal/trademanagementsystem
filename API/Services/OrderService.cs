namespace API.Services;

public class OrderService : BaseService<Order>, IOrderRepo
{
    private readonly IBaseRepo<Order> _baseRepo;

    public OrderService(TMSContext context, IBaseRepo<Order> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}