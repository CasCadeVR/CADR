using CADR.Adrs.Services.Contracts.Models.Comments;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateAdrCommentModel"/>
/// </summary>
public class CreateAdrCommentModelValidator : AbstractValidator<CreateAdrCommentModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrCommentModelValidator"/>
    /// </summary>
    public CreateAdrCommentModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AdrId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty();
    }
}
