namespace UI.Repositories;

public interface IBaseRepoUI<T> where T : BaseEntity
{
    Task<List<T>> GetAll(string APIName);
    Task<T> Create(string APIName, T entity);
    Task<List<T>> GetAllByCondition(string APIName);
    Task<T> GetSingleByCondition(string APIName);
    Task<T> Update(string APIName, T entity);
    Task<T> UpdateById(string APIName);
    Task UpdateDetail(string APIName, List<T> EntitiesCollection);
    Task<bool> BulkInsert(string APIName, List<T> EntitiesCollection);
    bool GetBooleanByCondition(string APIName);
    Task<T> GetLastRecord(string APIName);
    string Decryption(string cipherText);
}