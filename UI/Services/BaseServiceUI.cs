namespace UI.Services;

public class BaseServiceUI<T> : IBaseRepoUI<T> where T : BaseEntity
{
    private readonly HttpClient _httpClient;

    public BaseServiceUI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<T>> GetAll(string APIName)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<T>>(APIName);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return new List<T>();
        }
    }

    public async Task<List<T>> GetAllByCondition(string APIName)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<T>>(APIName);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return new List<T>();
        }
    }

    public async Task<T> GetSingleByCondition(string APIName)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<T>(APIName);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return default(T);
        }
    }

    public async Task<T> GetLastRecord(string APIName)
    {
        try
        {
            return _httpClient.GetFromJsonAsync<T>(APIName).Result;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return default(T);
        }
    }

    public bool GetBooleanByCondition(string APIName)
    {
        try
        {
            return _httpClient.GetFromJsonAsync<bool>(APIName).Result;
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return false;
        }
    }

    //public async Task<T> GetById(int id) => await entities.FindAsync(id);

    //public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
    //    entities.Where(expression).AsNoTracking();

    //public async Task<T> Create(string APIName, T entity)
    //{
    //    try
    //    {
    //        if (entity == null)
    //            throw new ArgumentNullException("entity required");
    //        var res = await _httpClient.PostAsJsonAsync(APIName, entity).Result.Content.ReadFromJsonAsync<T>();

    //        if (!res.IsSuccessStatusCode)
    //            throw new ArgumentNullException("An error occured during API calling");

    //        return res;
    //    }
    //    catch (Exception ex)
    //    {

    //        throw;
    //    }
    //}

    public async Task<T> Create(string APIName, T entity)
    {
        try
        {
            return await _httpClient.PostAsJsonAsync(APIName, entity)
            .Result.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return default(T);
        }
    }

    public async Task<T> Update(string APIName, T entity)
    {
        try
        {
            return await _httpClient.PostAsJsonAsync(APIName, entity)
            .Result.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return default(T);
        }
    }

    public async Task<T> UpdateById(string APIName)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<T>(APIName);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return default(T);
        }
    }

    public async Task UpdateDetail(string APIName, List<T> EntitiesCollection)
    {
        try
        {
            await _httpClient.PostAsJsonAsync(APIName, EntitiesCollection);
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }
    }

    public async Task<bool> BulkInsert(string APIName, List<T> EntitiesCollection)
    {
        try
        {
            return await _httpClient.PostAsJsonAsync(APIName, EntitiesCollection)
            .Result.Content.ReadFromJsonAsync<bool>();
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
            return false;
        }
    }

    //public async Task CreateDetail(List<T> entity)
    //{
    //    if (entity == null)
    //    {
    //        throw new ArgumentNullException("entity required");
    //    }
    //    await entities.AddRangeAsync(entity);
    //    await _context.SaveChangesAsync();
    //}

    ////public void Update(T entity) => entities.Update(entity);

    //public async Task Update(T entity)
    //{
    //    if (entity == null)
    //    {
    //        throw new ArgumentNullException("entity required");
    //    }
    //    entities.Update(entity);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task UpdateById(T entity)
    //{
    //    if (entity == null)
    //    {
    //        throw new ArgumentNullException("entity required");
    //    }
    //    entities.Update(entity);
    //    await _context.SaveChangesAsync();
    //}

    ////public void Delete(T entity) => entities.Remove(entity);

    //public async Task Delete(T entity)
    //{
    //    if (entity == null)
    //    {
    //        throw new ArgumentNullException("entity required");
    //    }
    //    entities.Remove(entity);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task DeleteById(int Id)
    //{
    //    //if (entity == null)
    //    //{
    //    //    throw new ArgumentNullException("entity required");
    //    //}
    //    entities.Remove(await GetById(Id));
    //    await _context.SaveChangesAsync();
    //}

    ////public void DeleteDetail(List<T> entity) => entities.RemoveRange(entity);

    //public async Task DeleteDetail(List<T> entity)
    //{
    //    if (entity == null)
    //    {
    //        throw new ArgumentNullException("entity required");
    //    }
    //    entities.RemoveRange(entity);
    //    await _context.SaveChangesAsync();
    //}

    public string Decryption(string cipherText)
    {
        string val = "";
        try
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            val = streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UILogger.WriteLog(ex);
        }
        return val;
    }

}