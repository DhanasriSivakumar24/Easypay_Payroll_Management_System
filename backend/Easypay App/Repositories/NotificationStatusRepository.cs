using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class NotificationStatusRepository : RepositoryDb<int, NotificationStatusMaster>
    {
        public NotificationStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<NotificationStatusMaster>> GetAllValue()
        {
            return await _context.NotificationStatusMasters.ToListAsync();
        }

        public override async Task<NotificationStatusMaster> GetValueById(int key)
        {
            var item = await _context.NotificationStatusMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
