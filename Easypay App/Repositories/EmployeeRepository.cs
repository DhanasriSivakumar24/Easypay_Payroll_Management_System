using Easypay_App.Exceptions;
using Easypay_App.Models;
namespace Easypay_App.Repositories
{
    public class EmployeeRepository : Repository<int, Employee>
    {
        public override Employee GetValueById(int key)
        {
            var item = list.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}