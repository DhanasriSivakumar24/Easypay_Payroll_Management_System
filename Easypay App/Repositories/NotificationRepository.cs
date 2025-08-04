using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class NotificationRepository : RepositoryDb<int, NotificationLog>
    {
        public NotificationRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<NotificationLog> GetAllValue()
        {
            return _context.NotificationLogs.ToList();
        }

        public override NotificationLog GetValueById(int key)
        {
            var item = _context.NotificationLogs.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
