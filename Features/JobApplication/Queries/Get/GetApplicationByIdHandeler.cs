using Microsoft.EntityFrameworkCore;

public class GetApplicationByIdHandeler(AppDbContext context)
{
    public async Task<ApplicationResponse> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var application = await context.JobApplications
            .Where(j => j.JobApplicationId == request.JobApplicationId)
            .Select(j => new ApplicationResponse(
                j.JobApplicationId,
                j.Position,
                j.Status.ToString(),
                j.ApplicationDate,
                j.Company.Name
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (application == null)
        {
            throw new Exception($"Job application with Id {request.JobApplicationId} was not found.");
        }

        return application;
    }
}