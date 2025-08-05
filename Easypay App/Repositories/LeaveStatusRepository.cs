using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class LeaveStatusRepository : RepositoryDb<int, LeaveStatusMaster>
    {
        public LeaveStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<LeaveStatusMaster>> GetAllValue()
        {
            return await _context.LeaveStatusMasters.ToListAsync();
        }

        public override async Task<LeaveStatusMaster> GetValueById(int key)
        {
            var item = await _context.LeaveStatusMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
