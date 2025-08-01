using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class DepartmentRepository : RepositoryDb<int, DepartmentMaster>
    {
        public DepartmentRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<DepartmentMaster> GetAllValue()
        {
            return _context.DepartmentMasters.ToList();
        }

        public override DepartmentMaster GetValueById(int key)
        {
            var item = _context.DepartmentMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
