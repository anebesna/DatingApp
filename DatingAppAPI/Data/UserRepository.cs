using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;
using DatingAppAPI.Helpers;
using DatingAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public void Update(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(n => n.UserName == username);
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = _context.Users.AsQueryable();
        
        query = query.Where(u => u.UserName != userParams.CurrentUsername);
        query = query.Where(u => u.Gender == userParams.Gender);
       
        var minBD = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
        
        query = query.Where(u => u.BirthDate >= minBD && u.BirthDate <= maxDB);
        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(u => u.Created),
            _ => query.OrderByDescending(u => u.LastActive)
        };
        
        return await PagedList<MemberDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), 
            userParams.PageNumber, 
            userParams.PageSize);

    }
    public async Task<MemberDto> GetMemberAsync(string username, bool isCurrentUser)
    {
        var query = _context.Users
            .Where(u => u.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        if (isCurrentUser) query = query.IgnoreQueryFilters();
       
        return await query.FirstOrDefaultAsync();
    }

    public async Task<string> GetUserGender(string username)
    {
        return await _context.Users
            .Where(x => x.UserName == username)
            .Select(x => x.Gender).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByPhotoId(int photoId)
    {
        return await _context.Users
            .Include(u => u.Photos)
            .IgnoreQueryFilters()
            .Where(u => u.Photos.Any(p => p.Id == photoId))
            .FirstOrDefaultAsync();
    }
}