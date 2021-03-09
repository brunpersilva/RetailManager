using Portal.Models;
using System.Threading.Tasks;

namespace Portal.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticadedUserModel> Login(AuthenticationUserModel userForAuthentication);
        Task Logout();
    }
}