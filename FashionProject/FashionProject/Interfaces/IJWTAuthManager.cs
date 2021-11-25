using FashionProject.Models;

namespace FashionProject.Interfaces
{
    public interface IJWTAuthManager
    {
        string Authenticate(User user);
    }
}
