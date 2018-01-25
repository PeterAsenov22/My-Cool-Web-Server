namespace WebServer.ByTheCakeApplication.Services.Interfaces
{
    using ViewModels.Account;

    public interface IUserService
    {
        bool Create(string username, string password);

        bool Find(string username, string password);

        ProfileViewModel Profile(string username);
    }
}
