namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepo _OrderRepo;

    public OrderController(IOrderRepo OrderRepo)
    {
        _OrderRepo = OrderRepo;
    }

    [Route("GetOrders")]
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetOrders()
    {
        try
        {
            return await _OrderRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<Order>();
        }
    }

    [HttpGet("GetOrderById")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        try
        {
            return await _OrderRepo.GetById(id) ?? new Order();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new Order();
        }
    }

    //[HttpGet("GetOrderNumberByType")]
    //public async Task<ActionResult<List<Order>>> GetOrderNumberByType(OrderTypes orderType)
    //{
    //    try
    //    {
    //        return await _OrderRepo.GetByCondition(x=>x.OType == orderType).AsNoTracking() ?? new List<Order>();
    //    }
    //    catch (Exception ex)
    //    {
    //        APILogger.WriteLog(ex);
    //        return new List<Order>();
    //    }
    //}
    [HttpGet("GetOrderNumberByType")]
    public async Task<ActionResult<string>> GetOrderNumberByType([FromQuery] OrderTypes orderType)
    {
        try
        {
            //return _OrderRepo.GetByCondition(x => x.OType == orderType)?.OrderByDescending(x=>x.Id)?.FirstOrDefaultAsync()?.Result.OrderNo?? string.Empty;
            var lastOrder = await _OrderRepo.GetByCondition(x => x.OType == orderType)
                                   .OrderByDescending(x => x.Id)
                                   .FirstOrDefaultAsync();

            return lastOrder?.OrderNo ?? string.Empty;
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return string.Empty;
        }
    }



    // PUT: api/Order/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutOrder(int id, Order Order)
    //{
    //    if (id != Order.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(Order).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!OrderExists(id))
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
    public async Task<ActionResult<Order>> Create(Order Order)
    {
        try
        {
            if (Order == null)
            {
                return Problem("Entity set 'TMSContext.Order'  is null.");
            }
            Order.Party = null;
            foreach(var a in Order.OrderDetail)
            {
                a.Item = null;
            }
            await _OrderRepo.Create(Order);

            return CreatedAtAction("GetOrderById", new { id = Order.Id }, Order);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new Order();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<Order>> Update(Order Order)
    {
        try
        {
            if (Order == null)
            {
                return Problem("Entity set 'TMSContext.Order'  is null.");
            }

            await _OrderRepo.Update(Order);

            return CreatedAtAction("GetOrderById", new { id = Order.Id }, Order);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new Order(); }
    }

    // DELETE: api/Order/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteOrder(int id)
    //{
    //    if (_context.Order == null)
    //    {
    //        return NotFound();
    //    }
    //    var Order = await _context.Order.FindAsync(id);
    //    if (Order == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.Order.Remove(Order);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool OrderExists(int id)
    //{
    //    return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
