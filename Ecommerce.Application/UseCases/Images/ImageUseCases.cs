namespace Ecommerce.Application.UseCases.Images;

using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Image = Ecommerce.Domain.Entities.Image;

public class ImageUseCases(IImageRepository imageRepository, IUploadImageService uploadImageService)
{
    private readonly IImageRepository _imageRepository = imageRepository;
    private readonly IUploadImageService _uploadImageService = uploadImageService;


    public async Task<string> AddImagesAsync(IFormFileCollection request)
    {

        var images = await _uploadImageService.AddImageAsync(request);

        foreach (var item in images)
        {
            var newImage = Image.Create(item, 1);
            await _imageRepository.AddAsync(newImage);
        }

        return "Imágenes agregadas exitosamente.";
    }

    public async Task<string> DeleteImageAsync(int imageId)
    {
        var findImage = await _imageRepository.GetByIdAsync(imageId) ??
            throw new InvalidOperationException("Imágen no encontrada.");

        var updateImage = Image.Delete(findImage);
        await _imageRepository.UpdateAsync(updateImage);

        return "Imagen eliminada exitosamente.";

    }


    public string GetContentType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };
    }

}
