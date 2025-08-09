using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class AttendanceRepository : RepositoryDb<int, Attendance>
    {
        public AttendanceRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Attendance>> GetAllValue()
        {
            return _context.Attendances.ToList();
        }

        public override async Task<Attendance> GetValueById(int key)
        {
            var result = await _context.Attendances.FirstOrDefaultAsync(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
