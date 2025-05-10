using Core.Common.Mappings;
using Core.Entities;

namespace Core.DTOs.Responses;

public class GetTaskTypeWithPagingDto : IMapFrom<TaskType>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
}

public class GetTaskTypeDto : IMapFrom<TaskType>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public string? BackgroundColor { get; set; }
    public string? Color { get; set; }
}

public class TaskTypeSimpleDto : IMapFrom<TaskType>
{
    public int Id { get; set; }
    public string Name { get; set; }
}