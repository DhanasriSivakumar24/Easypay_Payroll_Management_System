using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class PayrollRepository : RepositoryDb<int, Payroll>
    {
        public PayrollRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<Payroll> GetAllValue()
        {
            return _context.Payrolls.ToList();
        }

        public override Payroll GetValueById(int key)
        {
            var item = _context.Payrolls.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
