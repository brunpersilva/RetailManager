using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Models;
using Microsoft.Extensions.Configuration;

namespace RMDataManager.Library.DataAcess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAcess _sql;

        public UserData(ISqlDataAcess sql)
        {
            _sql = sql;
        }
        public List<UserModel> GetUSerById(string Id)
        {
            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id }, "RMData");

            return output;
        }
    }
}
