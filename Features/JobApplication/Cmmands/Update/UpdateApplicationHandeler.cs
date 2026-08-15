using Microsoft.EntityFrameworkCore;

public class UpdateApplicationHandeler(AppDbContext context)
{
    public async Task<bool> Handel(int id,UpdateApplicationCommand request,CancellationToken cancellationToken)
    {
        var application = await context.JobApplications
            .FirstOrDefaultAsync(j => j.JobApplicationId == id, cancellationToken);
        if(application is null)  
        {
            throw new Exception($"JobApplicatoin whiht{id} Not Fpund ");
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
        return true;
    }
}