using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetManagementLevelWithPagingDto : IMapFrom<ManagementLevel>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class GetManagementLevelDto : IMapFrom<ManagementLevel>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class ManagementLevelSimpleDto : IMapFrom<ManagementLevel>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
