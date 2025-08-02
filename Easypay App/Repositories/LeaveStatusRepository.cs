using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class LeaveStatusRepository : RepositoryDb<int, LeaveStatusMaster>
    {
        public LeaveStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<LeaveStatusMaster> GetAllValue()
        {
            return _context.LeaveStatusMasters.ToList();
        }

        public override LeaveStatusMaster GetValueById(int key)
        {
            var item = _context.LeaveStatusMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
