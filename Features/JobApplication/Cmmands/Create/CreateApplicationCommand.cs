public record CreateApplicationCommand(
    int CompanyId,
    int UserId,
    string Position
);