using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class EmployeeStatusRepository : RepositoryDb<int, EmployeeStatusMaster>
    {
        public EmployeeStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<EmployeeStatusMaster>> GetAllValue()
        {
            return await _context.EmployeeStatusMasters.ToListAsync();
        }

        public override async Task<EmployeeStatusMaster> GetValueById(int key)
        {
            var item = await _context.EmployeeStatusMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
