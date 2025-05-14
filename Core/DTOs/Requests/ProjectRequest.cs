namespace Core.DTOs.Requests;

public record CreateProjectRequest(
    string Name,
    string Content, 
    string Target,
    int SectorId,
    int? StatusId,
    int? ManagementLevelId,
    int? TaskTypeId,
    int? ProjectLeaderId,
    int? HostOrganizationId,
    int? GoverningAgencyId,
    int? RankId
);

public record UpdateProjectRequest(
    int Id,
    string Name,
    string Content,
    string Target, 
    int SectorId,
    int? StatusId,
    int? ManagementLevelId,
    int? TaskTypeId,
    int? ProjectLeaderId,
    int? HostOrganizationId,
    int? GoverningAgencyId,
    int? RankId
);

public record GetProjectsWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Keywords
);