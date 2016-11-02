using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using Turbo.OrmClient.Expressions;
using System.Linq;
using ServiceStack;
using System.Collections.Generic;
namespace Turbo.OrmClient.Test
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void TestMethod1()
        {
            object d = new Author() { Active = false };
            Expression<Func<Author, object>> expression = (p) => p.Name;

            string[] fields = null;

            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                if (member.Member.DeclaringType.AssignableFrom(typeof(Author)))
                    fields = new[] { member.Member.Name };
            }

            NewExpression newExpr = expression.Body as NewExpression;
            if (newExpr != null)
                fields = newExpr.Arguments.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();

            MemberInitExpression init = expression.Body as MemberInitExpression;
            if (init != null)
                fields = init.Bindings.Select(x => x.Member.Name).ToArray();
        }

        [TestMethod]
        public void TestMethod2()
        {
            SqlExpression<Author> expression = new SqlExpression<Author>(new SqlServerDialectProvider());
            string sql = expression.SelectClause();
        }

        [TestMethod]
        public void TestMethod3()
        {
            Database db = new Database("DefaultConnection");
            db.Insert<Author>(new { Active = false });
        }
    }
}
