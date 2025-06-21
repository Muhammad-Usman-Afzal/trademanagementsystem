using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockTransactionsController : ControllerBase
{
    private readonly IStockTransactionsRepo _StockTransactionsRepo;

    public StockTransactionsController(IStockTransactionsRepo StockTransactionsRepo)
    {
        _StockTransactionsRepo = StockTransactionsRepo;
    }

    [Route("GetStockTransactionss")]
    [HttpGet]
    public async Task<ActionResult<List<StockTransactions>>> GetStockTransactionss()
    {
        try
        {
            return await _StockTransactionsRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<StockTransactions>();
        }
    }

    [HttpGet("GetStockTransactionsById")]
    public async Task<ActionResult<StockTransactions>> GetStockTransactionsById(int id)
    {
        try
        {
            return await _StockTransactionsRepo.GetById(id) ?? new StockTransactions();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new StockTransactions();
        }
    }

    // PUT: api/StockTransactions/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutStockTransactions(int id, StockTransactions StockTransactions)
    //{
    //    if (id != StockTransactions.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(StockTransactions).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!StockTransactionsExists(id))
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
    public async Task<ActionResult<StockTransactions>> Create(StockTransactions StockTransactions)
    {
        try
        {
            if (StockTransactions == null)
            {
                return Problem("Entity set 'TMSContext.StockTransactions'  is null.");
            }
            await _StockTransactionsRepo.Create(StockTransactions);

            return CreatedAtAction("GetStockTransactionsById", new { id = StockTransactions.Id }, StockTransactions);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new StockTransactions();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<StockTransactions>> Update(StockTransactions StockTransactions)
    {
        try
        {
            if (StockTransactions == null)
            {
                return Problem("Entity set 'TMSContext.StockTransactions'  is null.");
            }

            await _StockTransactionsRepo.Update(StockTransactions);

            return CreatedAtAction("GetStockTransactionsById", new { id = StockTransactions.Id }, StockTransactions);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new StockTransactions(); }
    }

    [HttpPost("BulkUpdate")]
    public async Task<ActionResult<bool>> BulkUpdate(List<StockTransactions> StockTransactionsList)
    {
        try
        {
            if (StockTransactionsList.Count <= 0)
            {
                return Problem("Entity set 'TMSContext.StockTransactions'  is null.");
            }
            foreach (var a in StockTransactionsList)
            {
                a.Item = null;
            }
            await _StockTransactionsRepo.UpdateDetail(StockTransactionsList);

            return true;
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return false; }
    }

    [HttpPost("BulkInsert")]
    public async Task<ActionResult<bool>> BulkInsert(List<StockTransactions> StockTransactionsList)
    {
        try
        {
            if (StockTransactionsList.Count <= 0)
            {
                return Problem("Entity set 'TMSContext.StockTransactions'  is null.");
            }
            foreach (var a in StockTransactionsList)
            {
                a.Item = null;
            }
            await _StockTransactionsRepo.CreateDetail(StockTransactionsList);

            return true;
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return false; }
    }

    [HttpPost("Merge")]
    public async Task<ActionResult<StockTransactions>> Merge(StockTransactions StockTransactions)
    {
        try
        {
            if (StockTransactions == null)
            {
                return Problem("Entity set 'TMSContext.StockTransactions'  is null.");
            }

            //await _StockTransactionsRepo.Merge<AppUsers, UserAuthorizedScreens>(AppUsers, AppUsers.AuthorizedScreens);

            return CreatedAtAction("GetStockTransactionsById", new { id = StockTransactions.Id }, StockTransactions);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new StockTransactions(); }
    }

    [HttpGet("GetItemWiseStock")]
    public async Task<ActionResult<List<ItemStockSummaryDTO>>> GetItemWiseStock()
    {
        try
        {
            return await _StockTransactionsRepo.GetItemWiseStock();

        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<ItemStockSummaryDTO>();
        }
    }

    // DELETE: api/StockTransactions/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteStockTransactions(int id)
    //{
    //    if (_context.StockTransactions == null)
    //    {
    //        return NotFound();
    //    }
    //    var StockTransactions = await _context.StockTransactions.FindAsync(id);
    //    if (StockTransactions == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.StockTransactions.Remove(StockTransactions);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool StockTransactionsExists(int id)
    //{
    //    return (_context.StockTransactions?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
