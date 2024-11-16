namespace HC.Core.Models
{
    public class HealthcareProfessionalModel
    {
        public int ProfessionalId { get; set; }
        public string Name { get; set; }
        public List<string> Specialities { get; set; } // Updated to handle multiple specialities
    }
}
