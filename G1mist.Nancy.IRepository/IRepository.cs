using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace G1mist.Nancy.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <returns></returns>
        bool Insert(T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <returns></returns>
        bool Delete(T entity);

        T First(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool isAesc = true);

        long Count(Expression<Func<T, bool>> where);

        /// <summary>
        /// 通过where条件获取单个实体
        /// </summary>
        /// <param name="where">where条件</param>
        /// <returns></returns>
        T GetModal(Expression<Func<T, bool>> where);

        /// <summary>
        /// 通过where条件获取所有符合条件的实体
        /// </summary>
        /// <param name="where">where条件</param>
        /// <returns></returns>
        ICollection<T> GetList(Expression<Func<T, bool>> where);

        /// <summary>
        /// 通过where条件判断符合条件的实体是否存在
        /// </summary>
        /// <param name="where">where条件</param>
        /// <returns></returns>
        bool Exits(Expression<Func<T, bool>> where);

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="isAesc">是否正序</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        ICollection<T> GetList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool isAesc = true);

        /// <summary>
        /// 分页获取实体列表
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="where">where查询条件</param>
        /// <param name="orderBy">order排序条件</param>
        /// <param name="totalCount">总数</param>
        /// <param name="isAsc">是否升序,默认升序</param>
        /// <returns></returns>
        ICollection<T> GetListByPage(int offset, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy,
            out int totalCount, bool isAesc = true);
    }
}
