using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Repositories
{
    public class EmployeeRepositoryDb : RepositoryDb<int, Employee>
    {
        public EmployeeRepositoryDb(PayrollContext context):base(context)
        {
            
        }
        public override IEnumerable<Employee> GetAllValue()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.Status)
                .Include(e=>e.UserRole)
                .ToList();
        }

        public override Employee GetValueById(int key)
        {
            var result = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.Status)
                .Include(e => e.UserRole)
                .FirstOrDefault(e => e.Id == key);

            if (result == null)
                throw new NoItemFoundException();

            return result;
        }
    }
}