using FluentValidation;

public static class JobApplicationEndpoints
{
    public static void MapApplicationEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/applications").WithTags("Job Applications");

        // POST /api/applications
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

            var result = await handler.Handle(command, ct);
            return result.IsSuccess 
                ? Results.Ok(new { Id = result.Value }) 
                : Results.BadRequest(result.Error);
        });

        // GET /api/applications/{id}
        group.MapGet("/{id:int}", async (
            int id,
            GetApplicationByIdHandler handler,
            CancellationToken ct) =>
        {
            var result = await handler.Handle(new GetApplicationByIdQuery(id), ct);
            return result.IsSuccess 
                ? Results.Ok(result.Value) 
                : Results.NotFound(result.Error);
        });

        // GET /api/applications
        group.MapGet("/", async (
            GetApplicationsHandler handler,
            CancellationToken ct) =>
        {
            var applications = await handler.Handle(ct);
            return Results.Ok(applications);
        });

        // PUT /api/applications/{id}
        group.MapPut("/{id:int}", async (
            int id,
            UpdateApplicationCommand command,
            UpdateApplicationHandler handler,
            IValidator<UpdateApplicationCommand> validator,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var result = await handler.Handle(id, command, ct);
            return result.IsSuccess 
                ? Results.NoContent() 
                : Results.NotFound(result.Error);
        });

        // DELETE /api/applications/{id}
        group.MapDelete("/{id:int}", async (
            int id, 
            ApplicationDeleteHandler handler, 
            CancellationToken ct) =>
        {
            await handler.Handle(new ApplicationDeleteCommand(id), ct);
            return Results.NoContent();
        });
    }
}