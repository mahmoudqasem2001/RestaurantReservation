using Microsoft.EntityFrameworkCore;
using RestaurantReservation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OrderRepository : IRepository<Order>
{
    private readonly RestaurantReservationDbContext _context;

    public OrderRepository(RestaurantReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
    {
        return await _context.Orders
                             .Where(o => o.ReservationId == reservationId)
                             .Include(o => o.OrderItems)
                             .ThenInclude(oi => oi.MenuItem)
                             .ToListAsync();
    }

    public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
    {
        return await _context.Orders
                             .Where(o => o.EmployeeId == employeeId)
                             .AverageAsync(o => o.TotalAmount);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
