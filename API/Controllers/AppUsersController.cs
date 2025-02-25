namespace API.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AppUsersController : ControllerBase
{
    private readonly IAppUsersRepo _AppUsersRepo;
    private readonly TMSContext _context;

    public AppUsersController(IAppUsersRepo AppUsersRepo, TMSContext context)
    {
        _AppUsersRepo = AppUsersRepo;
        _context = context;
    }

    [Route("GetAppUsers")]
    [HttpGet]
    public async Task<ActionResult<List<AppUsers>>> GetAppUsers()
    {
        try
        {
            return await _AppUsersRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<AppUsers>();
        }
    }

    [HttpGet("GetAppUsersById")]
    public async Task<ActionResult<AppUsers>> GetAppUsersById(int id)
    {
        try
        {
            return await _AppUsersRepo.GetById(id) ?? new AppUsers();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new AppUsers();
        }
    }

    // PUT: api/AppUsers/5
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutAppUsers(int id, AppUsers AppUsers)
    //{
    //    if (id != AppUsers.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(AppUsers).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!AppUsersExists(id))
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
    public async Task<ActionResult<AppUsers>> Create(AppUsers AppUsers)
    {
        try
        {
            if (AppUsers == null)
            {
                return Problem("Entity set 'TMSContext.AppUsers'  is null.");
            }

            await _AppUsersRepo.Create(AppUsers);

            return CreatedAtAction("GetAppUsersById", new { id = AppUsers.Id }, AppUsers);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new AppUsers();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<AppUsers>> Update(AppUsers AppUsers)
    {
        try
        {
            if (AppUsers == null)
            {
                return Problem("Entity set 'TMSContext.AppUsers'  is null.");
            }

            //_context.UserAuthorizedScreens.Where(x => x.AppUserId == AppUsers.Id)

            await _AppUsersRepo.Update(AppUsers);

            return CreatedAtAction("GetAppUsersById", new { id = AppUsers.Id }, AppUsers);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new AppUsers(); }
    }

    // DELETE: api/AppUsers/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteAppUsers(int id)
    //{
    //    if (_context.AppUsers == null)
    //    {
    //        return NotFound();
    //    }
    //    var AppUsers = await _context.AppUsers.FindAsync(id);
    //    if (AppUsers == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.AppUsers.Remove(AppUsers);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool AppUsersExists(int id)
    //{
    //    return (_context.AppUsers?.Any(e => e.Id == id)).GetValueOrDefault();
    //}

    [AllowAnonymous]
    [HttpPost("ValidateLogin")]
    public async Task<ActionResult<AppUsers>> ValidateLogin(AppUsers oUser)
    {
        try
        {
            if (oUser == null || string.IsNullOrEmpty(oUser.UserName) || string.IsNullOrEmpty(oUser.Password))
                return Problem("Please provide Usercode & Password both");

            return await _AppUsersRepo.ValidateLogin(oUser);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new AppUsers(); }
    }

}