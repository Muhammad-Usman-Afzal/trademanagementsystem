namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PartyController : ControllerBase
{
    private readonly IPartyRepo _PartyRepo;

    public PartyController(IPartyRepo PartyRepo)
    {
        _PartyRepo = PartyRepo;
    }

    [Route("GetParties")]
    [HttpGet]
    public async Task<ActionResult<List<Party>>> GetParties()
    {
        try
        {
            return await _PartyRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<Party>();
        }
    }

    [HttpGet("GetPartyById")]
    public async Task<ActionResult<Party>> GetPartyById(int id)
    {
        try
        {
            return await _PartyRepo.GetById(id) ?? new Party();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new Party();
        }
    }

    // PUT: api/Party/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutParty(int id, Party Party)
    //{
    //    if (id != Party.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(Party).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!PartyExists(id))
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
    public async Task<ActionResult<Party>> Create(Party Party)
    {
        try
        {
            if (Party == null)
            {
                return Problem("Entity set 'TMSContext.Party'  is null.");
            }

            await _PartyRepo.Create(Party);

            return CreatedAtAction("GetPartyById", new { id = Party.Id }, Party);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new Party();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<Party>> Update(Party Party)
    {
        try
        {
            if (Party == null)
            {
                return Problem("Entity set 'TMSContext.Party'  is null.");
            }

            await _PartyRepo.Update(Party);

            return CreatedAtAction("GetPartyById", new { id = Party.Id }, Party);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new Party(); }
    }

    // DELETE: api/Party/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteParty(int id)
    //{
    //    if (_context.Party == null)
    //    {
    //        return NotFound();
    //    }
    //    var Party = await _context.Party.FindAsync(id);
    //    if (Party == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.Party.Remove(Party);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool PartyExists(int id)
    //{
    //    return (_context.Party?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
