namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerInfoController : ControllerBase
{
    private readonly ICustomerInfoRepo _CustomerInfoRepo;

    public CustomerInfoController(ICustomerInfoRepo CustomerInfoRepo)
    {
        _CustomerInfoRepo = CustomerInfoRepo;
    }

    [Route("GetCustomerInfos")]
    [HttpGet]
    public async Task<ActionResult<List<CustomerInfo>>> GetCustomerInfos()
    {
        try
        {
            return await _CustomerInfoRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<CustomerInfo>();
        }
    }

    [HttpGet("GetCustomerInfoById")]
    public async Task<ActionResult<CustomerInfo>> GetCustomerInfoById(int id)
    {
        try
        {
            return await _CustomerInfoRepo.GetById(id) ?? new CustomerInfo();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new CustomerInfo();
        }
    }

    // PUT: api/CustomerInfo/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutCustomerInfo(int id, CustomerInfo CustomerInfo)
    //{
    //    if (id != CustomerInfo.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(CustomerInfo).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!CustomerInfoExists(id))
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            throw;
    //        }
    //    }

    //    return NoContent();
    //}

    [HttpPost("Create")]
    public async Task<ActionResult<CustomerInfo>> Create(CustomerInfo CustomerInfo)
    {
        try
        {
            if (CustomerInfo == null)
            {
                return Problem("Entity set 'TMSContext.CustomerInfo'  is null.");
            }

            await _CustomerInfoRepo.Create(CustomerInfo);

            return CreatedAtAction("GetCustomerInfoById", new { id = CustomerInfo.Id }, CustomerInfo);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new CustomerInfo();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<CustomerInfo>> Update(CustomerInfo CustomerInfo)
    {
        try
        {
            if (CustomerInfo == null)
            {
                return Problem("Entity set 'TMSContext.CustomerInfo'  is null.");
            }

            await _CustomerInfoRepo.Update(CustomerInfo);

            return CreatedAtAction("GetCustomerInfoById", new { id = CustomerInfo.Id }, CustomerInfo);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new CustomerInfo(); }
    }

    // DELETE: api/CustomerInfo/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteCustomerInfo(int id)
    //{
    //    if (_context.CustomerInfo == null)
    //    {
    //        return NotFound();
    //    }
    //    var CustomerInfo = await _context.CustomerInfo.FindAsync(id);
    //    if (CustomerInfo == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.CustomerInfo.Remove(CustomerInfo);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool CustomerInfoExists(int id)
    //{
    //    return (_context.CustomerInfo?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
