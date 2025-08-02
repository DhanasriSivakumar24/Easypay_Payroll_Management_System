using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class PayrollPolicyRepository : RepositoryDb<int, PayrollPolicyMaster>
    {
        public PayrollPolicyRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<PayrollPolicyMaster> GetAllValue()
        {
            return _context.PayrollPolicyMasters.ToList();
        }

        public override PayrollPolicyMaster GetValueById(int key)
        {
            var item = _context.PayrollPolicyMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
