using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extentions;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.DeleteBreed;

public class DeleteBreedHandler(
    ISpeciesRepository specieRepository,
    IValidator<DeleteBreedCommand> validator,
    ILogger<CreateSpecieHandler> logger)
    : ICommandHandler<Guid, DeleteBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var specieResult = await specieRepository.GetById(command.SpecieId, cancellationToken);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();

        var breedResult = specieResult.Value.GetBreed(command.BreedId);
        if (breedResult.IsFailure)
            return breedResult.Error.ToErrorList();
     
        specieResult.Value.RemoveBreed(breedResult.Value);
        
        await specieRepository.Save(specieResult.Value, cancellationToken);

        logger.LogInformation("Delete breed with id: {BreedId}", breedResult.Value.Id.Value);

        return breedResult.Value.Id.Value;
    }
    
}