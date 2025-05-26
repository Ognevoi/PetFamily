using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.DeleteSpecie;

public class DeleteSpecieHandler(
    ISpeciesRepository specieRepository,
    ILogger<CreateSpecieHandler> logger)
    : ICommandHandler<Guid, DeleteSpecieCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var specieResult = await specieRepository.GetById(command.SpecieId);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();
        
        await specieRepository.Delete(specieResult.Value, cancellationToken);
        
        await specieRepository.Save(specieResult.Value, cancellationToken);

        logger.LogInformation("Delete specie with id: {SpecieId}", specieResult.Value.Id);

        return specieResult.Value.Id.Value;
    }
    
}