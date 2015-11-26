using System.Configuration;
using System.Runtime.Remoting.Messaging;
using ServiceStack.OrmLite;

namespace G1mist.Nancy.Model
{
    public static class ConnectionFactory
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        private static readonly string ProviderName = ConfigurationManager.ConnectionStrings["ConStr"].ProviderName;

        public static OrmLiteConnectionFactory GetCurrentConnection()
        {
            //从本地线程池中获取上下文对象
            var db = CallContext.GetData("DBContext") as OrmLiteConnectionFactory;

            //如果本地线程池中没有数据，则实例化一个上下文对象，并且把上下文对象放入本地线程池中
            if (db == null)
            {
                var provider = GetProvider(ProviderName);

                //TODO:实例化一个连接对象，然后加入本地线程池中。
                db = new OrmLiteConnectionFactory(ConnectionString, provider);

                CallContext.SetData("DBContext", db);
            }

            return db;
        }

        private static IOrmLiteDialectProvider GetProvider(string provider)
        {
            switch (provider)
            {
                case "MySql.Data.MySqlClient":
                    return MySqlDialect.Provider;
                case "System.Data.SqlClient":
                    return SqlServerDialect.Provider;
                default:
                    return MySqlDialect.Provider;
            }
        }
    }
}
