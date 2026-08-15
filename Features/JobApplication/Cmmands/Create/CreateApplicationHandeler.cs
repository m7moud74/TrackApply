using Microsoft.EntityFrameworkCore;

public class CreateApplicationHandler(AppDbContext context)
{

    public async Task<int> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {

        var userExists = await context.Users
            .AnyAsync(u => u.UserId == request.UserId, cancellationToken);

            var companyExists = await context.Companies
            .AnyAsync(c => c.CompanyId == request.CompanyId, cancellationToken);

        if (!companyExists)
        {
            throw new Exception($"Company with Id {request.CompanyId} was not found.");
        }

        if (!userExists)
        {
            throw new Exception($"User with Id {request.UserId} was not found.");
        }
        var jobApplication = new JobApplication
        {
            CompanyId = request.CompanyId,
            UserId = request.UserId,
            Position = request.Position,
            Status = ApplicationStatus.Applied,
            ApplicationDate = DateTime.UtcNow
        };

        context.JobApplications.Add(jobApplication);
        await context.SaveChangesAsync(cancellationToken);

        return jobApplication.JobApplicationId;
    }
}