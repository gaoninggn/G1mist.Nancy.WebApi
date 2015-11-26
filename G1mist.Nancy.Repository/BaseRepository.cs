using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using ServiceStack.OrmLite;

namespace G1mist.Nancy.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private OrmLiteConnectionFactory Context { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseRepository()
        {
            Context = ConnectionFactory.GetCurrentConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(T entity)
        {
            using (var conn = Context.Open())
            {
                return conn.Insert(entity) > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            using (var conn = Context.Open())
            {
                return conn.Update(entity) > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            using (var conn = Context.Open())
            {
                return conn.Delete(entity) > 0;
            }
        }

        /// <summary>
        /// 计算数据量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public long Count(Expression<Func<T, bool>> where)
        {
            using (var conn = Context.Open())
            {
                return conn.Count(where);
            }
        }

        public T GetModal(Expression<Func<T, bool>> where)
        {
            using (var conn = Context.Open())
            {
                return conn.Single(where);
            }
        }

        public T First(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool isAesc = true)
        {
            using (var conn = Context.Open())
            {
                return isAesc ? conn.Select(conn.From<T>().Where(@where).OrderBy(orderBy)).FirstOrDefault() : conn.Select(conn.From<T>().Where(@where).OrderByDescending(orderBy)).FirstOrDefault();
            }
        }

        public ICollection<T> GetList(Expression<Func<T, bool>> where)
        {
            using (var conn = Context.Open())
            {
                return conn.Select(where);
            }
        }

        public bool Exits(Expression<Func<T, bool>> where)
        {
            using (var conn = Context.Open())
            {
                return conn.Exists(where);
            }
        }

        public ICollection<T> GetList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool isAesc = true)
        {
            using (var conn = Context.Open())
            {
                return conn.Select(isAesc ? conn.From<T>().Where(@where).OrderBy(orderBy) : conn.From<T>().Where(@where).OrderByDescending(orderBy));
            }
        }

        public ICollection<T> GetListByPage(int offset, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, out int totalCount, bool isAesc = true)
        {
            using (var conn = Context.Open())
            {
                if (isAesc)
                {
                    var result = conn.Select(conn.From<T>().Where(where).OrderBy(orderBy).Skip(offset).Take(pageSize));

                    totalCount = (int)conn.Count<T>();
                    return result;
                }
                else
                {
                    var result = conn.Select(conn.From<T>().Where(where).OrderByDescending(orderBy).Skip(offset).Take(pageSize));

                    totalCount = (int)conn.Count<T>();
                    return result;
                }
            }
        }

    }
}
