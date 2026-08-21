using Microsoft.EntityFrameworkCore;

public class UpdateApplicationHandler(AppDbContext context, ICacheService cache,IMessageProducer producer)
{
    public async Task<Result<bool>> Handle(int id, UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await context.JobApplications.Include(u => u.User)
            .FirstOrDefaultAsync(j => j.JobApplicationId == id, cancellationToken);

        if (application is null)
        {
            return Result<bool>.Failure(new Error("JobApplication.NotFound", $"Job application with Id {id} was not found."));
        }

        if (!string.IsNullOrWhiteSpace(request.Position))
        {
            application.Position = request.Position;
        }
        bool statuscahnged = false;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            application.Status = Enum.Parse<ApplicationStatus>(request.Status, ignoreCase: true);
            statuscahnged = true;
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync("JobApplicationList", cancellationToken);

        if (statuscahnged)
        {
            var @event = new ApplicationStatusChangedEvent(application.UserId,
            application.User.Email, application.Status.ToString());
            await producer.PublishMessage(@event, "EmailChangedStatus",cancellationToken);
        }
        return Result<bool>.Success(true);
    }
}