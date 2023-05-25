using BackWI.Models;

namespace BackWI.Services
{
    public interface IJwtProvider
    {
        string CreateToken(Users user);
    }
}