using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class PayrollPolicyRepository : RepositoryDb<int, PayrollPolicyMaster>
    {
        public PayrollPolicyRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<PayrollPolicyMaster>> GetAllValue()
        {
            return await _context.PayrollPolicyMasters.ToListAsync();
        }

        public override async Task<PayrollPolicyMaster> GetValueById(int key)
        {
            var item = await _context.PayrollPolicyMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
