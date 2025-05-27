using FluentValidation;
using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UploadPetPhoto;

public class UploadPetPhotosCommandValidator : AbstractValidator<UploadPetPhotosCommand>
{
    public UploadPetPhotosCommandValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Photos).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(c => c.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Content).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}