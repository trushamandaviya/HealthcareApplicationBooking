namespace HC.DataAccess.Models;

/// <summary>
/// Main table which has data of the appointments of the user with the professionals
/// </summary>
public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int UserId { get; set; }

    public int HealthcareProfessionalId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int? StatusId { get; set; }

    public virtual HealthcareProfessional HealthcareProfessional { get; set; } = null!;

    public virtual AppointmentStatus? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
