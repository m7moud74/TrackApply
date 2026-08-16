public class Interview
{
    public int InterviewId { get; set; }
    public DateTime InterviewDate { get; set; }
    public string Interviewer { get; set; } = default!;
    public string Feedback { get; set; } = default!;
    public int JobApplicationId { get; set; }
    public JobApplication JobApplication { get; set; } = default!;
}