using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class UserDto: IMapFrom<User>
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public int Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>();
    }
}