using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class PayrollDetailRepository : RepositoryDb<int, PayrollDetail>
    {
        public PayrollDetailRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<PayrollDetail>> GetAllValue()
        {
            return await _context.PayrollDetails.ToListAsync();
        }

        public override async Task<PayrollDetail> GetValueById(int key)
        {
            var item = await _context.PayrollDetails.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
