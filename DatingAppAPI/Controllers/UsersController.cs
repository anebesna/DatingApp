using System.Security.Claims;
using AutoMapper;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;
using DatingAppAPI.Helpers;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers;
[Authorize]
public class UsersController : BaseApiController 
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly IPhotoService _photoService;

    public UsersController(IMapper mapper, IUnitOfWork uow, IPhotoService photoService)
    {
        _mapper = mapper;
        _uow = uow;
        _photoService = photoService;
    }
    
    [HttpGet]  
    public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
    {
        var gender = await _uow.UserRepository.GetUserGender(User.GetUsername());
        userParams.CurrentUsername = User.GetUsername();
        if (string.IsNullOrEmpty(userParams.Gender))
        {
            userParams.Gender = gender == "male" ? "female" : "male";
        }
        var users = await _uow.UserRepository.GetMembersAsync(userParams);
        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return await _uow.UserRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return NotFound();
        _mapper.Map(memberUpdateDto, user);
        if (await _uow.Complete()) return NoContent();
        return BadRequest("Failed to update the user.");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return NotFound();
        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        if (user.Photos.Count == 0) photo.IsMain = true;
        user.Photos.Add(photo);
        if (await _uow.Complete())
        {
            return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
        }
        return BadRequest("Failed adding photo.");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return NotFound();
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null) return NotFound();
        if (photo.IsMain == true) return BadRequest("This is already your main photo.");
        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if (await _uow.Complete()) return NoContent();
        return BadRequest("Failed setting the main photo.");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return NotFound();
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null) return NotFound();
        if (photo.IsMain) return BadRequest("You cannot delete your main photo.");
        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);
        if (await _uow.Complete()) return Ok(); 
        return BadRequest("Failed deleting the photo.");
    }
}