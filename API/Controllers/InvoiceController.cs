using API.Repositories;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRepo _InvoiceRepo;

    public InvoiceController(IInvoiceRepo InvoiceRepo)
    {
        _InvoiceRepo = InvoiceRepo;
    }

    [Route("GetInvoices")]
    [HttpGet]
    public async Task<ActionResult<List<Invoice>>> GetInvoices()
    {
        try
        {
            return await _InvoiceRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<Invoice>();
        }
    }

    [HttpGet("GetInvoiceById")]
    public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
    {
        try
        {
            return await _InvoiceRepo.GetById(id) ?? new Invoice();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new Invoice();
        }
    }

    //[HttpGet("GetInvoicesByType")]
    //public async Task<ActionResult<List<Invoice>>> GetInvoicesByType(InvoiceType InvoiceType)
    //{
    //    try
    //    {
    //        return await _InvoiceRepo.GetByCondition(x => x.InvoiceType == InvoiceType).ToListAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        APILogger.WriteLog(ex);
    //        return new List<Invoice>();
    //    }
    //}

	[HttpGet("GetInvoicesByType")]
	public async Task<ActionResult<string>> GetInvoicesByType(InvoiceType InvoiceType)
	{
		try
		{
			var invNo = await _InvoiceRepo.GetByCondition(x => x.InvoiceType == InvoiceType)
	 .OrderByDescending(x => x.Id)
	 .FirstOrDefaultAsync();

			return invNo?.InvoiceNumber ?? string.Empty;
		}
		catch (Exception ex)
		{
			APILogger.WriteLog(ex);
			return string.Empty;
		}
	}

	// PUT: api/Invoice/5
	//[HttpPut("{id}")]
	//public async Task<IActionResult> PutInvoice(int id, Invoice Invoice)
	//{
	//    if (id != Invoice.Id)
	//    {
	//        return BadRequest();
	//    }

	//    _context.Entry(Invoice).State = EntityState.Modified;

	//    try
	//    {
	//        await _context.SaveChangesAsync();
	//    }
	//    catch (DbUpdateConcurrencyException)
	//    {
	//        if (!InvoiceExists(id))
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
    public async Task<ActionResult<Invoice>> Create(Invoice Invoice)
    {
        try
        {
            if (Invoice == null)
            {
                return Problem("Entity set 'TMSContext.Invoice'  is null.");
            }

            Invoice.Customer = null;
            foreach(var a in Invoice.InvoiceDetails)
            {
                a.Product = null;
            }

            await _InvoiceRepo.Create(Invoice);

            return CreatedAtAction("GetInvoiceById", new { id = Invoice.Id }, Invoice);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new Invoice();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<Invoice>> Update(Invoice Invoice)
    {
        try
        {
            if (Invoice == null)
            {
                return Problem("Entity set 'TMSContext.Invoice'  is null.");
            }

            await _InvoiceRepo.Update(Invoice);

            return CreatedAtAction("GetInvoiceById", new { id = Invoice.Id }, Invoice);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new Invoice(); }
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

            var Invoice = await _InvoiceRepo.GetById(id);
            if (Invoice == null)
            {
                return false;
            }

            return await _InvoiceRepo.DeleteById(id);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return false;
        }
    }

    //private bool InvoiceExists(int id)
    //{
    //    return (_context.Invoice?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
