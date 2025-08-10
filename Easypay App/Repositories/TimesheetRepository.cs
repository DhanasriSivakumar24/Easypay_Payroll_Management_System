using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class TimesheetRepository : RepositoryDb<int, Timesheet>
    {
        public TimesheetRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Timesheet>> GetAllValue()
        {
            return _context.Timesheets.ToList();
        }

        public override async Task<Timesheet> GetValueById(int key)
        {
            var result = await _context.Timesheets.FirstOrDefaultAsync(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
