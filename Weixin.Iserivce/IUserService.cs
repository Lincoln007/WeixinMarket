using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface IUserService
    {
        Task<long> AddNew(string userName, string phoneNum, string password);
        Task Edit(long userId, string phoneNum, string password);
        Task<UserDTO[]> GetAll();
        Task<UserDTO> GetById(long userId);
        Task<UserDTO> GetByPhoneNum(string phoneNum);
        Task<bool> CheckLogin(String userName, String password);
        Task Delete(long userId);
        bool HasPermission(long userId, String permissionName);
    }
}
