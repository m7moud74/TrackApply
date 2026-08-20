using Microsoft.EntityFrameworkCore;

public class GetApplicationByIdHandler(AppDbContext context)
{
    public async Task<Result<ApplicationResponse>> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
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
            return Result<ApplicationResponse>.Failure(new Error("JobApplication.NotFound", $"Job application with Id {request.JobApplicationId} was not found."));
        }

        return Result<ApplicationResponse>.Success(application);
    }
}