using Microsoft.EntityFrameworkCore;
using RestaurantReservation;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MenuItemRepository : IRepository<MenuItem>
{
    private readonly RestaurantReservationDbContext _context;

    public MenuItemRepository(RestaurantReservationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems.ToListAsync();
    }

    public async Task<MenuItem> GetByIdAsync(int id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task CreateAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
