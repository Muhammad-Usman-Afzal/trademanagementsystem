namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailRepo _OrderDetailRepo;

    public OrderDetailController(IOrderDetailRepo OrderDetailRepo)
    {
        _OrderDetailRepo = OrderDetailRepo;
    }

    [Route("GetOrderDetails")]
    [HttpGet]
    public async Task<ActionResult<List<OrderDetail>>> GetOrderDetails()
    {
        try
        {
            return await _OrderDetailRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<OrderDetail>();
        }
    }

    [HttpGet("GetOrderDetailById")]
    public async Task<ActionResult<OrderDetail>> GetOrderDetailById(int id)
    {
        try
        {
            return await _OrderDetailRepo.GetById(id) ?? new OrderDetail();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new OrderDetail();
        }
    }

    // PUT: api/OrderDetail/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutOrderDetail(int id, OrderDetail OrderDetail)
    //{
    //    if (id != OrderDetail.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(OrderDetail).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!OrderDetailExists(id))
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
    public async Task<ActionResult<OrderDetail>> Create(OrderDetail OrderDetail)
    {
        try
        {
            if (OrderDetail == null)
            {
                return Problem("Entity set 'TMSContext.OrderDetail'  is null.");
            }
            await _OrderDetailRepo.Create(OrderDetail);

            return CreatedAtAction("GetOrderDetailById", new { id = OrderDetail.Id }, OrderDetail);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new OrderDetail();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<OrderDetail>> Update(OrderDetail OrderDetail)
    {
        try
        {
            if (OrderDetail == null)
            {
                return Problem("Entity set 'TMSContext.OrderDetail'  is null.");
            }

            await _OrderDetailRepo.Update(OrderDetail);

            return CreatedAtAction("GetOrderDetailById", new { id = OrderDetail.Id }, OrderDetail);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new OrderDetail(); }
    }
    
    [HttpPost("BulkUpdate")]
    public async Task<ActionResult<bool>> BulkUpdate(List<OrderDetail> OrderDetailList)
    {
        try
        {
            if (OrderDetailList.Count <= 0)
            {
                return Problem("Entity set 'TMSContext.OrderDetail'  is null.");
            }
            foreach (var a in OrderDetailList)
            {
                a.Item = null;
            }
            await _OrderDetailRepo.UpdateDetail(OrderDetailList);

            return true;
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return false; }
    }

    // DELETE: api/OrderDetail/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteOrderDetail(int id)
    //{
    //    if (_context.OrderDetail == null)
    //    {
    //        return NotFound();
    //    }
    //    var OrderDetail = await _context.OrderDetail.FindAsync(id);
    //    if (OrderDetail == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.OrderDetail.Remove(OrderDetail);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool OrderDetailExists(int id)
    //{
    //    return (_context.OrderDetail?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
