using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_App.Repositories
{
    public class BenefitEnrollmentRepository : RepositoryDb<int, BenefitEnrollment>
    {
        public BenefitEnrollmentRepository(PayrollContext context) : base(context)
        {

        }

        public override IEnumerable<BenefitEnrollment> GetAllValue()
        {
            return _context.BenefitEnrollments
                    .Include(b => b.Employee)
                    .Include(b => b.Benefit)
                    .Include(b => b.Status)
                    .ToList();
        }

        public override BenefitEnrollment GetValueById(int key)
        {
            var item = _context.BenefitEnrollments
                    .Include(b => b.Employee)
                    .Include(b => b.Benefit)
                    .Include(b => b.Status)
                    .FirstOrDefault(x => x.Id == key);

            if (item == null)
                throw new NoItemFoundException();
            return item;
        }
    }
}
