using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class RoleRepository : RepositoryDb<int, RoleMaster>
    {
        public RoleRepository(PayrollContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<RoleMaster>> GetAllValue()
        {
            return await _context.RoleMasters.ToListAsync();
        }

        public async override Task<RoleMaster> GetValueById(int key)
        {
            var item = await _context.RoleMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
