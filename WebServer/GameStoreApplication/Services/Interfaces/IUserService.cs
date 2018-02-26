namespace WebServer.GameStoreApplication.Services.Interfaces
{
    public interface IUserService
    {
        bool Find(string email, string password);

        bool Create(string email, string name, string password);

        bool IsAdmin(string email);
    }
}
