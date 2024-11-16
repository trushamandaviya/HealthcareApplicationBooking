using System;
using System.Collections.Generic;

namespace HC.DataAccess.Models;

/// <summary>
/// Doctors who will provide the treatment to the users
/// </summary>
public partial class HealthcareProfessional
{
    public int ProfessionalId { get; set; }

    public string ProfessionalName { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<ProfessionalsSpeciality> ProfessionalsSpecialities { get; set; } = new List<ProfessionalsSpeciality>();
}
