using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninesky.Core.Types;

namespace Ninesky.Core
{
    public class AdministratorManager : BaseManager<Administrator>
    {
        /// <summary>
        /// 添加
        /// </summary>
        public override Response Add(Administrator admin)
        {
            Response _resp = new Response();
            if (HasAccounts(admin.Accounts))
            {
                _resp.Code = 0;
                _resp.Message = "账号已存在";
            }
            else _resp = base.Add(admin);
            return _resp;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="administratorID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Response ChangePassword(int administratorID, string password)
        {
            Response _resp = new Response();
            var _admin = Repository.Find(administratorID);
            if (_admin == null)
            {
                _resp.Code = 0;
                _resp.Message = "该主键的管理员不存在";
            }
            else
            {
                _admin.Password = password;
                _resp = Update(_admin);
            }
            return _resp;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="administratorID"></param>
        /// <returns></returns>
        public override Response Delete(int administratorID)
        {
            Response _resp = new Response();
            if (Count() == 1)
            {
                _resp.Code = 0;
                _resp.Message = "不能删除唯一的管理员账号";
            }
            else _resp = base.Delete(administratorID);
            return _resp;
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public Administrator Find(string accounts)
        {
            return base.Repository.Find(a => a.Accounts == accounts);
        }
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public bool HasAccounts(string accounts)
        {
            return base.Repository.isContains(a => a.Accounts.ToUpper() == accounts.ToUpper());
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="administratorID"></param>
        /// <param name="ip"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Response UpdateLoginInfo(int administratorID, string ip, DateTime time)
        {
            Response _resp = new Response();
            var _admin = Find(administratorID);
            if (_admin == null)
            {
                _resp.Code = 0;
                _resp.Message = "该主键的管理员不存在";
            }
            else
            {
                _admin.LoginIP = ip;
                _admin.LoginTime = time;
                _resp = Update(_admin);
            }
            return _resp;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="accounts">账号</param>
        /// <param name="password">密码</param>
        /// <returns>Code:1-成功；2-账号不存在，3-密码错误</returns>

        public Response Verify(string accounts, string password)
        {
            Response _resp = new Response();
            var _admin = base.Repository.Find(a => a.Accounts == accounts);
            if (_admin == null)
            {
                _resp.Code = 2;
                _resp.Message = $"账号为【{accounts}】的管理员不存在";
            }
            else if (_admin.Password == password)
            {
                _resp.Code = 1;
                _resp.Message = $"验证通过";
            }
            else
            {
                _resp.Code = 3;
                _resp.Message = $"账号密码错误";
            }
            return _resp;
        }
    }
}
