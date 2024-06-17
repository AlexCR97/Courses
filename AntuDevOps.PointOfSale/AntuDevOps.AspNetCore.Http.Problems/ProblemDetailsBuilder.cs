namespace AntuDevOps.AspNetCore.Http.Problems;

public class ProblemDetailsBuilder
{
    private string? Type { get; set; }
    private string? Title { get; set; }
    private int? Status { get; set; }
    private string? Detail { get; set; }
    private string? Instance { get; set; }
    private Dictionary<string, object?> Extensions { get; set; } = new(StringComparer.Ordinal);

    public ProblemDetailsBuilder WithType(string? type)
    {
        Type = type;
        return this;
    }

    public ProblemDetailsBuilder WithTitle(string? title)
    {
        Title = title;
        return this;
    }

    public ProblemDetailsBuilder WithStatus(int? status)
    {
        Status = status;
        return this;
    }

    public ProblemDetailsBuilder WithDetail(string? detail)
    {
        Detail = detail;
        return this;
    }

    public ProblemDetailsBuilder WithInstance(string? instance)
    {
        Instance = instance;
        return this;
    }

    public ProblemDetailsBuilder WithExtensions(Dictionary<string, object?> extensions)
    {
        Extensions = extensions;
        return this;
    }

    public ProblemDetailsBuilder WithExtension(string key, object? value)
    {
        Extensions[key] = value;
        return this;
    }

    public ProblemDetails Build()
    {
        return new ProblemDetails
        {
            Type = Type,
            Title = Title,
            Status = Status,
            Detail = Detail,
            Instance = Instance,
            Extensions = Extensions.ToDictionary(),
        };
    }
}
