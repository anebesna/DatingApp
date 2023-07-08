using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;

namespace DatingAppAPI.Interfaces;

public interface IUserRepository
{
    void Update(User user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByUsername(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto> GetMemberAsync(string username);
}