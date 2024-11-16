namespace HC.Core.Models
{
    public class AppointmentBookingModel
    {
        public int UserId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}