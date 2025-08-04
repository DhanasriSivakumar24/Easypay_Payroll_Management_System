using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;

namespace Easypay_App.Repositories
{
    public class NotificationStatusRepository : RepositoryDb<int, NotificationStatusMaster>
    {
        public NotificationStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<NotificationStatusMaster> GetAllValue()
        {
            return _context.NotificationStatusMasters.ToList();
        }

        public override NotificationStatusMaster GetValueById(int key)
        {
            var item = _context.NotificationStatusMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
