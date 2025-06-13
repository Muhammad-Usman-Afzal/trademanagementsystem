namespace API.Repositories;

public interface IBaseRepo<T> where T : BaseEntity
{
    IQueryable<T> GetAll();
    Task<T> GetById(int id);
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
    Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression);
    Task Create(T entity);
    Task Update(T entity);
    Task UpdateDetail(List<T> EntitiesCollection);
    Task Delete(T entity);
    Task<bool> CreateDetail(List<T> entity);
    Task DeleteDetail(List<T> entity);
    Task UpdateById(T entity);
    Task DeleteByCondition(Expression<Func<T, bool>> expression);
    //Task UpdateUserWorkStatus(int Id, bool IsAnyTeamMemberWorking);
    bool GetBooleanByCondition(Expression<Func<T, bool>> expression);
    Task<T> GetLastSavedRecord();
    Task<bool> DeleteById(int Id);
	Task Merge<TParent, TChild>(TParent parentEntity, List<TChild> childEntities)
		where TParent : class
		where TChild : class;
}
