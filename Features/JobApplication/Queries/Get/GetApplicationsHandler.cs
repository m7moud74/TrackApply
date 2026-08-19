using Microsoft.EntityFrameworkCore;

public class GetApplicationsHandler(AppDbContext context ,ICacheService cache)
{
    private const string key = "JobApplicationList";
    public async Task<Result<List<ApplicationResponse>>> Handle(CancellationToken cancellationToken)
    {
        var casheapplication = await cache.GetTAsync<List<ApplicationResponse>>(key, cancellationToken);
        if(casheapplication is not null)
        {
            return Result<List<ApplicationResponse>>.Success(casheapplication);
        }
        var applications = await context.JobApplications
            .Select(j => new ApplicationResponse(
                j.JobApplicationId,
                j.Position,
                j.Status.ToString(),
                j.ApplicationDate,
                j.Company.Name
            ))
            .ToListAsync(cancellationToken);
        await cache.SetTAsync(applications,key,TimeSpan.FromMinutes(10),cancellationToken);

        return Result<List<ApplicationResponse>>.Success(applications);
    }
}