namespace Ecommerce.Domain.Entities;

public class Image
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public string Path { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Image() { }

    public static Image Create(string path, int productId)
    {
        return new Image
        {
            Path = path,
            ProductId = productId,
            IsActive = true
        };
    }


    public static Image Delete(Image image)
    {
        image.IsActive = false;
        return image;
    }
}
