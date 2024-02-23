namespace Chirp.Core;
using FluentValidation;

public record CheepDto(string Text, string AuthorName, DateTime TimeStamp, Guid CheepId);
public record CreateCheepDto(string Text, string AuthorName);

public class CreateCheepValidator : AbstractValidator<CreateCheepDto>
{
    public CreateCheepValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(160);
    }
}
