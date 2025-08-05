using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class UserRoleRepository : RepositoryDb<int, UserRoleMaster>
    {
        public UserRoleRepository(PayrollContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<UserRoleMaster>> GetAllValue()
        {
            return await _context.UserRoleMasters.ToListAsync();
        }

        public async override Task<UserRoleMaster> GetValueById(int key)
        {
            var item = await _context.UserRoleMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
