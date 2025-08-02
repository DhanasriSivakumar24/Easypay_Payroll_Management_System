using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class BenefitsStatusRepository : RepositoryDb<int, BenefitStatusMaster>
    {
        public BenefitsStatusRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<BenefitStatusMaster> GetAllValue()
        {
            return _context.BenefitStatusMasters.ToList();
        }

        public override BenefitStatusMaster GetValueById(int key)
        {
            var item = _context.BenefitStatusMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
