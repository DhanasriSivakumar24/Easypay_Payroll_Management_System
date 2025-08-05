using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class EmployeeRepository : RepositoryDb<int, Employee>
    {
        public EmployeeRepository(PayrollContext context):base(context)
        {
            
        }
        public override async Task<IEnumerable<Employee>> GetAllValue()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.Status)
                .Include(e=>e.UserRole)
                .ToListAsync();
        }

        public override async Task<Employee> GetValueById(int key)
        {
            var result = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.Status)
                .Include(e => e.UserRole)
                .FirstOrDefaultAsync(e => e.Id == key);

            if (result == null)
                throw new NoItemFoundException();

            return result;
        }
    }
}