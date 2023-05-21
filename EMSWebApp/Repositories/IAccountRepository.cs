using EMSWebApp.Models;
using EMSWebApp.ViewModel;

namespace EMSWebApp.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> SignUpUserAsync(RegisterUserViewModel user);
        Task<string> SignInUserAsync(LoginUserViewModel loginUserViewModel);
        Task<string> GetApplicationUserId(string token);
    }
}
