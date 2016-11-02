using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Test
{
    public class User
    {
        public User()
        {

        }

        ///<summary>
        ///主键ID
        ///</summary>
        public long Id
        {
            get;
            set;
        }

        ///<summary>
        ///用户名
        ///</summary>
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        ///密码
        ///</summary>
        public string Password
        {
            get;
            set;
        }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string Name
        {
            get;
            set;
        }

        ///<summary>
        ///所属公司
        ///</summary>
        public string Companyname
        {
            get;
            set;
        }

        ///<summary>
        ///备注
        ///</summary>
        public string Remark
        {
            get;
            set;
        }

        ///<summary>
        ///邮箱
        ///</summary>
        public string Email
        {
            get;
            set;
        }

        ///<summary>
        ///联系电话
        ///</summary>
        public string Mobile
        {
            get;
            set;
        }

        ///<summary>
        ///联系地址
        ///</summary>
        public string Address
        {
            get;
            set;
        }

        ///<summary>
        ///是否删除 0-未删除；1-已删除
        ///</summary>
        public System.SByte Isremove
        {
            get;
            set;
        }

        ///<summary>
        ///登录次数
        ///</summary>
        public long Logintotal
        {
            get;
            set;
        }

        ///<summary>
        ///登录失败次数
        ///</summary>
        public int Failedtotal
        {
            get;
            set;
        }

        ///<summary>
        ///最后登录IP
        ///</summary>
        public string Loginip
        {
            get;
            set;
        }

        ///<summary>
        ///最后登录时间
        ///</summary>
        public DateTime Logintime
        {
            get;
            set;
        }

        ///<summary>
        ///创建人
        ///</summary>
        public string Creator
        {
            get;
            set;
        }

        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime Createtime
        {
            get;
            set;
        }

        ///<summary>
        ///账户锁定截至时间
        ///</summary>
        public DateTime Lockendtime
        {
            get;
            set;
        }

        ///<summary>
        ///是否启用账户锁定功能：0-不启用；1-启用
        ///</summary>
        public System.SByte Lockoutenabled
        {
            get;
            set;
        }

        ///<summary>
        ///是否锁定状态：0-未锁定；1-已锁定
        ///</summary>
        public System.SByte Islocked
        {
            get;
            set;
        }

        ///<summary>
        ///启用双重身份验证:0-不启用;1-启用
        ///</summary>
        public System.SByte Twofactorenabled
        {
            get;
            set;
        }

        ///<summary>
        ///是否已经email确认：0-未确认；1-已确认
        ///</summary>
        public System.SByte Emailconfirmed
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已经手机确认：0-未确认；1-已确认
        /// </summary>
        public SByte Mobileconfirmed
        {
            get;
            set;
        }

        ///<summary>
        ///用户安全凭据，当用户凭据发生更改时（密码改变，登录删除）产生的一个随机值
        ///</summary>
        public string Securitystamp
        {
            get;
            set;
        }
    }
}
