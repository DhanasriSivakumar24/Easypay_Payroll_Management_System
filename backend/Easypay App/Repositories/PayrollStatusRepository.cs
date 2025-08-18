using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class PayrollStatusRepository : RepositoryDb<int, PayrollStatusMaster>
    {
        public PayrollStatusRepository(PayrollContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<PayrollStatusMaster>> GetAllValue()
        {
            return await _context.PayrollStatusMasters.ToListAsync();
        }

        public override async Task<PayrollStatusMaster> GetValueById(int key)
        {
            var item = await _context.PayrollStatusMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
