using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAcess
{
    public interface IUserData
    {
        void CreateUser(UserModel user);
        List<UserModel> GetUSerById(string Id);
    }
}