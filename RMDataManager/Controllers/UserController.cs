using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAcess;
using RMDataManager.Library.Models;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public List<UserModel> GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();

            return data.GetUSerById(userId);
        }
    }
}
