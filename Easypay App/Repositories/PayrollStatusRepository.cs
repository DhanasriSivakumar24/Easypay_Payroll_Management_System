using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class PayrollStatusRepository : RepositoryDb<int, PayrollStatusMaster>
    {
        public PayrollStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<PayrollStatusMaster> GetAllValue()
        {
            return _context.PayrollStatusMasters.ToList();
        }

        public override PayrollStatusMaster GetValueById(int key)
        {
            var item = _context.PayrollStatusMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
