using Microsoft.EntityFrameworkCore;

public class ApplicationDeleteHandler(AppDbContext context,ICacheService cache)
{
    public async Task  Handle(ApplicationDeleteCommand request,CancellationToken cancellationToken)
    {
        var efectedrow = await context.JobApplications
            .Where(x => x.JobApplicationId == request.JobApplicatoinid)
            .ExecuteDeleteAsync(cancellationToken);
        if (efectedrow > 0)
            await cache.RemoveAsync("JobApplicationList");
    }
}