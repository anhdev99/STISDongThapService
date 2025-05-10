using Shared;

namespace Core.Entities;

public class Project : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Content { get; set; }
    public string Target { get; set; }
    public int SectorId { get; set; }
    public Sector Sector { get; set; }
    public int? StatusId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public int? ManagementLevelId { get; set; }
    public ManagementLevel ManagementLevel { get; set; }
    public int? TaskTypeId { get; set; }
    public TaskType TaskType { get; set; }
    public int? ProjectLeaderId { get; set; }
    public ProjectLeader ProjectLeader { get; set; }
    public int? HostOrganizationId { get; set; }
    public HostOrganization HostOrganization { get; set; }
    public int? GoverningAgencyId { get; set; }
    public GoverningAgency GoverningAgency { get; set; }
    public int? RankId { get; set; }
    public Rank Rank { get; set; }
}