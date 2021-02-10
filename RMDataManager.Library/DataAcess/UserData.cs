using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Models;
using RMDataManager.Library.Internal.DataAcess;
using Microsoft.Extensions.Configuration;

namespace RMDataManager.Library.DataAcess
{
    public class UserData
    {
        private readonly IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }
        public List<UserModel> GetUSerById(string Id)
        {
            SqlDataAcess sql = new SqlDataAcess(_config);
            var p = new { Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "RMData");

            return output;
        }
    }
}
