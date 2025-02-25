namespace API.Services;

public class OrderDetailService : BaseService<OrderDetail>, IOrderDetailRepo
{
    private readonly IBaseRepo<OrderDetail> _baseRepo;

    public OrderDetailService(TMSContext context, IBaseRepo<OrderDetail> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}