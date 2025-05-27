using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.GetPetPhoto;

public class GetPetPhotosHandler : ICommandHandler<IEnumerable<string>, GetPetPhotosCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFilesProvider _filesProvider;
    private readonly IValidator<GetPetPhotosCommand> _validator;
    private readonly ILogger<GetPetPhotosHandler> _logger;

    public GetPetPhotosHandler(
        IVolunteersRepository volunteersRepository,
        IFilesProvider filesProvider,
        IValidator<GetPetPhotosCommand> validator,
        ILogger<GetPetPhotosHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _filesProvider = filesProvider;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> HandleAsync(
        GetPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var photosToGet = petResult.Value.Photos.Select(x => x.FileName);

        var getResult = await _filesProvider.GetFileLink(
            photosToGet,
            Constants.BUCKET_NAME,
            cancellationToken);

        _logger.LogInformation("Photos for pet with id {PetId} retrieved", command.PetId);

        return getResult.Value.ToList();
    }
}