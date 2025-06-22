using Microsoft.EntityFrameworkCore;
using server_dotnet_dal.Context;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet_dal.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.User)
            .Include(o => o.Organization)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
