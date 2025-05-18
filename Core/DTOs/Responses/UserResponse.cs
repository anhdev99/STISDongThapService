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
    public List<GetRoleDto>? Roles { get; set; }
    public string DepartmentName { get; set; }  // sửa thành string

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, GetUserWithPaginationDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => new GetRoleDto
            {
                Id = ur.Role.Id,
                Code = ur.Role.Code,
                Name = ur.Role.Name,
                DisplayName = ur.Role.DisplayName,
                Description = ur.Role.Description,
                Color = ur.Role.Color,
            })))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));
    }
}
