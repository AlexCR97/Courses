using FluentValidation;

namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly struct Password : IEquatable<Password>
{
    public Password()
    {
        Value = string.Empty;
        new Validator().ValidateAndThrow(this);
    }

    public Password(string value)
    {
        Value = value;
        new Validator().ValidateAndThrow(this);
    }
    
    public string Value { get; }

    public override bool Equals(object? obj) => obj is Password other && this.Equals(other);

    public bool Equals(Password other) => string.Equals(this.Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Password left, Password right) => left.Equals(right);

    public static bool operator !=(Password left, Password right) => !left.Equals(right);

    private class Validator : AbstractValidator<Password>
    {
        public Validator()
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(48)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
        }
    }
}
