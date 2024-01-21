using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface ITreatmentProcessService
    {
        TreatmentProcessInfoDto GetRemainingTreatmentDays(int userId);
    }
    public class TreatmentProcessService : ITreatmentProcessService
    {
        private readonly IsoSupportDbContext _dbContext;
        public TreatmentProcessService(IsoSupportDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public TreatmentProcessInfoDto GetRemainingTreatmentDays(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var totalTreatmentDays = (user.Weight * user.ClimaxDoseInMiligramsPerKilogramOfBodyWeight) / user.DailyDose;

            var treatmentDaysPassed = (DateTime.Today - user.MedicationStartDate.Date).Days + 1;

            var remainingTreatmentDays = totalTreatmentDays - treatmentDaysPassed;

            var treatmentProcessInfo = new TreatmentProcessInfoDto()
            {
                TotalTreatmentDays = totalTreatmentDays,
                TreatmentDaysPassed = treatmentDaysPassed,
                RemainingTreatmentDays = remainingTreatmentDays
            };

            return treatmentProcessInfo;
        }
    }
}
