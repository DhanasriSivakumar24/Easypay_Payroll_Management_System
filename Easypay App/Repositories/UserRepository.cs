using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;

namespace Easypay_App.Repositories
{
    public class UserRepository : RepositoryDb<string, UserAccount>
    {
        public UserRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<UserAccount> GetAllValue()
        {
            return _context.UserAccounts.ToList();
        }

        public override UserAccount GetValueById(string key)
        {
            var item = _context.UserAccounts.FirstOrDefault(x => x.UserName.ToLower() == key.ToLower());
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
