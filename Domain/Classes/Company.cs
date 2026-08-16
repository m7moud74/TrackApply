public class Company
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = default!;
    public List<Interview> Interviews { get; set; } = new List<Interview>();
    public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}