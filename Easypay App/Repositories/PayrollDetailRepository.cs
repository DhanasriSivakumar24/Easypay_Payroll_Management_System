using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class PayrollDetailRepository : RepositoryDb<int, PayrollDetail>
    {
        public PayrollDetailRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<PayrollDetail> GetAllValue()
        {
            return _context.PayrollDetails.ToList();
        }

        public override PayrollDetail GetValueById(int key)
        {
            var item = _context.PayrollDetails.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
