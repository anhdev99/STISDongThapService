using Shared;

namespace Core.Entities;

public class ProjectLeader : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Degree { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
}