using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class TimesheetStatusRepository : RepositoryDb<int, TimesheetStatusMaster>
    {
        public TimesheetStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TimesheetStatusMaster>> GetAllValue()
        {
            return _context.TimesheetStatusMasters.ToList();
        }

        public override async Task<TimesheetStatusMaster> GetValueById(int key)
        {
            var result = await _context.TimesheetStatusMasters.FirstOrDefaultAsync(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
