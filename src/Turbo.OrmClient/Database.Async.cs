using System;
using System.Collections.Generic;
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
        public Task<IEnumerable<T>> SelectAsync<T>(string sql = null, object param = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = new SqlExpression<T>(DialectProvider).SelectClause();
            }
            return Connection.QueryAsync<T>(sql, param);
        }

        public Task<IEnumerable<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).SelectClause();
            return Connection.QueryAsync<T>(sql, expression.Params);
        }

        public Task<IEnumerable<T>> SelectAsync<T>(ISqlExpression<T> expression)
        {
            return Connection.QueryAsync<T>(expression.SelectClause(), expression.Params);
        }

        public Task<T> SingleAsync<T>(string sql = null, object param = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = new SqlExpression<T>(DialectProvider).Take(1).SelectClause();
            }
            return Connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).Take(1).SelectClause();
            return Connection.QueryFirstOrDefaultAsync<T>(sql, expression.Params);
        }

        public Task<TKey> ScalarAsync<TKey>(string sql, object param = null)
        {
            return Connection.ExecuteScalarAsync<TKey>(sql, param);
        }

        public Task<TKey> ScalarAsync<T, TKey>(Expression<Func<T, object>> field, Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).Select(field).SelectClause();
            return Connection.ExecuteScalarAsync<TKey>(sql, expression.Params);
        }

        public Task<TKey> ScalarAsync<T, TKey>(ISqlExpression<T> expression)
        {
            return Connection.ExecuteScalarAsync<TKey>(expression.SelectClause(), expression.Params);
        }

        public Task<long> CountAsync<T>(string sql, object param = null)
        {
            return Connection.ExecuteScalarAsync<long>(sql, param);
        }

        public Task<long> CountAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>();
            string sql = expression.Where(predicate).CountClause();
            return Connection.ExecuteScalarAsync<long>(sql, expression.Params);
        }

        public Task<long> CountAysnc<T>(ISqlExpression<T> expression)
        {
            return Connection.ExecuteScalarAsync<long>(expression.CountClause(), expression.Params);
        }

        public Task<long> InsertAsync(string sql, object param)
        {
            return Connection.ExecuteScalarAsync<long>(sql, param);
        }

        public Task<long> InsertAsync<T>(object fields)
        {
            var expression = DialectProvider.SqlExpression<T>();
            return Connection.ExecuteScalarAsync<long>(expression.InsertClause(fields), expression.Params);
        }

        public Task<int> UpdateAsync(string sql, object param)
        {
            return Connection.ExecuteAsync(sql, param);
        }

        public Task<int> UpdateAsync<T>(T param)
        {
            var expression = DialectProvider.SqlExpression<T>();
            return Connection.ExecuteAsync(expression.UpdateClause(param), param);
        }

        public Task<int> UpdateAsync<T>(object fields, Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>().Where(predicate);
            return Connection.ExecuteAsync(expression.UpdateClause(fields), expression.Params);
        }

        public Task<int> DeleteAsync(string sql, object param)
        {
            return Connection.ExecuteAsync(sql, param);
        }

        public Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var expression = DialectProvider.SqlExpression<T>().Where(predicate);
            return Connection.ExecuteAsync(expression.DeleteClause(), expression.Params);
        }
    }
}
