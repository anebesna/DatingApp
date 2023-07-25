using DatingAppAPI.Entities;

namespace DatingAppAPI.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(User user);
}