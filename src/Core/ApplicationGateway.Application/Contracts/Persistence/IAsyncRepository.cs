using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        //Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size);
        //Task<IReadOnlyList<T>> GetSortedPagedResponseAsync(int page, int size, string param, bool isDesc = false);
        //Task<IReadOnlyList<T>> GetSearchedListAsync(int page, int size, Func<T, bool> expression);
        Task<(IEnumerable<T> list, int count)> GetPagedListAsync(int page, int size, string sortParam = null, bool isDesc = false, Func<T, bool> expression = null);
        Task<int> GetTotalCount();

    }
}
