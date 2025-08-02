using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;

namespace EasyPay_App.Repositories
{
    public class BenefitsRepository : RepositoryDb<int, BenefitMaster>
    {
        public BenefitsRepository(PayrollContext context) : base(context)
        {
        }

        public override IEnumerable<BenefitMaster> GetAllValue()
        {
            return _context.BenefitMasters.ToList();
        }

        public override BenefitMaster GetValueById(int key)
        {
            var item = _context.BenefitMasters.FirstOrDefault(x => x.Id == key);
            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
