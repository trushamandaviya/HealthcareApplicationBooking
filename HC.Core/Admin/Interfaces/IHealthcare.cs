using HC.Core.Models;

namespace HC.Core.Admin.Interfaces
{
    public interface IHealthcare
    {
        Task<List<HealthcareProfessionalModel>> GetHealthcareProfessionalsAsync();
        Task<string> BookAppointmentAsync(AppointmentBookingModel model);
        Task<List<UserAppointmentModel>> GetAppointmentsByUserIdAsync(int userId);
        Task<string> CancelAppointmentAsync(AppointmentCancelModel appointmentCancelModel);
    }
}
