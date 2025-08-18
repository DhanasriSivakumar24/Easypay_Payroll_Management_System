using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class PayrollRepository : RepositoryDb<int, Payroll>
    {
        public PayrollRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Payroll>> GetAllValue()
        {
            return await _context.Payrolls.ToListAsync();
        }

        public override async Task<Payroll> GetValueById(int key)
        {
            var item = await _context.Payrolls.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
