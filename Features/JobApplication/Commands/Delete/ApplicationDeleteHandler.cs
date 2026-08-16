using Microsoft.EntityFrameworkCore;

public class ApplicationDeleteHandler(AppDbContext context)
{
    public async Task  Handle(ApplicationDeleteCommand request,CancellationToken cancellationToken)
    {
        await context.JobApplications
            .Where(x => x.JobApplicationId == request.JobApplicatoinid)
            .ExecuteDeleteAsync(cancellationToken);
    }
}