using HC.Core.Models;

namespace HC.Core.Admin.Interfaces
{
    public interface IAccount
    {
        Task<string> RegisterUserAsync(RegisterUserModel registerUserModel);
        Task<string> LoginAsync(LoginModel model);
    }
}
