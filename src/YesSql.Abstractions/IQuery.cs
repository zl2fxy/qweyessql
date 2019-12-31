using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YesSql.Indexes;

namespace YesSql
{
    public interface IQuery
    {
        /// <summary>
        /// Adds a filter on the document type
        /// </summary>
        /// <param name="filterType">If <c>false</c> the document type won't be filtered.</param>
        /// <typeparam name="T">The type of document to return</typeparam>
        IQuery<T> For<T>(bool filterType = true) where T : class;

        /// <summary>
        /// Defines what type of index should be returned
        /// </summary>
        /// <typeparam name="T">The type of index to return</typeparam>
        IQueryIndex<T> ForIndex<T>() where T : class, IIndex;

        /// <summary>
        /// Returns documents from any type
        /// </summary>
        IQuery<object> Any();
    }

    /// <summary>
    /// Represents a query over an entity
    /// </summary>
    /// <typeparam name="T">The type to return. It can be and index or an entity</typeparam>
    public interface IQuery<T> where T : class
    {
        IQuery<T, TIndex> With<TIndex>() where TIndex : class, IIndex;
        IQuery<T, TIndex> With<TIndex>(Expression<Func<TIndex, bool>> predicate) where TIndex : class, IIndex;
        IQuery<T> Skip(int count);
        IQuery<T> Take(int count);
        Task<T> FirstOrDefaultAsync();
        Task<IEnumerable<T>> ListAsync();
        IAsyncEnumerable<T> ToAsyncEnumerable();
        Task<int> CountAsync();

        //{{ * qwe新增修改
        /// <summary>
        /// 添加绑定Index
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IQuery<T> BindIndexTable(string tableName);
        /// <summary>
        /// 添加SQL where语句支持
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IQuery<T> QweWhere(string sql);
        /// <summary>
        /// 添加SQL OrderBy语句支持
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IQuery<T> QweOrderBy(string keySelector);
        /// <summary>
        /// 添加SQL OrderByDescending语句支持
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IQuery<T> QweOrderByDescending(string keySelector);
        /// <summary>
        /// 添加SQL ThenBy语句支持
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IQuery<T> QweThenBy(string keySelector);
        /// <summary>
        /// 添加SQL ThenByDescending语句支持
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IQuery<T> QweThenByDescending(string keySelector);

        //修改结束 * }}


    }

    /// <summary>
    /// Represents a query over an index, which can be ordered.
    /// </summary>
    /// <typeparam name="T">The index's type to query over.</typeparam>
    public interface IQueryIndex<T> where T : IIndex
    {
        IQueryIndex<TIndex> With<TIndex>() where TIndex : class, IIndex;
        IQueryIndex<TIndex> With<TIndex>(Expression<Func<TIndex, bool>> predicate) where TIndex : class, IIndex;
        IQueryIndex<T> Where(string sql);
        IQueryIndex<T> Where(Func<ISqlDialect, string> sql);
        IQueryIndex<T> WithParameter(string name, object value);
        IQueryIndex<T> Where(Expression<Func<T, bool>> predicate);
        IQueryIndex<T> OrderBy(Expression<Func<T, object>> keySelector);
        IQueryIndex<T> OrderByDescending(Expression<Func<T, object>> keySelector);
        IQueryIndex<T> ThenBy(Expression<Func<T, object>> keySelector);
        IQueryIndex<T> ThenByDescending(Expression<Func<T, object>> keySelector);
        IQueryIndex<T> Skip(int count);
        IQueryIndex<T> Take(int count);
        Task<T> FirstOrDefaultAsync();
        Task<IEnumerable<T>> ListAsync();
        IAsyncEnumerable<T> ToAsyncEnumerable();
        Task<int> CountAsync();
    }

    /// <summary>
    /// Represents a query over an index that targets a specific entity.
    /// </summary>
    /// <typeparam name="T">The entity's type to return.</typeparam>
    /// <typeparam name="TIndex">The index's type to query over.</typeparam>
    public interface IQuery<T, TIndex> : IQuery<T>
        where T : class
        where TIndex : IIndex
    {
        IQuery<T, TIndex> Where(string sql);
        IQuery<T, TIndex> Where(Func<ISqlDialect, string> sql);
        IQuery<T, TIndex> WithParameter(string name, object value);
        IQuery<T, TIndex> Where(Expression<Func<TIndex, bool>> predicate);
        IQuery<T, TIndex> OrderBy(Expression<Func<TIndex, object>> keySelector);
        IQuery<T, TIndex> OrderBy(string sql);
        IQuery<T, TIndex> OrderByDescending(Expression<Func<TIndex, object>> keySelector);
        IQuery<T, TIndex> OrderByDescending(string sql);
        IQuery<T, TIndex> ThenBy(Expression<Func<TIndex, object>> keySelector);
        IQuery<T, TIndex> ThenBy(string sql);
        IQuery<T, TIndex> ThenByDescending(Expression<Func<TIndex, object>> keySelector);
        IQuery<T, TIndex> ThenByDescending(string sql);
    }
}
