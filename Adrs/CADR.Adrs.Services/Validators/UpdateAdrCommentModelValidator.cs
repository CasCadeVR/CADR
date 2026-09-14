using CADR.Adrs.Services.Contracts.Models.Comments;
using FluentValidation;

namespace CADR.Adrs.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateAdrCommentModel"/>
/// </summary>
public class UpdateAdrCommentModelValidator : AbstractValidator<UpdateAdrCommentModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrCommentModelValidator"/>
    /// </summary>
    public UpdateAdrCommentModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Text).NotEmpty();
    }
}
