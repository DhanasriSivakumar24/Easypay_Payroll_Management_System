using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class LeaveRequestRepository : RepositoryDb<int, LeaveRequest>
    {
        public LeaveRequestRepository(PayrollContext context):base(context)
        {
            
        }
        public override async Task<IEnumerable<LeaveRequest>> GetAllValue()
        {
            return await _context.LeaveRequests.ToListAsync();
        }
        public override async Task<LeaveRequest> GetValueById(int key)
        {
            var result = await _context.LeaveRequests.FirstOrDefaultAsync(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
