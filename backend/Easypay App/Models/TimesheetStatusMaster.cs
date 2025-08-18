namespace Easypay_App.Models
{
    public class TimesheetStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<Timesheet>? Timesheets { get; set; }
    }
}
