using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class AttendanceMasterRepository : RepositoryDb<int, AttendanceStatusMaster>
    {
        public AttendanceMasterRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<AttendanceStatusMaster>> GetAllValue()
        {
             return _context.AttendanceStatusMasters.ToList();
        }

        public override async Task<AttendanceStatusMaster> GetValueById(int key)
        {
            var result = await _context.AttendanceStatusMasters.FirstOrDefaultAsync(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
