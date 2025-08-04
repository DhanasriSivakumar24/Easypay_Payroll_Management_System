using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;

namespace Easypay_App.Repositories
{
    public class NotificationChannelRepository : RepositoryDb<int, NotificationChannelMaster>
    {
        public NotificationChannelRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<NotificationChannelMaster> GetAllValue()
        {
            return _context.NotificationChannelMasters.ToList();
        }

        public override NotificationChannelMaster GetValueById(int key)
        {
            var item = _context.NotificationChannelMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
