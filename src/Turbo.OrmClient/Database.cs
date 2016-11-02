using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Dapper;
using Turbo.OrmClient.Expressions;

namespace Turbo.OrmClient
{
    public partial class Database
    {
        public IDbConnection Connection { get; private set; }

        public ISqlDialectProvider DialectProvider { get; private set; }

        public Database(string name)
        {
            Connection = DatabaseConfiguration.GetConnection(name);
            DialectProvider = DatabaseConfiguration.GetSqlProvider(name);
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public IEnumerable<T> Select<T>(string sql = null, object param = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = new SqlExpression<T>(DialectProvider).SelectClause();
            }
            return Connection.Query<T>(sql, param);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).SelectClause();
            return Connection.Query<T>(sql, expression.Params);
        }

        public IEnumerable<T> Select<T>(ISqlExpression<T> expression)
        {
            return Connection.Query<T>(expression.SelectClause(), expression.Params);
        }

        public T Single<T>(string sql = null, object param = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = new SqlExpression<T>(DialectProvider).Take(1).SelectClause();
            }
            return Connection.QueryFirstOrDefault<T>(sql, param);
        }

        public T Single<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).Take(1).SelectClause();
            return Connection.QueryFirstOrDefault<T>(sql, expression.Params);
        }

        public TKey Scalar<TKey>(string sql, object param = null)
        {
            return Connection.ExecuteScalar<TKey>(sql, param);
        }

        public TKey Scalar<T, TKey>(Expression<Func<T, object>> field, Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).Select(field).SelectClause();
            return Connection.ExecuteScalar<TKey>(sql, expression.Params);
        }

        public TKey Scalar<T, TKey>(ISqlExpression<T> expression)
        {
            return Connection.ExecuteScalar<TKey>(expression.SelectClause(), expression.Params);
        }

        public long Count<T>(string sql=null, object param = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = new SqlExpression<T>(DialectProvider).CountClause();
            }
            return Connection.ExecuteScalar<long>(sql, param);
        }

        public long Count<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).CountClause();
            return Connection.ExecuteScalar<long>(sql, expression.Params);
        }

        public long Count<T>(ISqlExpression<T> expression)
        {
            return Connection.ExecuteScalar<long>(expression.CountClause(), expression.Params);
        }

        public long Insert(string sql, object param)
        {
            return Connection.ExecuteScalar<long>(sql, param);
        }

        public long Insert<T>(object fields)
        {
            var expression = DialectProvider.SqlExpression<T>();
            return Connection.ExecuteScalar<long>(expression.InsertClause(fields), expression.Params);
        }

        public int Update(string sql, object param)
        {
            return Connection.Execute(sql, param);
        }

        public int Update<T>(T param)
        {
            var expression = DialectProvider.SqlExpression<T>();
            return Connection.Execute(expression.UpdateClause(param), param);
        }

        public int Update<T>(object fields, Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>().Where(predicate);
            return Connection.Execute(expression.UpdateClause(fields), expression.Params);
        }

        public int Delete(string sql, object param)
        {
            return Connection.Execute(sql, param);
        }

        public int Delete<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>().Where(predicate);
            return Connection.Execute(expression.DeleteClause(), expression.Params);
        }

    }
}
