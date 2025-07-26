namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvoiceDetailsController : ControllerBase
{
    private readonly IInvoiceDetailsRepo _InvoiceDetailsRepo;

    public InvoiceDetailsController(IInvoiceDetailsRepo InvoiceDetailsRepo)
    {
        _InvoiceDetailsRepo = InvoiceDetailsRepo;
    }

    [Route("GetInvoiceDetailss")]
    [HttpGet]
    public async Task<ActionResult<List<InvoiceDetails>>> GetInvoiceDetailss()
    {
        try
        {
            return await _InvoiceDetailsRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<InvoiceDetails>();
        }
    }

    [HttpGet("GetInvoiceDetailsById")]
    public async Task<ActionResult<InvoiceDetails>> GetInvoiceDetailsById(int id)
    {
        try
        {
            return await _InvoiceDetailsRepo.GetById(id) ?? new InvoiceDetails();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new InvoiceDetails();
        }
    }

  
    // PUT: api/InvoiceDetails/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutInvoiceDetails(int id, InvoiceDetails InvoiceDetails)
    //{
    //    if (id != InvoiceDetails.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(InvoiceDetails).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!InvoiceDetailsExists(id))
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
    public async Task<ActionResult<InvoiceDetails>> Create(InvoiceDetails InvoiceDetails)
    {
        try
        {
            if (InvoiceDetails == null)
            {
                return Problem("Entity set 'TMSContext.InvoiceDetails'  is null.");
            }

            await _InvoiceDetailsRepo.Create(InvoiceDetails);

            return CreatedAtAction("GetInvoiceDetailsById", new { id = InvoiceDetails.Id }, InvoiceDetails);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new InvoiceDetails();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<InvoiceDetails>> Update(InvoiceDetails InvoiceDetails)
    {
        try
        {
            if (InvoiceDetails == null)
            {
                return Problem("Entity set 'TMSContext.InvoiceDetails'  is null.");
            }

            await _InvoiceDetailsRepo.Update(InvoiceDetails);

            return CreatedAtAction("GetInvoiceDetailsById", new { id = InvoiceDetails.Id }, InvoiceDetails);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new InvoiceDetails(); }
    }

    [HttpDelete("Delete")]
    public async Task<bool> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return false;
            }

            var InvoiceDetails = await _InvoiceDetailsRepo.GetById(id);
            if (InvoiceDetails == null)
            {
                return false;
            }

            return await _InvoiceDetailsRepo.DeleteById(id);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return false;
        }
    }

    //private bool InvoiceDetailsExists(int id)
    //{
    //    return (_context.InvoiceDetails?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
