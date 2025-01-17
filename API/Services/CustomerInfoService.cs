namespace API.Services;

public class CustomerInfoService : BaseService<CustomerInfo>, ICustomerInfoRepo
{
    private readonly IBaseRepo<CustomerInfo> _baseRepo;

    public CustomerInfoService(TMSContext context, IBaseRepo<CustomerInfo> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}