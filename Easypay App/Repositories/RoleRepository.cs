using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class RoleRepository : RepositoryDb<int, RoleMaster>
    {
        public RoleRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<RoleMaster> GetAllValue()
        {
            return _context.RoleMasters.ToList();
        }

        public override RoleMaster GetValueById(int key)
        {
            var item = _context.RoleMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
