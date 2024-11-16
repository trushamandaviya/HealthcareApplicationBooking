using System;
using System.Collections.Generic;

namespace HC.DataAccess.Models;

/// <summary>
/// Represents status of the appointment
/// </summary>
public partial class AppointmentStatus
{
    public int AppointmentStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
