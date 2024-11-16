namespace HC.Core.Models
{
    public class UserAppointmentModel
    {
        public int AppointmentId { get; set; }
        public string ProfessionalName { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}