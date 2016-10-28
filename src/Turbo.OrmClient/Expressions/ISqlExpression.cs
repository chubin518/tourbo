using System;
using System.Linq.Expressions;
using Turbo.OrmClient.Dapper;

namespace Turbo.OrmClient.Expressions
{
    public interface ISqlExpression<T>
    {
        DynamicParameters Params { get; }
        ModelDefinition modelDef { get; }

        string DeleteClause();
        SqlExpression<T> Distinct();
        SqlExpression<T> GroupBy(params string[] fields);
        SqlExpression<T> GroupBy(Expression<Func<T, object>> fields);
        SqlExpression<T> Having(string sqlFilter);
        SqlExpression<T> Insert(Expression<Func<T, object>> fields);
        string InsertClause(object fields);
        SqlExpression<T> OrderBy(params string[] fields);
        SqlExpression<T> OrderBy(Expression<Func<T, object>> fields);
        SqlExpression<T> OrderByDescending(params string[] fields);
        SqlExpression<T> OrderByDescending(Expression<Func<T, object>> fields);
        SqlExpression<T> Select(params string[] fields);
        SqlExpression<T> Select(Expression<Func<T, object>> fields);
        string SelectClause();
        SqlExpression<T> Skip(uint skip = 0);
        SqlExpression<T> Take(uint take = 0);
        SqlExpression<T> Update(Expression<Func<T, object>> fields);
        string UpdateClause(object obj);
        SqlExpression<T> Where(Expression<Func<T, bool>> predicate);
        string CountClause();
        void CreateParam(object value, ref string name);
    }
}