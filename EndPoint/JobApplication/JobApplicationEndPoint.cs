using FluentValidation;

public static class JobApplicationEndPoint
{
    public static void MapApplicationEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/applications").WithTags("Job Applications");

        group.MapPost("/", async (
            CreateApplicationCommand command,
            CreateApplicationHandler handler,
            IValidator<CreateApplicationCommand> validator,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var applicationId = await handler.Handle(command, ct);
            return Results.Ok(new { Id = applicationId });
        });
        group.MapGet("/{id:int}", async (
            int id,
            GetApplicationByIdHandeler handler,
            CancellationToken ct) =>
        {
            var application = await handler.Handle(new GetApplicationByIdQuery(id), ct);
            return Results.Ok(application);
        });
        group.MapGet("/", async (
            GetApplicationsHandeler handler,
            CancellationToken ct) =>
        {
            var applications = await handler.Handle(ct);
            return Results.Ok(applications);
        });
        group.MapPut("/{id:int}", async (
            int id,
            UpdateApplicationHandeler handler,
            UpdateApplicationCommand command,
            IValidator<UpdateApplicationCommand> validator,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var applicationId = await handler.Handel(id, command, ct);
            return Results.NoContent();
        });
        group.MapDelete("/{id:int}", async (int id, AppicationDeleteHandeler handeler, CancellationToken ct) =>
        {
            await handeler.Handle(new ApplicationDeleteCommand(id), ct);
            return Results.NoContent();
        });
        
    }
    
}

    