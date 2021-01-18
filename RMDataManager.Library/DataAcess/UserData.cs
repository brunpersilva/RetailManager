using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Models;
using RMDataManager.Library.Internal.DataAcess;

namespace RMDataManager.Library.DataAcess
{
    public class UserData
    {
        public List<UserModel> GetUSerById(string Id)
        {
            SqlDataAcess sql = new SqlDataAcess();
            var p = new { Id = Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "RMData");

            return output;
        }
    }
}
