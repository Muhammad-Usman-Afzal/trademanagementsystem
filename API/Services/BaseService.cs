namespace API.Services;

public class BaseService<T> : IBaseRepo<T> where T : BaseEntity
{
    private readonly TMSContext _context;
    private DbSet<T> entities;

    public BaseService(TMSContext context)
    {
        _context = context;
        entities = context.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        try
        {
            return entities.AsNoTracking();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return null;
        }
    }

    public async Task<T> GetById(int id)
    {
        try
        {
            return await entities.FindAsync(id);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return null;
        }
    }

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
    {
        try
        {
            return entities.Where(expression).AsNoTracking();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return null;
        }
    }

    public Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression)
    {
        try
        {
            return entities.Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }
        catch (Exception ex) { APILogger.WriteLog(ex); return null; }
    }

    public bool GetBooleanByCondition(Expression<Func<T, bool>> expression)
    {
        try
        {
            return entities.Any(expression);
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return false;
        }
    }

    public async Task Create(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity required");
            }

            entity.CreateOn = DateTime.Now;

            await entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    public async Task<bool> CreateDetail(List<T> entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity required");
            }

            entity.ForEach(x => x.CreateOn = DateTime.Now);

            await entities.AddRangeAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return false;
        }
    }

    //public void Update(T entity) => entities.Update(entity);

    public async Task Update(T entity)
    {
        try
        {
            if (entity == null || entity.Id == 0)
            {
                throw new ArgumentNullException("entity required");
            }

            entity.UpdateOn = DateTime.Now;

            //entities.Update(entity);
            entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    public async Task UpdateDetail(List<T> EntitiesCollection)
    {
        try
        {
            if (EntitiesCollection == null)
            {
                throw new ArgumentNullException("entity required");
            }

            EntitiesCollection.ForEach(x => x.UpdateOn = DateTime.Now);

            //entities.Update(entity);
            entities.UpdateRange(EntitiesCollection);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    public async Task UpdateById(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity required");
            }
            entities.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    //public void Delete(T entity) => entities.Remove(entity);

    public async Task Delete(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity required");
            }
            entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    public async Task<bool> DeleteById(int Id)
    {
        try
        {
            //if (entity == null)
            //{
            //    throw new ArgumentNullException("entity required");
            //}
            entities.Remove(await GetById(Id));
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return false;
        }
    }

    //public void DeleteDetail(List<T> entity) => entities.RemoveRange(entity);

    public async Task DeleteDetail(List<T> entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity required");
            }
            entities.AttachRange(entity);
            //_context.Entry(entity).State = EntityState.Deleted;

            entities.RemoveRange(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    public async Task DeleteByCondition(Expression<Func<T, bool>> expression)
    {
        try
        {
            entities.RemoveRange(entities.Where(expression));
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
        }
    }

    public async Task<T> GetLastSavedRecord()
    {
        try
        {
            return await entities.AsNoTracking().OrderBy(x => x.Id).LastOrDefaultAsync();
        }
        catch (Exception ex)
        {
            APILogger.WriteLog(ex);
            return null;
        }
    }

	public async Task Merge<TParent, TChild>(TParent parentEntity, List<TChild> childEntities)
	where TParent : class
	where TChild : class
	{
		try
		{
			if (parentEntity == null || childEntities == null)
				throw new ArgumentNullException("Entity and ChildEntities are required");

			var parentSet = _context.Set<TParent>();
			var childSet = _context.Set<TChild>();

			#region 🔴 DELETE CASE

			if (parentEntity.GetType().GetProperty("IsDeleted")?.GetValue(parentEntity) as bool? == true)
			{
				parentSet.Remove(parentEntity);
				await _context.SaveChangesAsync();
				return;
			}

			var childrenToDelete = childEntities.Where(x =>
				x.GetType().GetProperty("IsDeleted")?.GetValue(x) as bool? == true).ToList();

			if (childrenToDelete.Any())
			{
				childSet.RemoveRange(childrenToDelete);
				await _context.SaveChangesAsync();
			}

			#endregion

			#region 🔵 UPDATE CASE

			parentEntity.GetType().GetProperty("UpdateOn")?.SetValue(parentEntity, DateTime.Now);
			foreach (var child in childEntities)
			{
				child.GetType().GetProperty("UpdateOn")?.SetValue(child, DateTime.Now);
			}

			parentSet.Update(parentEntity);
			childSet.UpdateRange(childEntities);

			await _context.SaveChangesAsync();

			#endregion
		}
		catch (Exception ex)
		{
			APILogger.WriteLog(ex);
			throw;
		}
	}

}
