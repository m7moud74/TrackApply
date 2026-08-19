using Microsoft.EntityFrameworkCore;

public class UpdateApplicationHandler(AppDbContext context,ICacheService cache)
{
    public async Task<Result<bool>> Handle(int id, UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await context.JobApplications
            .FirstOrDefaultAsync(j => j.JobApplicationId == id, cancellationToken);

        if (application is null)
        {
            return Result<bool>.Failure(new Error("JobApplication.NotFound", $"Job application with Id {id} was not found."));
        }

        if (!string.IsNullOrWhiteSpace(request.Position))
        {
            application.Position = request.Position;
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            application.Status = Enum.Parse<ApplicationStatus>(request.Status, ignoreCase: true);
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync("JobApplicationList",cancellationToken);
        return Result<bool>.Success(true);
    }
}