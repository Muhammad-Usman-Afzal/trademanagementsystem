namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderRepo _PurchaseOrderRepo;

    public PurchaseOrderController(IPurchaseOrderRepo PurchaseOrderRepo)
    {
        _PurchaseOrderRepo = PurchaseOrderRepo;
    }

    [Route("GetPurchaseOrders")]
    [HttpGet]
    public async Task<ActionResult<List<PurchaseOrder>>> GetPurchaseOrders()
    {
        try
        {
            return await _PurchaseOrderRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<PurchaseOrder>();
        }
    }

    [HttpGet("GetPurchaseOrderById")]
    public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderById(int id)
    {
        try
        {
            return await _PurchaseOrderRepo.GetById(id) ?? new PurchaseOrder();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new PurchaseOrder();
        }
    }

    // PUT: api/PurchaseOrder/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrder PurchaseOrder)
    //{
    //    if (id != PurchaseOrder.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(PurchaseOrder).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!PurchaseOrderExists(id))
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
    public async Task<ActionResult<PurchaseOrder>> Create(PurchaseOrder PurchaseOrder)
    {
        try
        {
            if (PurchaseOrder == null)
            {
                return Problem("Entity set 'TMSContext.PurchaseOrder'  is null.");
            }
            PurchaseOrder.Vendor = null;
            foreach(var a in PurchaseOrder.PODetail)
            {
                a.Item = null;
            }
            await _PurchaseOrderRepo.Create(PurchaseOrder);

            return CreatedAtAction("GetPurchaseOrderById", new { id = PurchaseOrder.Id }, PurchaseOrder);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new PurchaseOrder();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<PurchaseOrder>> Update(PurchaseOrder PurchaseOrder)
    {
        try
        {
            if (PurchaseOrder == null)
            {
                return Problem("Entity set 'TMSContext.PurchaseOrder'  is null.");
            }

            await _PurchaseOrderRepo.Update(PurchaseOrder);

            return CreatedAtAction("GetPurchaseOrderById", new { id = PurchaseOrder.Id }, PurchaseOrder);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new PurchaseOrder(); }
    }

    // DELETE: api/PurchaseOrder/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeletePurchaseOrder(int id)
    //{
    //    if (_context.PurchaseOrder == null)
    //    {
    //        return NotFound();
    //    }
    //    var PurchaseOrder = await _context.PurchaseOrder.FindAsync(id);
    //    if (PurchaseOrder == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.PurchaseOrder.Remove(PurchaseOrder);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool PurchaseOrderExists(int id)
    //{
    //    return (_context.PurchaseOrder?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
