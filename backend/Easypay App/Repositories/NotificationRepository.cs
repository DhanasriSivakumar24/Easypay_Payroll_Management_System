using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class NotificationRepository : RepositoryDb<int, NotificationLog>
    {
        public NotificationRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<NotificationLog>> GetAllValue()
        {
            return await _context.NotificationLogs.ToListAsync();
        }

        public override async Task<NotificationLog> GetValueById(int key)
        {
            var item = await _context.NotificationLogs.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
