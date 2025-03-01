namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MasterMenuController : ControllerBase
{
    private readonly IMasterMenuRepo _MasterMenuRepo;

    public MasterMenuController(IMasterMenuRepo MasterMenuRepo)
    {
        _MasterMenuRepo = MasterMenuRepo;
    }

    [Route("GetMasterMenus")]
    [HttpGet]
    public async Task<ActionResult<List<MasterMenu>>> GetMasterMenus()
    {
        try
        {
            return await _MasterMenuRepo.GetAll().ToListAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new List<MasterMenu>();
        }
    }

    [HttpGet("GetMasterMenuById")]
    public async Task<ActionResult<MasterMenu>> GetMasterMenuById(int id)
    {
        try
        {
            return await _MasterMenuRepo.GetById(id) ?? new MasterMenu();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new MasterMenu();
        }
    }

    [HttpPost("Create")]
    public async Task<ActionResult<MasterMenu>> Create(MasterMenu MasterMenu)
    {
        try
        {
            if (MasterMenu == null)
            {
                return Problem("Entity set 'TMSContext.MasterMenu'  is null.");
            }

            await _MasterMenuRepo.Create(MasterMenu);

            return CreatedAtAction("GetMasterMenuById", new { id = MasterMenu.Id }, MasterMenu);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return new MasterMenu();
        }
    }

    [HttpPost("Update")]
    public async Task<ActionResult<MasterMenu>> Update(MasterMenu MasterMenu)
    {
        try
        {
            if (MasterMenu == null)
            {
                return Problem("Entity set 'TMSContext.MasterMenu'  is null.");
            }

            await _MasterMenuRepo.Update(MasterMenu);

            return CreatedAtAction("GetMasterMenuById", new { id = MasterMenu.Id }, MasterMenu);
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return new MasterMenu(); }
    }

}
