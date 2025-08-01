namespace Easypay_App.Models
{
    public class BenefitStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<BenefitEnrollment>? BenefitEnrollments { get; set; }//to know all BenefitEnrollments that share a particular status

        public BenefitStatusMaster()
        {
            
        }

        public BenefitStatusMaster(int id, string statusName)
        {
            Id = id;
            StatusName = statusName;
        }
    }
}
