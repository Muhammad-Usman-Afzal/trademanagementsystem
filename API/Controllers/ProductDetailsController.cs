namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductDetailsController : ControllerBase
{
    private readonly IProductDetailsRepo _ProductDetailsRepo;

    public ProductDetailsController(IProductDetailsRepo ProductDetailsRepo)
    {
        _ProductDetailsRepo = ProductDetailsRepo;
    }

    [Route("GetProductDetails")]
    [HttpGet]
    public async Task<ActionResult<List<ProductDetails>>> GetParties()
    {
        try
        {
            return await _ProductDetailsRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<ProductDetails>();
        }
    }

    [HttpGet("GetProductDetailsById")]
    public async Task<ActionResult<ProductDetails>> GetProductDetailsById(int id)
    {
        try
        {
            return await _ProductDetailsRepo.GetById(id) ?? new ProductDetails();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new ProductDetails();
        }
    }

    [HttpPost("Create")]
    public async Task<ActionResult<ProductDetails>> Create(ProductDetails ProductDetails)
    {
        try
        {
            if (ProductDetails == null)
            {
                return Problem("Entity set 'TMSContext.ProductDetails'  is null.");
            }

            await _ProductDetailsRepo.Create(ProductDetails);

            return CreatedAtAction("GetProductDetailsById", new { id = ProductDetails.Id }, ProductDetails);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new ProductDetails();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<ProductDetails>> Update(ProductDetails ProductDetails)
    {
        try
        {
            if (ProductDetails == null)
            {
                return Problem("Entity set 'TMSContext.ProductDetails'  is null.");
            }

            await _ProductDetailsRepo.Update(ProductDetails);

            return CreatedAtAction("GetProductDetailsById", new { id = ProductDetails.Id }, ProductDetails);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new ProductDetails(); }
    }

}
