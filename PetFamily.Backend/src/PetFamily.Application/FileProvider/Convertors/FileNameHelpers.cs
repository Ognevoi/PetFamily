namespace PetFamily.Application.FileProvider.Convertors;

public static class FileNameHelpers
{
    public static string GetRandomizedFileName(string fileName)
    {
        return Guid.NewGuid().ToString() + Path.GetExtension(fileName);
    }
}