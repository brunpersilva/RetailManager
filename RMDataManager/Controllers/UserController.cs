using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RMDataManager.Library.DataAcess;
using RMDataManager.Library.Models;
using RMDataManager.Models;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();

            return data.GetUSerById(userId).First();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                List<ApplicationUserModel> output = users.Select(x => new ApplicationUserModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Roles = x.Roles.Join(roles, s1 => s1.RoleId, s2 => s2.Id,
                    (s1, s2) => new { a = s1, b = s2 }).ToDictionary(z => z.a.RoleId, z => z.b.Name)
                }).ToList();
                return output;
               
            }
            

        }
    }
}
