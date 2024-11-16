using HC.Core.Admin.Interfaces;
using HC.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HC.API.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthcareController : ControllerBase
    {     
        private readonly IHealthcare _healthcare;

        public HealthcareController(IHealthcare healthcare)
        {
            _healthcare = healthcare;
        }

        //[Authorize] // Ensures this endpoint is accessible only to authenticated users
        [HttpGet]
        [Route("GetHealthcareProfessionals")]
        public async Task<IActionResult> GetHealthcareProfessionals()
        {
            var professionals = await _healthcare.GetHealthcareProfessionalsAsync();
            return Ok(professionals);            
        }

        [HttpPost]
        [Route("BookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentBookingModel model)
        {
            var result = await _healthcare.BookAppointmentAsync(model);
            return Ok(new { Message = result });
            
        }

        [HttpGet("User/{userId}/Appointments")]
        public async Task<IActionResult> GetUserAppointments(int userId)
        {
            var appointments = await _healthcare.GetAppointmentsByUserIdAsync(userId);
            if (!appointments.Any())
            {
                return NotFound(new { Message = "No appointments found for the user." });
            }
            return Ok(appointments);            
        }

        [HttpPost("CancelAppointment")]
        public async Task<IActionResult> CancelAppointment(AppointmentCancelModel appointmentCancelModel)
        {             
            var result = await _healthcare.CancelAppointmentAsync(appointmentCancelModel);
            return Ok(new { Message = result });            
        }
    }
}
