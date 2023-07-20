using AutoMapper;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;

namespace DatingAppAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, MemberDto>()
            .ForMember(dest => dest.MainPhotoUrl,
                opt => opt.MapFrom(src => src.Photos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age, 
                opt => opt.MapFrom(src => src.BirthDate.CalculateAge()));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, User>();
        CreateMap<RegisterDto, User>();
        CreateMap<Message, MessageDto>()
            .ForMember(d => d.SenderPhotoUrl, 
                o => o.MapFrom(s => s.Sender.Photos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(d => d.RecipientPhotoUrl, 
                o => o.MapFrom(s => s.Recipient.Photos
                    .FirstOrDefault(p => p.IsMain).Url));
    }
}