using RMDataManager.Models;
using System.Threading.Tasks;

namespace RMDesktopUi.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string userName, string password);
    }
}