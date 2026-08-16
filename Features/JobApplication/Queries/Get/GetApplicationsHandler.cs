using Microsoft.EntityFrameworkCore;

public class GetApplicationsHandler(AppDbContext context) 
{
    public async Task<List<ApplicationResponse>> Handle(CancellationToken cancellationToken)
    {
        var applications = await context.JobApplications
            .Select(j => new ApplicationResponse(
                j.JobApplicationId,
                j.Position,
                j.Status.ToString(),
                j.ApplicationDate,
                j.Company.Name
            ))
            .ToListAsync(cancellationToken);

        return applications;
    }
}