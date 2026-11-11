namespace Application.Common.Results;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new("", "");

    public static readonly Error NotFound = new("NotFound", "Resource was not found.");

    public static readonly Error Validation = new("Validation", "Validation failed.");

    public static readonly Error Conflict = new("Conflict", "Resource conflict.");
}
