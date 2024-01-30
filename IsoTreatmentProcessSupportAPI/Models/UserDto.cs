namespace IsoTreatmentProcessSupportAPI.Models
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Weight { get; set; }
        public int ClimaxDoseInMiligramsPerKilogramOfBodyWeight { get; set; }
        public int DailyDose { get; set; }
    }
}
