using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAcess
{
    public interface IUserData
    {
        List<UserModel> GetUSerById(string Id);
    }
}