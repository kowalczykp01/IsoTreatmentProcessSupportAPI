﻿namespace IsoTreatmentProcessSupportAPI.Models
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Weight { get; set; }
        public int ClimaxDoseInMiligramsPerKilogramOfBodyWeight { get; set; }
        public int DailyDose { get; set; }
        public DateTime MedicationStartDate { get; set; }
    }
}
