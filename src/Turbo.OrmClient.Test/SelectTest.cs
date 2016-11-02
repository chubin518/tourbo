using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Test
{
    [TestClass]
    public class SelectTest
    {
        Database db;
        public SelectTest()
        {
            db = new Database("DefaultConnection");
        }

        [TestMethod]
        public void Test1()
        {
            IEnumerable<User> users = db.Select<User>();
        }
        [TestMethod]
        public void Test2()
        {
            IEnumerable<User> users = db.Select<User>(p => p.UserName.Contains("lxw3311") && p.Islocked == 0 && p.Isremove == 0);

        }

        [TestMethod]
        public void Test3()
        {
            User users = db.Single<User>(p => p.UserName.Contains("lxw3311") && p.Islocked == 0 && p.Isremove == 0);
        }
        [TestMethod]
        public void Test4()
        {
            var d = db.CountAsync<User>();
            long a = d.Result;
        }
        [TestMethod]
        public void Test5()
        {
            var d = db.Scalar<User, string>(p => p.Email, p => p.Id == 6);
        }
    }
}
