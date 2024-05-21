using FluentValidation;

namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly struct Email : IEquatable<Email>
{
    public Email()
    {
        OriginalValue = string.Empty;
        new Validator().ValidateAndThrow(this);
    }

    public Email(string value)
    {
        OriginalValue = value;
        new Validator().ValidateAndThrow(this);
    }

    public string OriginalValue { get; }

    public string NormalizedValue => OriginalValue.Trim().ToUpper();

    public override bool Equals(object? obj) => obj is Email other && this.Equals(other);

    public bool Equals(Email other) => string.Equals(this.NormalizedValue, other.NormalizedValue, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => NormalizedValue.GetHashCode();

    public override string ToString() => OriginalValue;

    public static bool operator ==(Email left, Email right) => left.Equals(right);

    public static bool operator !=(Email left, Email right) => !left.Equals(right);

    private class Validator : AbstractValidator<Email>
    {
        public Validator()
        {
            RuleFor(x => x.OriginalValue)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
        }
    }
}
