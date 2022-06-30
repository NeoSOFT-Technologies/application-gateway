﻿using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ApplicationGateway.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        public BaseRepository(ApplicationDbContext dbContext, ILogger<T> logger)
        {
            _dbContext = dbContext; _logger = logger;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            _logger.LogInformation("ListAllAsync Initiated");
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<(IEnumerable<T> list, int count)> GetPagedListAsync(int page, int size, string sortParam=null, bool isDesc = false,  Func<T, bool> expression=null)
        {
            var GetPagedList = new Func<IEnumerable<T>, IEnumerable<T>>(list => { return list.Skip((page - 1) * size).Take(size); });
            StringBuilder queryBuilder = new();
            //retrieve all record if no pagination
            if (size == 0 || page == 0)
            {
                page = 1;
                size = await GetTotalCount();
            }
            //check if => sort then set queryBuiler
            if (!string.IsNullOrWhiteSpace(sortParam))
            {
                string name = ValidateParam(sortParam);
                string sortingOrder = isDesc ? "descending" : "ascending";
                queryBuilder.Append($"{ name } { sortingOrder}");
            }
            //for both search and sort
            if (!string.IsNullOrWhiteSpace(sortParam) && expression != null)
            {
                var list =  _dbContext.Set<T>().OrderBy(queryBuilder.ToString()).Where(expression).ToList();
                return (list: GetPagedList(list),count: list.Count());
            }
            //for sort
            else if (!string.IsNullOrWhiteSpace(sortParam))
            {
                var list = await _dbContext.Set<T>().OrderBy(queryBuilder.ToString()).AsNoTracking().ToListAsync();
                return (list: GetPagedList(list), count: list.Count());
            }
            //for search
            else if (expression != null)
            {
                var list = await _dbContext.Set<T>().Where(expression).ToDynamicListAsync<T>();
                return (list: GetPagedList(list), count: list.Count());
            }

            //return all record
            else
            {
                var list = await _dbContext.Set<T>().AsNoTracking().ToListAsync();
                return (list: GetPagedList(list), count: list.Count());
            }
        }

        //public async virtual Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size)
        //{
        //    return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        //}

        //public async Task<IReadOnlyList<T>> GetSortedPagedResponseAsync(int page, int size, string param, bool isDesc = false)
        //{
        //    if (string.IsNullOrWhiteSpace(param))
        //        throw new NotFoundException("Sorting parameter", param);
        //    string name = ValidateParam(param);
        //    StringBuilder queryBuilder = new();
        //    string sortingOrder = isDesc ? "descending" : "ascending";
        //    queryBuilder.Append($"{ name } { sortingOrder}");

        //    return await _dbContext.Set<T>().OrderBy(queryBuilder.ToString()).Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        //}
        //public async Task<IReadOnlyList<T>> GetSearchedListAsync(int page, int size, Func<T, bool> expression)
        //{
        //    return await _dbContext.Set<T>().Where(expression).Skip((page - 1) * size).Take(size).ToDynamicListAsync<T>();
        //}
        public string ValidateParam(string param)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(param, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
                throw new BadRequestException(param + " doesn't exists");
            return objectProperty.Name;
        }

        public async virtual Task<int> GetTotalCount()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
