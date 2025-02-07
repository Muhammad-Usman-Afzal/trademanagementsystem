namespace API.Services;

public class ProductDetailsService : BaseService<ProductDetails>, IProductDetailsRepo
{
    private readonly IBaseRepo<ProductDetails> _baseRepo;

    public ProductDetailsService(TMSContext context, IBaseRepo<ProductDetails> baseRepo)
        : base(context)
    {
        _baseRepo = baseRepo;
    }
}