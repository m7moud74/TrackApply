public class JobApplication
{
    public int  JobApplicationId { get; set; }
    public string Position { get; set; } = default!;
    public ApplicationStatus Status { get; set; } = default!;
    public DateTime ApplicationDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;
}