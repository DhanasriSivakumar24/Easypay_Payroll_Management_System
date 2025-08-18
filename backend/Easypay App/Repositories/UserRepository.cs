using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class UserRepository : RepositoryDb<string, UserAccount>
    {
        public UserRepository(PayrollContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<UserAccount>> GetAllValue()
        {
            return await _context.UserAccounts.ToListAsync();
        }

        public async override Task<UserAccount> GetValueById(string key)
        {
            var item = await _context.UserAccounts.FirstOrDefaultAsync(x => x.UserName.ToLower() == key.ToLower());
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
