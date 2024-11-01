using System.Linq.Expressions;

namespace YaTrackerParser.Contracts.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Получить все записи
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Получить записиь по ID
    /// </summary>
    /// <param name="id">ID записи</param>
    /// <returns>Записи отобранные по ID или Null</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Поиск записей по условию
    /// </summary>
    /// <param name="predicate">Условия поиска</param>
    /// <returns>Отфильровыенные записи</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Добавить запись
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Обновить запись
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Удалить запись
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Сохранить изменения
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Получить или создать запись
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="createFunc"></param>
    /// <returns>Создает уникальную запись или обновляет информацию по уже имеющейся</returns>
    Task<T?> GetOrCreateAsync(Expression<Func<T, bool>> predicate);
}
