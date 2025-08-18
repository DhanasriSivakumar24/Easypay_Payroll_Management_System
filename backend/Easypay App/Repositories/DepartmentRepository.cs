using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class DepartmentRepository : RepositoryDb<int, DepartmentMaster>
    {
        public DepartmentRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<DepartmentMaster>> GetAllValue()
        {
            return await _context.DepartmentMasters.ToListAsync();
        }

        public override async Task<DepartmentMaster> GetValueById(int key)
        {
            var item = await _context.DepartmentMasters.FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
