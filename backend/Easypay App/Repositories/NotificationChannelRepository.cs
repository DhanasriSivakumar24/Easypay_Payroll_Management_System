using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class NotificationChannelRepository : RepositoryDb<int, NotificationChannelMaster>
    {
        public NotificationChannelRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<NotificationChannelMaster>> GetAllValue()
        {
            return await _context.NotificationChannelMasters.ToListAsync();
        }

        public override async Task<NotificationChannelMaster> GetValueById(int key)
        {
            var item = await _context.NotificationChannelMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
