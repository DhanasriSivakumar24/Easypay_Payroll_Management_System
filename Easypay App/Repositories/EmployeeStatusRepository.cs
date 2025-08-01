using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class EmployeeStatusRepository : RepositoryDb<int, EmployeeStatusMaster>
    {
        public EmployeeStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<EmployeeStatusMaster> GetAllValue()
        {
            return _context.EmployeeStatusMasters.ToList();
        }

        public override EmployeeStatusMaster GetValueById(int key)
        {
            var item = _context.EmployeeStatusMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
