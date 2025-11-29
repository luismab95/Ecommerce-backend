namespace Ecommerce.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public int CategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; } = 0;
    public int Stock { get; private set; } = 0;
    public List<Image> Images { get; private set; } = new();
    public bool Featured { get; private set; } = false;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime UpdatedAt { get; private set; }

    public virtual Category? Category { get; set; }

    private Product() { }

    public static Product Create(int categoryId, string name, string description, decimal price, int stock, bool featured)
    {
        return new Product
        {
            CategoryId = categoryId,
            Name = name,
            Description = description,
            Stock = stock,
            Price = price,
            Featured = featured,
            IsActive = true,
        };
    }

    public static Product Update(Product product, string name, string description, decimal price, int stock, bool featured,int categoryId)
    {
        product.Name = name;
        product.Description = description;
        product.Price = price;
        product.Stock = stock;
        product.Featured = featured;
        product.CategoryId = categoryId;
        return product;
    }

    public static Product Delete(Product product)
    {
        product.IsActive = false;
        return product;
    }

    public static object ToSafeResponse(Product product)
    {
        return new
        {
            product.Id,
            product.CategoryId,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.Featured,
            CreatedAt = product.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = product.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Images = product.Images.Select(img => Image.ToSafeResponse(img)),
            Category = product.Category != null ? Category.ToSafeResponse(product.Category) : null
        };
    }
}