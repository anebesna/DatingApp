using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;

namespace DatingAppAPI.Interfaces;

public interface IPhotoRepository
{
    public Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
    public Task<Photo> GetPhotoById(int id);
    public void RemovePhoto(Photo photo);

}
