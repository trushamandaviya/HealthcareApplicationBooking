using System;
using System.Collections.Generic;

namespace HC.DataAccess.Models;

/// <summary>
/// Connector table for the healthcare professional and his specialities
/// </summary>
public partial class ProfessionalsSpeciality
{
    public int ProfessionalsSpecialitiesId { get; set; }

    public int HealthcareProfessionalId { get; set; }

    public int SpecialityId { get; set; }

    public virtual HealthcareProfessional HealthcareProfessional { get; set; } = null!;

    public virtual Speciality Speciality { get; set; } = null!;
}
