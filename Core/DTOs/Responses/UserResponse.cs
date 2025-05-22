using AutoMapper;
using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class UserDto: IMapFrom<User>
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public int Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>().ForMember(x => x.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
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
    public int DepartmentId { get; set; }

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

    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsVerified { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetAllUsersDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
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
    public List<BaseRole>? Roles { get; set; }
    public string DepartmentName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserWithPaginationDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => new BaseRole
            {
                Code = ur.Role.Code,
                DisplayName = ur.Role.DisplayName,
                Color = ur.Role.Color,
            })))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));
    }
}

public class GetMeDto : IMapFrom<User>
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string? DepartmentCode { get; set; }
    public string? DepartmentName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetMeDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"))
            .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department!.Code))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department!.Name));
    }
}
