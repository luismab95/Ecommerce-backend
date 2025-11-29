namespace Ecommerce.Infrastructure.Repositories;

using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{

    private readonly ApplicationDbContext _context = context;
    
    public async Task AddAsync(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
    }

    public async Task<Image?> GetByIdAsync(int imageId)
    {
        return await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId && i.IsActive);
    }

    public async Task UpdateAsync(Image image)
    {
        _context.Images.Update(image);
        await _context.SaveChangesAsync();
    }
}
