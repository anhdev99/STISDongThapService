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
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }  
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsVerified { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")); // Kết hợp FirstName và LastName để tạo FullName
    }
}

public class GetAllUsersDto : IMapFrom<User>
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public string FullName { get; set; } // Giữ lại FullName thay vì FirstName và LastName
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsVerified { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetAllUsersDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")); // Kết hợp FirstName và LastName thành FullName
    }
}


public class GetUserWithPaginationDto : IMapFrom<User>
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsVerified { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserWithPaginationDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
