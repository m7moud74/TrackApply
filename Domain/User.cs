public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }=default!;
    public string Email { get; set; } = default!;
    public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}