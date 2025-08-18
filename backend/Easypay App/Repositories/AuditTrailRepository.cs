using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class AuditTrailRepository : RepositoryDb<int, AuditTrail>
    {
        public AuditTrailRepository(PayrollContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<AuditTrail>> GetAllValue()
        {
            return _context.AuditTrails.ToList();
        }

        public async override Task<AuditTrail> GetValueById(int key)
        {
            var result = await _context.AuditTrails.FirstOrDefaultAsync(e=>e.Id == key);
            if (result == null) 
                throw new NoItemFoundException();
            return result;
        }
    }
}
