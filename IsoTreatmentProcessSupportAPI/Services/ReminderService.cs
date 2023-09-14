using AutoMapper;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface IReminderService
    {
        void Add(int userId, CreateReminderDto dto);
    }
    public class ReminderService : IReminderService
    {
        private readonly IsoSupportDbContext _dbContext;
        private readonly IMapper _mapper;
        public ReminderService(IsoSupportDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void Add(int userId, CreateReminderDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var reminderEntity = _mapper.Map<Reminder>(dto);

            reminderEntity.UserId = userId;

            _dbContext.Reminders.Add(reminderEntity);
            _dbContext.SaveChanges();
        }
    }
}
