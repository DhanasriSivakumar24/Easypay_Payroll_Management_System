using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class LeaveRequestRepository : RepositoryDb<int, LeaveRequest>
    {
        public LeaveRequestRepository(PayrollContext context):base(context)
        {
            
        }
        public override IEnumerable<LeaveRequest> GetAllValue()
        {
            return _context.LeaveRequests.ToList();
        }
        public override LeaveRequest GetValueById(int key)
        {
            var result = _context.LeaveRequests.FirstOrDefault(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
