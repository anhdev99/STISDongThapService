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
public class GetUserDto : IMapFrom<User>
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserDto>();
    }
}

public class GetAllUsersDto : IMapFrom<User>
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetAllUsersDto>();
    }
}

public class GetUserWithPaginationDto : IMapFrom<User>
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserWithPaginationDto>();
        FullName = $"{FirstName} {LastName}";
    }
}