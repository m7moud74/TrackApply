using Microsoft.EntityFrameworkCore;

public class CreateApplicationHandler(AppDbContext context)
{

    public async Task<Result<int>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {

        var userExists = await context.Users
            .AnyAsync(u => u.UserId == request.UserId, cancellationToken);

            var companyExists = await context.Companies
            .AnyAsync(c => c.CompanyId == request.CompanyId, cancellationToken);

        if (!companyExists)
        {
            return Result<int>.Failure (new Error("Company  Not Found",$"JobApplication with {request.CompanyId} Not Found"));
        }

        if (!userExists)
        {
            return Result<int>.Failure(new Error( "UserId Not Found",$"User with Id {request.UserId} was not found."));
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

        return Result<int>.Success(jobApplication.JobApplicationId);
    }
}