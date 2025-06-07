using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.Commands.DTO.Validators;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(x => x.Content).Must(c => c.Length < 5000000).WithError(Errors.General.FileSizeLimitExceeded(5000000));
        RuleFor(x => x.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}