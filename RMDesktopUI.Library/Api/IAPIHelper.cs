using System.Net.Http;
using System.Threading.Tasks;
using RMDesktopUi.Library.Models;

namespace RMDesktopUi.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        void LogOffUser();
        Task<AuthenticatedUser> Authenticate(string userName, string password);
        Task GetLoggedUserInfo(string token);
    }
}