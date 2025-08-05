using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class LeaveTypeRepository : RepositoryDb<int, LeaveTypeMaster>
    {
        public LeaveTypeRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<LeaveTypeMaster>> GetAllValue()
        {
            return await _context.LeaveTypeMasters.ToListAsync();
        }

        public override async Task<LeaveTypeMaster> GetValueById(int key)
        {
            var item = await _context.LeaveTypeMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
