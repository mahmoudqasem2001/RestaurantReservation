using Microsoft.EntityFrameworkCore;
using RestaurantReservation;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TableRepository : IRepository<Table>
{
    private readonly RestaurantReservationDbContext _context;

    public TableRepository(RestaurantReservationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Table>> GetAllAsync()
    {
        return await _context.Tables.ToListAsync();
    }

    public async Task<Table> GetByIdAsync(int id)
    {
        return await _context.Tables.FindAsync(id);
    }

    public async Task CreateAsync(Table table)
    {
        await _context.Tables.AddAsync(table);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Table table)
    {
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table != null)
        {
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
        }
    }
}
