using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class AuditTrailActionRepository : RepositoryDb<int,AuditTrailActionMaster>
    {
        public AuditTrailActionRepository(PayrollContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<AuditTrailActionMaster>> GetAllValue()
        {
            return _context.AuditTrailActionMasters.ToList();
        }

        public async override Task<AuditTrailActionMaster> GetValueById(int key)
        {
            var result = await _context.AuditTrailActionMasters.FirstOrDefaultAsync(e => e.Id == key);
            if (result == null)
                throw new NoItemFoundException();
            return result;
        }
    }
}
