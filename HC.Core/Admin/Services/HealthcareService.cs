using HC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HC.DataAccess.Models;
using HC.Core.Helpers;
using HC.Core.Admin.Interfaces;

namespace HC.Core.Admin.Services
{
    public class HealthcareService : IHealthcare
    {
        private readonly HealthcareAppointmentContext _context;
        private readonly JwtHelper _jwtHelper;

        public HealthcareService(HealthcareAppointmentContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtHelper = new JwtHelper(configuration);
        }

        [HttpPost]
        public async Task<List<HealthcareProfessionalModel>> GetHealthcareProfessionalsAsync()
        {
            return await _context.HealthcareProfessionals
                .Include(hp => hp.ProfessionalsSpecialities) // Include the relationship
                .ThenInclude(ps => ps.Speciality) // Navigate to the Specialities table
                .Select(professional => new HealthcareProfessionalModel
                {
                    ProfessionalId = professional.ProfessionalId,
                    Name = professional.ProfessionalName,
                    Specialities = professional.ProfessionalsSpecialities
                        .Select(ps => ps.Speciality.SpecialityName).ToList() // Fetch a list of specialities
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<string> BookAppointmentAsync(AppointmentBookingModel model)
        {
            // Validate: Start time must be in the future
            if (model.StartTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("Start time must be in the future.");
            }

            // Validate: End time must be greater than start time
            if (model.EndTime <= model.StartTime)
            {
                throw new ArgumentException("End time must be greater than start time.");
            }

            // Validate: Professional's schedule should not overlap
            bool isOverlapping = await _context.Appointments
                .AnyAsync(a => a.HealthcareProfessionalId == model.ProfessionalId &&
                               ((model.StartTime >= a.StartTime && model.StartTime < a.EndTime) ||
                                (model.EndTime > a.StartTime && model.EndTime <= a.EndTime)));

            if (isOverlapping)
            {
                throw new InvalidOperationException("The selected professional is not available during the specified time.");
            }

            // Validate: User's schedule should not overlap
            bool isUserOverlapping = await _context.Appointments
                .AnyAsync(a => a.UserId == model.UserId && a.Status.Status == AppointmentStatus.Booked &&
                               ((model.StartTime >= a.StartTime && model.StartTime < a.EndTime) ||
                                (model.EndTime > a.StartTime && model.EndTime <= a.EndTime)));

            if (isUserOverlapping)
            {
                throw new InvalidOperationException("User has already appointment with another healthcare professional during the specified time.");
            }

            DataAccess.Models.AppointmentStatus bookingStatus = await _context.AppointmentStatuses.FirstOrDefaultAsync(f => f.Status.Equals(AppointmentStatus.Booked));
            // Create and save the appointment
            var appointment = new Appointment
            {
                UserId = model.UserId,
                HealthcareProfessionalId = model.ProfessionalId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                StatusId = bookingStatus?.AppointmentStatusId 
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return "Appointment booked successfully!";
        }

        [HttpPost]
        public async Task<List<UserAppointmentModel>> GetAppointmentsByUserIdAsync(int userId)
        {
            return await _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.HealthcareProfessional)
                .Include(a => a.Status)
                .Select(a => new UserAppointmentModel
                {
                    AppointmentId = a.AppointmentId,
                    ProfessionalName = a.HealthcareProfessional.ProfessionalName,
                    Status = a.Status.Status,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<string> CancelAppointmentAsync(AppointmentCancelModel appointmentCancelModel)
        {
            // Fetch the appointment
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentCancelModel.AppointmentId && a.UserId == appointmentCancelModel.UserId);

            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found or does not belong to the user.");
            }

            // Validate: Cannot cancel within 24 hours of the start time
            var timeDifference = appointment.StartTime - DateTime.UtcNow;
            if (timeDifference.TotalHours < 24)
            {
                throw new InvalidOperationException("Appointments cannot be cancelled within 24 hours of the start time.");
            }

            // Update the status to "Cancelled"
            var cancelledStatus = await _context.AppointmentStatuses
                .FirstOrDefaultAsync(s => s.Status == AppointmentStatus.Cancelled);

            if (cancelledStatus == null)
            {
                throw new InvalidOperationException("Cancelled status is not configured in the database.");
            }

            appointment.StatusId = cancelledStatus.AppointmentStatusId;
            await _context.SaveChangesAsync();

            return "Appointment cancelled successfully.";
        }
    }
}
