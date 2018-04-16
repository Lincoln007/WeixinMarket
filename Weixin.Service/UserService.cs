using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;
using Weixin.Iserivce;
using Weixin.Service.Entities;
using Weixin.Core.Helper;
using System.Data.Entity;

namespace Weixin.Service
{
    public class UserService : IUserService
    {
        public async Task<long> AddNew(string userName, string phoneNum, string password)
        {
            User user = new User()
            {
                UserName = userName,
                PhoneNum = phoneNum
            };
            string salt = CommonHelper.CreateVerifyCode(5);
            user.PasswordSalt = salt;
            var passwordHash = CommonHelper.CalcMD5(password + salt);
            user.PasswordHash = passwordHash;
            using (var db=new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                bool exists = await commonService.GetAll().AnyAsync(u => u.PhoneNum == phoneNum);
                bool exists2 = await commonService.GetAll().AnyAsync(u => u.UserName == userName);
                if (exists)
                {
                    throw new ArgumentException("手机号"+ phoneNum + "已经存在");
                }
                if (exists2)
                {
                    throw new ArgumentException("用户名" + userName + "已经存在");
                }
                db.User.Add(user);
                await db.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<bool> CheckLogin(string userName, string password)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var user = await commonService.GetAll().SingleOrDefaultAsync(u => u.UserName == userName);
                if (user==null)
                {
                    return false;
                }
                string dbPasswordHash = user.PasswordHash;//取出数据库中的密码hash
                string inputPasswordHash = CommonHelper.CalcMD5(user.PasswordSalt + password); //根据输入的密码计算出hash
                return dbPasswordHash == inputPasswordHash;
            }
        }

        public async Task Edit(long userId, string phoneNum, string password)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var user = await commonService.GetById(userId);
                if (user==null)
                {
                    throw new ArgumentException("该用户不存在！");
                }
                user.PhoneNum = phoneNum;
                user.PasswordHash = CommonHelper.CalcMD5(user.PasswordSalt + password);
                await db.SaveChangesAsync();
            }
        }

        public async Task<UserDTO[]> GetAll()
        {
            using(var db=new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var all = await commonService.GetAll().ToListAsync();
                return (all.Select(u => ToDTO(u))).ToArray();

            }
        }

        public async Task<UserDTO> GetById(long userId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var user = await commonService.GetById(userId);
                return ToDTO(user);
            }
        }

        public async Task<UserDTO> GetByPhoneNum(string phoneNum)
        {
            using (var db=new WeixinDbContext())
            {
                var user = await db.User.SingleOrDefaultAsync(u => u.PhoneNum == phoneNum);
                return ToDTO(user);
            }
        }

        public bool HasPermission(long userId, string permissionName)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var user = commonService.GetAll().Include(u => u.Roles).
                    AsNoTracking().SingleOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new ArgumentException("找不到id=" + userId + "的用户");
                }
                /*每个Role都有一个Permissions属性
                  Roles.SelectMany(r => r.Permissions)就是遍历Roles的每一个Role
                  然后把每个Role的Permissions放到一个集合中*/
                var permissions = user.Roles.SelectMany(r => r.Permissions);
                //有任何一个权限匹配，就返回true
                return permissions.Any(p => p.PermissionName == permissionName);
            }
        }

        public async Task Delete(long userId)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var user = await commonService.GetById(userId);
                if (user==null)
                {
                    throw new ArgumentException("用户不存在！");
                }
                db.User.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        private UserDTO ToDTO(User user)
        {
            UserDTO dto = new UserDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNum=user.PhoneNum
            };
            return dto;
        }
    }
}
