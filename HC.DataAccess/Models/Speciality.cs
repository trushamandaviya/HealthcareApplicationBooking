namespace HC.DataAccess.Models;

/// <summary>
/// Specialities of the healthcare practisners
/// </summary>
public partial class Speciality
{
    public int SpecialityId { get; set; }

    public string SpecialityName { get; set; } = null!;

    public virtual ICollection<ProfessionalsSpeciality> ProfessionalsSpecialities { get; set; } = new List<ProfessionalsSpeciality>();
}
